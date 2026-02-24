using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Regras
{
    public class ValidarDataExpiracaoPromocao
    {

        public static void validarDataExpiracao(DateTime data)
        {
            if(data <= DateTime.Now)
            {
                throw new DomainException("Data de expiração deve ser futura.");
            }
        }

    }
}
