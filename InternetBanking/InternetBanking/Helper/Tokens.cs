using Core.Models;
using Core.Services;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InternetBanking.Helper
{
    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string accountNumber, JwIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(accountNumber, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalHours,
                auth = identity.IsAuthenticated,
                account_number = identity.Claims.Single(c => c.Type == "accountNumber").Value,
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
