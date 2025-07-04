﻿using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IUsuarioRepository : IGenericoRepository<Usuario>
    {
        Task<Usuario> GetByLogin(string login);
    }
}
