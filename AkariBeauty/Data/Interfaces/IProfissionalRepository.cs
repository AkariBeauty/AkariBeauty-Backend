using System;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces;

public interface IProfissionalRepository : IGenericoRepository<Profissional>
{
    Task<Profissional> GetByLogin(string login);
    Task<Profissional> GetByCpf(string cpf);
    Task<IEnumerable<Profissional>> GetByServicoId(int servicoId);
    Task<Profissional?> GetWithEmpresaAsync(int profissionalId);
}
