using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class LogAlteracaoRepository : ILogAlteracaoProdutoRepository
    {
        private readonly Vh_BurguerProfContext _context;
        public LogAlteracaoRepository(Vh_BurguerProfContext context)
        {
            _context = context;
        }

        public List<Log_AlteracaoProduto> Listar()
        {
            List<Log_AlteracaoProduto> log = _context.Log_AlteracaoProduto.OrderByDescending(log_ => log_.DataAlteracao).ToList();
            return log;
        }

        public List<Log_AlteracaoProduto> listarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> log_ = _context.Log_AlteracaoProduto.Where
                (log__ => log__.ProdutoID == produtoId).OrderByDescending
                (log___ => log___.DataAlteracao).ToList();
            return log_;
        }






    }
}
