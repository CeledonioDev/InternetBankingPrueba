﻿using Core.Models;
using Core.Services;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string accountNumber, ClaimsIdentity identity)
        {
            var claims = new[]
         {
                 new Claim(JwtRegisteredClaimNames.Sub, accountNumber),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Helper.Constants.Strings.JwtClaimIdentifiers.AccountNumber),
                 identity.FindFirst(Helper.Constants.Strings.JwtClaimIdentifiers.Id),
                 identity.FindFirst(Helper.Constants.Strings.JwtClaimIdentifiers.Email),
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string accountNumber, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(accountNumber, "Token"), new[]
            {
                new Claim(Helper.Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Helper.Constants.Strings.JwtClaimIdentifiers.AccountNumber, accountNumber),
            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
        private static void ThrowIfInvalidOptions(JwIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwIssuerOptions.ValidFor));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwIssuerOptions.JtiGenerator));
            }
        }
    }
}
