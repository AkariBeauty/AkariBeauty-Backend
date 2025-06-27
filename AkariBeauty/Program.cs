using AkariBeauty.Authentication;
using AkariBeauty.Controllers;
using AkariBeauty.Data;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Data.Repositories;
using AkariBeauty.Services.Entities;
using AkariBeauty.Services.Interfaces;
using AkariBeauty.Services.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 🔥 REMOVIDO: Configuração de Authentication/JWT

// Configuração do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Colocando todas as rotas em lowercase(letras minusculas)
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});


// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<TimeOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new Microsoft.OpenApi.Any.OpenApiString("00:00:00")
    });

    c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new Microsoft.OpenApi.Any.OpenApiString("2025-12-31")
    });

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Akari Beauty API", Version = "v1" });

    // Configurando botão de Autenticação no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite o token JWT no campo abaixo:\n\nExemplo: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuração do JSON
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Adicionar Controllers
builder.Services.AddControllers().AddApplicationPart(typeof(ServicoController).Assembly);
builder.Services.AddEndpointsApiExplorer();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Para APIs
// Para APIs
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter()); // <-- ADICIONE ESTA LINHA
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Injeção de Dependências
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IServicoAgendamentoRepository, ServicoAgendamentoRepository>();
builder.Services.AddScoped<ICategoriaServicoRepository, CategoriaServicoRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<IProfissionalServicoRepository, ProfissionalServicoRepository>();


builder.Services.AddScoped<IProfissionalServicoService, ProfissionalServicoService>();
builder.Services.AddScoped<IProfissionalService, ProfissionalService>();
builder.Services.AddScoped<ICategoriaServicoService, CategoriaServicoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();
builder.Services.AddScoped<IServicoAgendamentoService, ServicoAgendamentoService>();

// Configuração para escutar todas as interfaces de rede **sem HTTPS**
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Porta HTTP
});

var jwtSettings = new JwtSettings();
builder.Configuration.Bind("JwtSettings", jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddAuthorization();

// Construção do App
var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Akari Beauty API v1");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();
        c.SupportedSubmitMethods(
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get,
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Post,
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Put,
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Delete,
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Patch
        );
    });

}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();