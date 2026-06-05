using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.CategoriaDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;
using Xunit;

namespace VhBurguer.Tests.Services
{
    public class CategoriaServiceTests
    {

        //private readonly 
        [Fact] // tag para teste unitário no xUnit
        public void Adicionar_DeveGerarErro_QuandoEstiverVazio()
        {
            //cria um objeto falso do repositório para simular seu comportamento

            Mock<ICategoriaRepository> respositoryMock = new Mock<ICategoriaRepository>();

            CategoriaService service = new CategoriaService(respositoryMock.Object);

            CriarCategoriaDto categoriaDTO = new CriarCategoriaDto()
            {
                Nome = ""
            };

            Action acao = () => service.Adicionar(categoriaDTO);

            acao.Should() // verifica se a execução da ação lança uma DomainException
                          // contendo a mensagem informada para nome obrigatório
                .Throw<DomainException>()
                .WithMessage("Nome é obrigatório.");
        }

        [Fact]
        public void Adicionar_DeveGerarErro_QuandoJaExistirCategoria()
        {
            Mock<ICategoriaRepository> repositoryMock = new Mock<ICategoriaRepository>();

            //setando um objeto falso com mock
            /*
             Configura o mock para retornar true quando
            o método NomeExiste for chamado com o nome "Lanche"
            Enquanto o setup serve para configurar o comportamento do método mock
            quando o método NomeExiste for chamado com o nome "Lanche
 e qualquer valor for passado para o id It.IsAny<int?> 
            retorna true simulando que a categoria lanche já existe.*/
            repositoryMock
                .Setup(x => x.NomeExiste("Lanche", It.IsAny<int?>()))
                .Returns(true);
            CategoriaService service = new CategoriaService(repositoryMock.Object);

            CriarCategoriaDto categoriaDto = new CriarCategoriaDto()
            {
                Nome = "Lanche"
            };

            Action acao = () => service.Adicionar(categoriaDto);

            acao.Should().Throw<DomainException>().WithMessage("Categoria Já existente.");
        }
    }
}
