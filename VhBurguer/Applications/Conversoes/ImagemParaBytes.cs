using VHBurguer.Domains;

namespace VHBurguer.Applications.Conversoes
{
    public class ImagemParaBytes
    {

        public static byte[] ConverterImagem(IFormFile Imagem)
        {
            using var ms = new MemoryStream();
            Imagem.CopyTo(ms);
            return ms.ToArray();
        }

    }
}
