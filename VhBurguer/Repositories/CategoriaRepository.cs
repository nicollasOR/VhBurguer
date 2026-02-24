using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {

        private readonly Vh_BurguerProfContext _context;

        public CategoriaRepository(Vh_BurguerProfContext context)
        {
            _context = context;
        }

        public List<Categoria> Listar()
        {
            return _context.Categoria.ToList();
        }

        public Categoria ObterPorId(int id)
        {
            Categoria? categoria = _context.Categoria.FirstOrDefault(c => c.CategoriaID == id);

            return categoria;
        }

        public bool NomeExiste(string nome, int? categoriaIdAtual = null)
        {
            var consulta = _context.Categoria.AsQueryable(); // Cria uma consulta inicial na tabela Categoria

            //se foi informado um ID atual 
            // significa que estamos editando uma categoria existente
            // então vamos ignorar própria categoria na verificação
            if (categoriaIdAtual.HasValue)
            {
                consulta = consulta.Where(categoria => categoria.CategoriaID != categoriaIdAtual.Value);
            }
            return consulta.Any(categoria => categoria.Nome == nome);
        }

        public void Adicionar(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            _context.SaveChanges();
        }

        public void Atualizar(Categoria categoria)
        {
            Categoria? categoriaBanco = _context.Categoria.FirstOrDefault(categoria => categoria.CategoriaID == categoria.CategoriaID);
            if (categoriaBanco == null)
            { 
                return; 
            }

            categoriaBanco.Nome = categoria.Nome;
            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Categoria? categoriaBanco = _context.Categoria.FirstOrDefault(categoria => categoria.CategoriaID == id);
            if (categoriaBanco == null)
            {
                return;
            }

            _context.Categoria.Remove(categoriaBanco);
            _context.SaveChanges();
                    
        }   


    }
}
