using VHBurguer.Domains;
using VHBurguer.DTOs.CategoriaDto;
using VHBurguer.DTOs;
using VHBurguer.Interfaces;
using VHBurguer.Exceptions;


namespace VHBurguer.Applications.Services
{
    public class CategoriaService
    {

        private readonly ICategoriaRepository _repository;
        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<LerCategoriaDto> Listar()
        {
            List<Categoria> categorias = _repository.Listar();
            List<LerCategoriaDto> categoriaDto = categorias.Select(categoria => new LerCategoriaDto
            {
                CategoriaId = categoria.CategoriaID,
                Nome = categoria.Nome
            }).ToList();

            return categoriaDto;
        }

        /*
         
        public List<LerCategoriaDto> Listar()
{
    List<Categoria> categorias = _repository.Listar();
    // Converte cada categoria para LerCategoriaDto
    List<LerCategoriaDto> categoriaDto = categorias.Select(categoria => new LerCategoriaDto
    {
        CategoriaId = categoria.CategoriaID,
        Nome = categoria.Nome,
    }).ToList();

    // Retorna a lista já convertida em DTO
    return categoriaDto;
}
         
         */

        public LerCategoriaDto ObterPorId(int id)
        {
            Categoria categoria = _repository.ObterPorId(id);
            if(categoria == null)
                throw new DomainException("Categoria não encontrada");
            LerCategoriaDto categoriaDto = new LerCategoriaDto
            {
                CategoriaId = categoria.CategoriaID,
                Nome = categoria.Nome,
            };
            return categoriaDto;
        }

        private static void validarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                throw new DomainException("Nome é obrigatório");
        }

        public void Adicionar(CriarCategoriaDto criarDto)
        {
            validarNome(criarDto.Nome);

            if (_repository.NomeExiste(criarDto.Nome))
                {
                throw new DomainException("Categoria já existente"); 
            }


        Categoria categoria = new Categoria
        {
            Nome = criarDto.Nome
        };

            _repository.Adicionar(categoria);
        }

        public void Atualizar(int id, CriarCategoriaDto criarDto)
        {
            validarNome(criarDto.Nome);

            Categoria categoriaBanco = _repository.ObterPorId(id);

            if (categoriaBanco == null)
                throw new DomainException("Categoria não encontrada");

            if (_repository.NomeExiste(criarDto.Nome, categoriaIdAtual: id))
                throw new DomainException("Já existe outra categoria com esse nome.");
            categoriaBanco.Nome = criarDto.Nome;
            _repository.Atualizar(categoriaBanco);
                
        }

        public void Remover(int id)
        {
            Categoria catergoriaBanco = _repository.ObterPorId(id);
            if (catergoriaBanco == null)
                throw new DomainException("Categoria não encontrada");

            _repository.Remover(id);
        
        }


       


    }
}
