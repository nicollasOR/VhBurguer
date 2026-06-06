namespace VHBurguer.Applications.ContentSafety
{
    public interface IContentSafetyRepository
    {
        //public Task<(bool aprovado, string msg)> ValidarConteudoProdutoAsync(string texto); // quando vemos uma função assíncrona, utilizamos ela
        //public string validarALT(string texto);
        //public string gerarDesc(string texto, byte[] img);
        //public Task<string> gerarDescricao(byte[] imgBytes, string nomeArquivo);
        public Task<string> gerarDescricao(IFormFile img);

    }
}
