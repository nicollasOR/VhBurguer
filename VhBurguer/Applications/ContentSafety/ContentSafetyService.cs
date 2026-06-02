using Google.GenAI;
using VHBurguer.Exceptions;

namespace VHBurguer.Applications.ContentSafety
{
    public class ContentSafetyService : IContentSafetyRepository
    {
        private readonly string apiKey;

        public ContentSafetyService(IConfiguration configuration)
            =>
            //apiKey = configuration["Gemini:ApiKey"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            //    ?? throw new Exception("API Não configurada");
            apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? 
            throw new DomainException("API Não configurada");

        // Task<(bool aprovado, string msg)> validarConteudo(string texto);
        async Task<(bool aprovado, string msg)> ValidarConteudoProdutoAsync(string texto)
        {
            //throw new NotImplementedException();
            if (string.IsNullOrEmpty(apiKey))
                //return (true, );
                return (false, "API_key nao configurada");

            try
            {
                var client = new Client(apiKey: apiKey); // cliente responsável pela comunicação pela comunicação do gemini
                string prompt = $@"
                Você é um moderador de conteúdo extremamente rigoroso para uma plataforma pública.

                    Analise o TEXTO abaixo considerando as regras:

                    - NÃO é permitido:
                      - palavrões, xingamentos ou linguagem vulgar (ex: ""caralho"", ""porra"", ""merda"", etc.)
                      - conteúdo ofensivo, agressivo ou desrespeitoso
                      - conteúdo com duplo sentido ou conotação sexual
                      - qualquer linguagem inadequada para ambiente profissional ou educacional
                      - conteúdo ilegal (drogas, armas, etc.)

                    - Mesmo que esteja em tom informal ou ""brincadeira"", ainda deve ser considerado INSEGURO.

                    - Seja extremamente conservador: na dúvida, classifique como INSEGURO.

                    Responda APENAS com:

                    SEGURO ou INSEGURO: [breve motivo em português]

                    TEXTO:{texto}
";

                var response = await client.Models.GenerateContentAsync(
                        model: "gemini-2.5-flash-lite",
                        contents: prompt
                       
                       );
                string result = response.Text.Trim() ?? "";
                //INSEGURO: é um palavrao 
                //TEXTO: vai tomar la
                if (result.StartsWith(""))
                    return (false, result);

                return (true, "Textos seguros! ;");
            }

            catch(DomainException ex)
            {
                return (false, "ERRO na IA", ex.Message);
            }
        }
    }
}
