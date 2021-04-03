using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Example.Utilities
{
    public class TokenCreater
    {
        public string CreateMemberRoleToken()
        {

            var codingKey = Encoding.UTF8.GetBytes("eraybakırberkaybakır"); //UTF8 formatında key imizi oluşturduk.Middleware tarafındaki ile aynı olması gerekiyor yoksa token geçersiz kabul edilir

            SymmetricSecurityKey key = new SymmetricSecurityKey(codingKey); //Credential için simetrik bir key oluşturduk

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Tokenin imzası için kullanacağımız yapıyı key imiz ve belirlediğimiz şifreleme algoritması ile oluşturduk


            //Kullanıcı hakkında ki bilgileri userId,email,username,Rolleri vs... tokene eklemek için belirttiğimiz liste
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"Member"),
                new Claim(ClaimTypes.Role,"Editor"),
                new Claim(ClaimTypes.Name,"MemberUserName"), //User.Identity.Name ile kullanıcı adına ulaşabilmek için claimslere ekledik.Kişiye özel işlemler yapmak adına gerekiyor
                new Claim("Şehir","İstanbul") //Özel bir Key ile oluşturmuş olduğumuz Claim nesnesine User.Claims.Where(i=>i.Type==Şehir).FirstOrDefault(); ile ulaşabilirim
            };



            JwtSecurityToken securityToken = new JwtSecurityToken //Tokenin özelliklerini belirtiyorum
                (
                     issuer: "http://localhost", //Üreten bilgisi
                     audience: "http://localhost", //Tüketen bilgisi
                     notBefore: DateTime.Now, //Hangi zamandan önce geçerliliğini yitirsin
                     expires: DateTime.Now.AddMinutes(5), //Ne kadar süre sonra geçerliliğini yitirsin
                     signingCredentials: signingCredentials, //Key ve şifreleme algoritmamız eşliğinde oluştuduğumuz imzayı token'e veriyoruz
                     claims: claims
                );


            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler(); //Token'ı oluşturacak sınıfımız
            var token = handler.WriteToken(securityToken); //Verdiğimiz özellikler neticesinde token i oluşturduk

            return token;

        }

        public string CreateAdminRoleToken()
        {
            var codingKey = Encoding.UTF8.GetBytes("eraybakırberkaybakır");

            SymmetricSecurityKey key = new SymmetricSecurityKey(codingKey);

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Name,"AdminUserName")
            };

            JwtSecurityToken securityToken = new JwtSecurityToken
                (
                     issuer: "http://localhost",
                     audience: "http://localhost",
                     notBefore: DateTime.Now,
                     expires: DateTime.Now.AddMinutes(5),
                     signingCredentials: signingCredentials,
                     claims: claims
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(securityToken);

            return token;
        }
    }
}
