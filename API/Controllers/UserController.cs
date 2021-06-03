using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;
using DAL;

namespace API.Controllers
{
    //post-כל מה שעושה שינויים בדטה בייס
    //get-מה שלא עושה שינויים בדטה בייס
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        UserBL ubl = new UserBL();

        [AcceptVerbs("GET", "POST")]
        [Route("logIn/{username}/{password}")]
        [HttpGet]
        //login 
        public int LogIn(string username, string password)
        {
            return ubl.Login(username, password);
        }

        [AcceptVerbs("GET", "POST")]
        [Route("signUp")]
        [HttpPost]
        public int SignUp(Entities.User u)
        {
            return (ubl.SignUp(u));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateUser")]
        [HttpPost]
        public int UpdateUser(Entities.User u)
        {
            return (ubl.UpdateUser(u));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deleteUser")]
        [HttpPost]
        public int DeleteUser(Entities.User u)
        {
            return (ubl.DeleteUser(u));
        }
    }
}
