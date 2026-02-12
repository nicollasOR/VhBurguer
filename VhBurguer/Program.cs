using Microsoft.Data;
using Microsoft.EntityFrameworkCore;
using VHBurguer.Applications.Services;
using VHBurguer.Contexts;
using VHBurguer.Repositories;
using VHBurguer.Interfaces;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adicionando o banco na program
builder.Services.AddDbContext<Vh_BurguerProfContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();//IUsuarioRepository
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
