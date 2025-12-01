using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AkariBeauty.Objects.Dtos.Agendamentos
{
    public class DisponibilidadeDiaDTO
    {
        [JsonPropertyName("date")]
        public DateOnly Data { get; set; }

        [JsonPropertyName("slots")]
        public IEnumerable<string> Horarios { get; set; } = Array.Empty<string>();
    }
}
