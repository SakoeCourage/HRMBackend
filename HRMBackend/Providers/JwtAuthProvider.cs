using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMBackend.Providers
{
    public enum AuthorizationDecisionType
    {
        Applicant,
        SystemUser
    }

    public class JwtAuthProvider
    {
        private readonly string key;

        public JwtAuthProvider(string key)
        {
            this.key = key;
        }

        public string Authenticate(int id, AuthorizationDecisionType decisionType)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.AuthorizationDecision, decisionType.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
