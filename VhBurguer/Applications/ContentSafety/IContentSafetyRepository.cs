namespace VHBurguer.Applications.ContentSafety
{
    public interface IContentSafetyRepository
    {
                 //aprovado -> o texto foi aprovado ou não
                 // msg -> avio da recusa do texto
        Task<(bool aprovado, string msg)> ValidarConteudoProdutoAsync(string texto); // quando vemos uma função assíncrona, utilizamos ela




    }
}
