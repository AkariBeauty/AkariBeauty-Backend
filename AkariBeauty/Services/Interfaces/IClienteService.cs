﻿using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Services.Interfaces
{
    public interface IClienteService : IGenericoService<Cliente, ClienteDTO>
    {
        Task<string> Login(RequestLoginDTO request);
    }
}
