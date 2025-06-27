using System;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Services.Entities;

public class ServicoAgendamentoService : IServicoAgendamentoService
{
    private readonly IServicoAgendamentoRepository _repository;
    private readonly IProfissionalServicoRepository _proserico;
    private readonly IProfissionalRepository _profissional;
    private readonly IAgendamentoRepository _agendamento;
    private readonly IServicoRepository _servico;

    public ServicoAgendamentoService(IServicoAgendamentoRepository repository, IProfissionalServicoRepository proserico, IProfissionalRepository profissional, IAgendamentoRepository agendamento, IServicoRepository servico)
    {
        this._repository = repository;
        this._proserico = proserico;
        this._profissional = profissional;
        this._agendamento = agendamento;
        this._servico = servico;
    }

    public async Task DesvincularServicoDoAgendamento(int agendamentoId, int servicoId)
    {

        await _repository.Delete(agendamentoId, servicoId);

        var agendamento = await _agendamento.GetById(agendamentoId);
        var profissional = await _profissional.GetById(agendamento.ProfissionalId);

        if (profissional == null) 
            throw new ArgumentException("Profissional nao encontrado");

        var profissional_servico = await _proserico.GetProfissionalAndServico(profissional.Id, servicoId);

        var servico = await _servico.GetById(servicoId);
        var comissao = profissional_servico.Comissao;
        var valor_base = servico.ValorBase;

        var soma = comissao + valor_base;
        agendamento.Comissao -= comissao;
        agendamento.Valor -= soma;

        await _agendamento.SaveChanges();
    }

    public async Task VincularServicoAoAgendamento(int agendamentoId, int servicoId)
    {
        // 01 - Fazer o vinculo de servico e agendamento.
        await _repository.Add(agendamentoId, servicoId);

        // 02 - Pesquisar o ProfissionalSerico - Pegar o atributo COMISÃO
        var agendamento = await _agendamento.GetById(agendamentoId);
        var profissional = await _profissional.GetById(agendamento.ProfissionalId);

        if (profissional == null)
            throw new ArgumentException("Profissional nao encontrado");

        var profissional_servico = await _proserico.GetProfissionalAndServico(profissional.Id, servicoId);

        // 03 - Pesquisar o Servico - Par o atributo VALOR_BASE
        var servico = await _servico.GetById(servicoId);
        var comissao = profissional_servico.Comissao;
        var valor_base = servico.ValorBase;

        // 04 - Somar esses dois valores(COMISSÃO + VALOR_BASE)
        var soma = comissao + valor_base;

        // 05 - Pesquisar pelo agendamento, somar o valor anterior com o valor do agendamento.
        agendamento.Comissao += comissao;
        agendamento.Valor += soma;

        // 06 - Atualizar o agendamento
        await _agendamento.SaveChanges();
    }
}
