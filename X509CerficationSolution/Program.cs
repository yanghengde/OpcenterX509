using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace X509CerficationSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            X509Certificate2 certificate = new X509Certificate2("C:\\Mendix\\UAIntegration.pfx", "SwqaMe$1");
            // Create x.509 signed token
            var now = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();

            // certificate was previously loaded either from the filesystem or from the X509Store(see code examples above)
            X509SigningCredentials x509SigningCredentials = new X509SigningCredentials(certificate);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
            new ClaimsIdentity(
            new Claim[]
            {new Claim(ClaimTypes.Name, certificate.Issuer),new Claim(ClaimTypes.Thumbprint, certificate.Thumbprint),new Claim("urn:realm", "x.509"),}),
                Issuer = "urn:unifiedoauth",
                Audience = "urn:unified",
                Expires = now.AddMinutes(2),
                SigningCredentials = x509SigningCredentials
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
        }
    }
}
