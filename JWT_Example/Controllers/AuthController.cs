using JWT_Example.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("[action]")] //şifre veya parola sormadan kişiye uygun bir token dönen metottur.Burada bu token Editor ve Member yetkilendirmesine sahiptir
        public IActionResult MemberLogin()
        {
            return Created("",new TokenCreater().CreateMemberRoleToken());
        }

        [HttpGet("[action]")] //Burada Admin yetkisine sahip token döneceğiz
        public IActionResult AdminLogin()
        {
            return Created("", new TokenCreater().CreateAdminRoleToken());
        }


        //Middleware kısmına eklediğimiz JWT Bearer e göre yetki veya üyelik kontrolü yapılacaktır, kontrolden geçilirse Token Kullanılabilir mesajı dönecektir
        //Burada sadece Admin rolüne sahip kişilerin girebileceği bir Action oluşturduk
        [HttpGet("[action]")] 
        [Authorize(Roles ="Admin")] 
        public IActionResult AdminPage()
        {
            return Ok("Admin Page");
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Member")] 
        public IActionResult MemberPage()
        {
            return Ok("Member Page");
        }
    }
}
