namespace AkariBeauty.Data.Interfaces
{
    public interface IAuthRepository
    {
        // Método para autenticar usuários
        string Authenticate(string username, string password);

        //Métodos relacionados à autenticação em geral
        Task<string> ValidateCredentialsAsync(string expiredToken);
        Task<bool> GetUserRoleAsync(string token);
    }
}
