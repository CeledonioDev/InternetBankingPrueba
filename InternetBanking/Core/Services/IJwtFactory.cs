using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string accountNumber, ClaimsIdentity identity);

        ClaimsIdentity GenerateClaimsIdentity(string accountNumber, string id);
    }
}
