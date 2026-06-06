using Google.GenAI;
using Google.GenAI.Types;
using System.Threading.Tasks;
using VHBurguer.Exceptions;
using DotNetEnv;

namespace VHBurguer.Applications.ContentSafety
{
    public class ContentSafetyService : IContentSafetyRepository
    {
        private readonly string apiKey;

        public ContentSafetyService(IConfiguration configuration)
            =>
            //apiKey = configuration["Gemini:ApiKey"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            //    ?? throw new Exception("API Não configurada");
            //apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ??
            //throw new DomainException("API Não configurada");

            apiKey = System.Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new DomainException("API Não configurada");

        private static string DetectarTipoMime(string nomeArquivo)
        {
            string extensao = Path.GetExtension(nomeArquivo).ToLower();

            return extensao switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };
        }

//        public async Task<(bool aprovado, string msg)> ValidarConteudoProdutoAsync(string texto)
//        {
//            //throw new NotImplementedException();
//            if (string.IsNullOrEmpty(apiKey))
//                //return (true, );
//                return (false, "API_key nao configurada");

//            try
//            {
//                var client = new Client(apiKey: apiKey); // cliente responsável pela comunicação pela comunicação do gemini
//                string prompt = $@"
//                Você é um moderador de conteúdo extremamente rigoroso para uma plataforma pública.

//                    Analise o TEXTO abaixo considerando as regras:

//                    - NÃO é permitido:
//                      - palavrões, xingamentos ou linguagem vulgar (ex: ""caralho"", ""porra"", ""merda"", etc.)
//                      - conteúdo ofensivo, agressivo ou desrespeitoso
//                      - conteúdo com duplo sentido ou conotação sexual
//                      - qualquer linguagem inadequada para ambiente profissional ou educacional
//                      - conteúdo ilegal (drogas, armas, etc.)

//                    - Mesmo que esteja em tom informal ou ""brincadeira"", ainda deve ser considerado INSEGURO.

//                    - Seja extremamente conservador: na dúvida, classifique como INSEGURO.

//                    Responda APENAS com:

//                    SEGURO ou INSEGURO: [breve motivo em português]

//                    TEXTO:{texto}
//";

//                var response = await client.Models.GenerateContentAsync(
//                        model: "gemini-2.5-flash-lite",
//                        contents: prompt

//                       );
//                string result = response.Text.Trim() ?? "";
//                //INSEGURO: é um palavrao 
//                //TEXTO: vai tomar la
//                if (result.StartsWith(""))
//                    return (false, result);

//                return (true, "Textos seguros! ;");
//            }

//            catch (DomainException ex)
//            {
//                return (false, "ERRO na IA", ex.Message);
//            }
//        }

        public async Task<string> gerarDescricao(IFormFile imagem)
        {
            string promptText = @"
                Analise esta imagem de um produto e gere uma descrição detalhada em português.
                
                A descrição deve incluir:
                - O que é o produto
                - Características principais (cor, tamanho, material)
                - Possíveis usos
                - Qualidade observada
                
                Seja conciso mas informativo (utilize no máximo 20 palavras).
            ";

            try
            {
                if (imagem == null || imagem.Length == 0)
                    throw new DomainException("Imagem não pode estar vazia");

                string mimeType = imagem.ContentType ?? "image/jpeg";

                //string base64Imagem;
                //using (var memoryStream = new MemoryStream())
                //{
                //    await imagem.CopyToAsync(memoryStream);
                //    byte[] imagemBytes = memoryStream.ToArray();
                //    base64Imagem = Convert.ToBase64String(imagemBytes);
                //}

                byte[] imagemBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await imagem.CopyToAsync(memoryStream);
                    imagemBytes = memoryStream.ToArray();
                }

                var client = new Client(apiKey: apiKey);

                
                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite",
                    contents: new List<Content>
                    {
                        new Content
                        {
                            Parts = new List<Part>
                            {
                                new Part { Text = promptText },
                                new Part
                                {
                                    InlineData = new Blob
                                    {
                                        MimeType = mimeType,
                                        Data = imagemBytes
                                    }
                                }
                            }
                        }
                    }
                );

                return response.Text?.Trim() ?? "Descrição não gerada pelo modelo.";
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DomainException($"Erro ao analisar imagem com o Gemini: {ex.Message}");
            }
        }
        }
}


       /*teste anterior public async Task<string> gerarDescricao(IFormFile imagem)
        {
            string promptText = @"
                        Analise esta imagem de um produto e gere uma descrição detalhada em português.
                        
                        A descrição deve incluir:
                        - O que é o produto
                        - Características principais (cor, tamanho, material)
                        - Possíveis usos
                        - Qualidade observada
                        
                        Seja conciso mas informativo (2-3 parágrafos).
                    ";
            try
            {
                if (imagem == null || imagem.Length == 0)
                    throw new DomainException("Imagem não pode estar vazia");

                string mimeType = imagem.ContentType ?? "image/jpeg";

                byte[] imagemBytes = new byte[imagem.Length];
                using (var stream = imagem.OpenReadStream())
                {
                    await stream.ReadAsync(imagemBytes);
                }

                string base64Imagem = Convert.ToBase64String(imagemBytes);

                var client = new Client(apiKey: apiKey);

                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite",
                    //contents: new ContentInput
                    contents: new List<ContentInput>
                    {
                        Parts = new List<Part>
                        {
                    new Part { Text = promptText },
                    new Part
                    {
                        InlineData = new Blob
                        {
                            MimeType = mimeType,
                            Data = base64Imagem
                        }
                    }
                        }
                    }
                );

                return response.Text?.Trim() ?? "Descrição não gerada";
            }
            catch (DomainException ex)
            {
                throw new DomainException($"Erro ao analisar imagem: {ex.Message}");
            }
        }









    }



}

//public async Task<string> gerarDescricao(byte[] img, string nomeArquivo)
//{
//    try
//    {
//        if (img.Length == 0 || img.Length == null)
//            throw new DomainException("A imagem pode estar vazia não meu fio");
//        string img64 = Convert.ToBase64String(img);
//        string tipoMime = DetectarTipoMime(nomeArquivo);

//        var client = new Client(apiKey: apiKey);
//        string prompt = $@"
//            Analise a imagem e gere uma descrição curta para catálogo de produtos.

//            Regras:
//            - Máximo de 50 palavras.
//            - Foque apenas no produto principal.
//            - Destaque características visuais observáveis.
//            - Use linguagem clara e profissional.
//            - Não faça suposições sobre marca, especificações técnicas ou funcionalidades não visíveis.
//            - Retorne apenas o texto da descrição.
//                                                    ";

//        var response = await client.Models.GenerateContentAsync(
//            model: "gemini-2.5-flash-lite",
//            contents: new ContentInput
//            {
//                Parts = new Part[]
//                {
//                new Part {Text = prompt},
//                new Part
//                {
//            InlineData = new Blob
//            {
//                MimeTypes = tipoMime,
//                Data = img64
//            }

//                }
//                }
//            }

//            );
//        string descricao = response.Text?.Trim() ?? "Descrição não gerada";

//        if (string.IsNullOrEmpty(descricao))
//        {
//            throw new DomainException("A descrição não pôde ser gerada");
//            return descricao;
//        }
//    }

//    catch(DomainException ex)
//    {
//        throw new DomainException($"Erro ao acessar a imagem {ex.Message}");
//    }


//}

        */

