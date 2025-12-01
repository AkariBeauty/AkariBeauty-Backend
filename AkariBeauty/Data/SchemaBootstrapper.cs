using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data;

public static class SchemaBootstrapper
{
    public static void EnsureLatestSchema(AppDbContext context)
    {
        // Garantir que o banco exista (cria tudo se estiver vazio)
        context.Database.EnsureCreated();

        const string addObservacaoColumn = "ALTER TABLE IF EXISTS agendamento ADD COLUMN IF NOT EXISTS observacao text NULL;";
        const string addProfissionalColumn = "ALTER TABLE IF EXISTS agendamento ADD COLUMN IF NOT EXISTS profissional_id integer NULL;";

        context.Database.ExecuteSqlRaw(addObservacaoColumn);
        context.Database.ExecuteSqlRaw(addProfissionalColumn);

        const string addProfissionalForeignKey = @"DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_agendamento_profissional'
          AND table_name = 'agendamento'
    ) THEN
        ALTER TABLE agendamento
        ADD CONSTRAINT fk_agendamento_profissional
        FOREIGN KEY (profissional_id)
        REFERENCES profissional(id)
        ON DELETE NO ACTION;
    END IF;
END $$;";

        context.Database.ExecuteSqlRaw(addProfissionalForeignKey);

        var profissionalServicoSeedData = new (string Login, int ServicoId, float Comissao, string Tempo)[]
        {
            ("livia.andrade", 1, 0.15f, "00:50:00"),
            ("livia.andrade", 4, 0.18f, "01:30:00"),
            ("marcos.diniz", 2, 0.12f, "00:40:00"),
            ("marcos.diniz", 8, 0.10f, "01:00:00"),
            ("renata.freitas", 3, 0.15f, "01:00:00"),
            ("renata.freitas", 10, 0.20f, "01:45:00"),
            ("camila.prado", 5, 0.14f, "01:10:00"),
            ("camila.prado", 9, 0.10f, "00:40:00"),
            ("diego.santana", 6, 0.12f, "00:35:00"),
            ("diego.santana", 1, 0.10f, "00:45:00"),
            ("elaine.costa", 5, 0.15f, "01:15:00"),
            ("felipe.duarte", 7, 0.12f, "00:50:00"),
        };

        foreach (var item in profissionalServicoSeedData)
        {
            var sql = $@"
INSERT INTO profissional_servico (profissional_id, servico_id, comissao, tempo)
SELECT p.id, {item.ServicoId}, {item.Comissao.ToString(CultureInfo.InvariantCulture)}, '{item.Tempo}'::time
FROM profissional p
WHERE p.login = '{item.Login}'
  AND NOT EXISTS (
      SELECT 1 FROM profissional_servico ps
      WHERE ps.profissional_id = p.id AND ps.servico_id = {item.ServicoId}
  );";

            context.Database.ExecuteSqlRaw(sql);
        }
    }
}
