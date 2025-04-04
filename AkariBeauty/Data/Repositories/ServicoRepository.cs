using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using System;

namespace AkariBeauty.Data.Repositories
{
    public class ServicoRepository : GenericoRepository<Servico>, IServicoRepository
    {

        private readonly AppDbContext _context;

        public ServicoRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
