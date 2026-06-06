using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VHBurguer.Applications.Autenticacao;
using VHBurguer.Applications.ContentSafety;
using VHBurguer.Applications.Services;
using VHBurguer.Contexts;
using VHBurguer.Interfaces;
using VHBurguer.Repositories;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//GEMINI_API_KEY = AQ.Ab8RN6JFQnaUZ7hDcnVvdMoXH4pXCXImWBuYY1jvvAtJkL4ezQ

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    // Esta linha impede o loop infinito e o travamento
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
})
    ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Value: Bearer TokenJWT"
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
            new string[] {}
        }
    });
});

// chamar nossa conexão com o banco aqui na program
builder.Services.AddDbContext<VH_BurguerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
/*
   "nome": "123",
  "email": "123@gmail.com"
 */
// Usuário
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

// Usuário
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ProdutoService>();

//JWT
builder.Services.AddScoped<GeradorTokenJWT>();
builder.Services.AddScoped<AutenticacaoService>();

//Categoria
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();

//Promocao
builder.Services.AddScoped<IPromocaoRepository, PromocaoRepository>();
builder.Services.AddScoped<PromocaoService>();

//Log alteracao
builder.Services.AddScoped<ILogAlteracaoProdutoRepository, LogAlteracaoRepository>();
builder.Services.AddScoped<Log_AlteracaoProdutoService>();
// Registra o seu novo serviço do Gemini no container de Injeção de Dependência
//builder.Services.AddScoped<IContentSafetyRepository>();
//builder.Services.AddScoped<ContentSafetyService>();
// Registra o seu novo serviço do Gemini no container de Injeção de Dependência
builder.Services.AddScoped<IContentSafetyRepository, ContentSafetyService>();

// Configura o sistema de autenticação da aplicação.
// Aqui estamos dizendo que o tipo de autenticação padrão será JWT Bearer.
// Ou seja: a API vai esperar receber um Token JWT nas requisições.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    // Adiciona o suporte para autenticação usando JWT.
    .AddJwtBearer(options =>
    {
        // Lê a chave secreta definida no appsettings.json.
        // Essa chave é usada para ASSINAR o token quando ele é gerado
        // e também para VALIDAR se o token recebido é verdadeiro.
        var chave = builder.Configuration["Jwt:Key"]!;

        // Quem emitiu o token (ex: nome da sua aplicação).
        // Serve para evitar aceitar tokens de outro sistema.
        var issuer = builder.Configuration["Jwt:Issuer"]!;

        // Para quem o token foi criado (normalmente o frontend ou a própria API).
        // Também ajuda a garantir que o token pertence ao seu sistema.
        var audience = builder.Configuration["Jwt:Audience"]!;

        // Define as regras que serão usadas para validar o token recebido.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Verifica se o emissor do token é válido
            // (se bate com o issuer configurado).
            ValidateIssuer = true,

            // Verifica se o destinatário do token é válido
            // (se bate com o audience configurado).
            ValidateAudience = true,

            // Verifica se o token ainda está dentro do prazo de validade.
            // Se já expirou, a requisição será negada.
            ValidateLifetime = true,

            // Verifica se a assinatura do token é válida.
            // Isso garante que o token não foi alterado.
            ValidateIssuerSigningKey = true,

            // Define qual emissor é considerado válido.
            ValidIssuer = issuer,

            // Define qual audience é considerado válido.
            ValidAudience = audience,

            // Define qual chave será usada para validar a assinatura do token.
            // A mesma chave usada na geração do JWT deve estar aqui.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(chave)
            )
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
}
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();