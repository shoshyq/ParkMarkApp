using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Entities;


namespace BL
{
    public class Email
    {
        // mailing  user about prking spot hungarian has found him
        public static void MailToUserFoundParkSpot(PSpotHandler pspot, PSpotSearchHandler psearch)
        {
            try
            {
                string email = "parkmarkapp2021@gmail.com";
                string password = "parkmark0583249977";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                var pss = DbHandler.GetAll<ParkingSpotSearch>().First(u => u.Code == psearch.Code);
                var user = DbHandler.GetAll<User>().First(u => u.Code == pss.UserId);
                var psp = DbHandler.GetAll<ParkingSpot>().First(u => u.Code == pspot.Code);
                var spotowner = DbHandler.GetAll<User>().First(u => u.Code == psp.UserCode);
                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(user.UserEmail));

                msg.Subject = "חניה להשכרה קבועה";
                //LinkedResource res = new LinkedResource(DTO.StartPoint.Liraz + "DAL\\Files\\icon.png");
                //res.ContentId = Guid.NewGuid().ToString();
                #region buildHtmlMessageBody
                string htmlBodyString = string.Format(
                      @"
                       <div style='  direction: rtl;
                                     background-color: #EE8989;
                                     font-family: Amerald;
                                     font-size:medium; '>
                           <div style='text-align:center'>
                               <h1>שלום!</h1>
                               <h3>נמצאה עבורך חניה</h3>
                           </div>
                           <div style='  position: relative;
                                         padding: 0.75rem 1.25rem;
                                         margin-bottom: 1rem;
                                         margin-left: 7%;
                                         margin-right: 7%;
                                         border: 1px solid #5c060e;
                                         border-radius: 0.25rem;
                                         color: #5c060e;
                                         width: 75%;
                                         background-color: #f5c9c9;
                                         border-radius: 5px; '>
                               <label> address : {0}</label>
                               <br />
                               <label> price per hour : {1}</label>
                               <br />
                               <label> owner's username: : {2}</label>
                               <br />
                               <label> for more information open Your ParkMark app </label>
                               <br />
                             
                               <br />
                           </div>",

                         pspot.Address, pspot.PricePerHour, spotowner.Username);





                #endregion
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBodyString, null, MediaTypeNames.Text.Html);
                //alternateView.LinkedResources.Add(res);
                msg.AlternateViews.Add(alternateView);
                msg.IsBodyHtml = true;
                //msg.Attachments.Add(new Attachment(doctor.pictureDiploma));
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {


                throw ex;
            }
        }
        //mailing user that his rating is very low and we had to delete him from the app db
        public static void MailToUserDeletingUser(User user)
        {
            try
            {
                string email = "parkmarkapp2021@gmail.com";
                string password = "parkmark0583249977";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(user.UserEmail));
                Feedback user_f = DbHandler.GetAll<Feedback>().First(f => f.DescriptedUserCode == user.Code);

                msg.Subject = "הודעה על מחיקת חשבונך מאפליקציה";
                //LinkedResource res = new LinkedResource(DTO.StartPoint.Liraz + "DAL\\Files\\icon.png");
                //res.ContentId = Guid.NewGuid().ToString();
                #region buildHtmlMessageBody
                string htmlBodyString = string.Format(
                      @"
                       <div style='  direction: rtl;
                                     background-color: #EE8989;
                                     font-family: Amerald;
                                     font-size:medium; '>
                           <div style='text-align:center'>
                               <h1>שלום!</h1>
                               <h3>נמצאה עבורך חניה</h3>
                           </div>
                           <div style='  position: relative;
                                         padding: 0.75rem 1.25rem;
                                         margin-bottom: 1rem;
                                         margin-left: 7%;
                                         margin-right: 7%;
                                         border: 1px solid #5c060e;
                                         border-radius: 0.25rem;
                                         color: #5c060e;
                                         width: 75%;
                                         background-color: #f5c9c9;
                                         border-radius: 5px; '>
                               <label>היקר, צר לנו להודיע לך שנאלצנו למחוק את פרטי חשבונך מהאפליקציה שלנו בגין דרוג הנמוך שלך ({1}){0}</label>
                               <br />
                               <label> בברכה, </label>
                               <br />
                               <label> PrkMark </label>
                               <br />
                               <label> for more information open Your ParkMark app </label>
                               <br />
                             
                               <br />
                           </div>",

                         user.Username, user_f.Rating);
                #endregion
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBodyString, null, MediaTypeNames.Text.Html);
                //alternateView.LinkedResources.Add(res);
                msg.AlternateViews.Add(alternateView);
                msg.IsBodyHtml = true;
                //msg.Attachments.Add(new Attachment(doctor.pictureDiploma));
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {


                throw ex;
            }
        }
    }
}
