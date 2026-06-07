namespace VHBurguer.Applications.ContentSafety
{
    public interface IContentSafetyRepository
    {
        public Task<(bool aprovado, string msg)> ValidarConteudoProdutoAsync(string texto); // quando vemos uma função assíncrona, utilizamos ela

        public Task<string> gerarDescricao(IFormFile img);

    }
}
