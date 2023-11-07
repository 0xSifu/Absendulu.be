using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AbsenDulu.BE.Token.Helper
{
    public class JwtHelper
    {
        public static JwtSecurityToken GetJwtToken(
            string username,
            string userid,
            string signingkey,
            string issuer,
            string audience,
            TimeSpan expiration,
            string companyid,
            string companyname,
            string role,
            Claim[]? additionalClaims = null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", userid),
                new Claim("company_id", companyid),
                new Claim("company_name", companyname),
                new Claim(ClaimTypes.Role, role)
            };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
        }
    }
}