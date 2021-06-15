using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;
using Entities;


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
        [Route("logIn/{username}/resetPassword")]
        [HttpGet]
        // reseting by username + email 
        public int ForgotPassword(string username)
        {
            return ubl.ResetPassword(username);
        }

        [AcceptVerbs("GET", "POST")]

        [Route("logIn/confirmValcode/{username}/{valcode}")]
        [HttpGet]
        // mailed valditaion code confirmation. returns ucode if succeds, if not =0
        public int ConfirmValCode(string username, string valcode)
        {
            return ubl.ConfirmValCode(username, valcode);
        }

        [AcceptVerbs("GET", "POST")]
        [Route("logIn/setNewPassword/{ucode}/{newPassword}")]
        [HttpGet]
        // setting a new password 
        public int NewPassword(int usercode, string newPassword)
        {
            return ubl.NewPassword(usercode, newPassword);
        }

        [AcceptVerbs("GET", "POST")]
        [Route("signUp")]
        [HttpPost]
        public int SignUp(User u)
        {
            return (ubl.SignUp(u));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("updateUser")]
        [HttpPost]
        public int UpdateUser(User u)
        {
            return (ubl.UpdateUser(u));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("deleteUser")]
        [HttpPost]
        public int DeleteUser(User u)
        {
            return (ubl.DeleteUser(u));
        }
    }
}
