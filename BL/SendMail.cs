using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SendMail
    {
        public string SenderName { get; set; }
        public string SenderEmailId { get; set; }
        public MailAddress FromAddress { get; set; }
        public SendMail(string senderName, string emailId)
        {
            SenderName = senderName;
            SenderEmailId = emailId;
            FromAddress = new MailAddress(SenderEmailId, SenderName);
        }
        //פונקציה השולחת מייל
        public bool SendEMail(MessageGmail message)
        {
            var success = false;
            var msg = createMessage(message);
            var client = createClient();
            try
            {
                client.Send(msg);
                success = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return success;
        }
        //פןקציה המיצרת את הודעת המייל
        private MailMessage createMessage(MessageGmail message)
        {
            MailMessage msg = new MailMessage()
            {
                From = FromAddress,
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };

            //מצרף את כתובות המייל לשליחה
            msg.To.Add(message.sendTo);
            return msg;
        }
        //התממשקות ל-gmail
        //ע"י הפרוטוקול Smtp 
        private SmtpClient createClient()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential
                 ("parkmarkapp2021@gmail.com", "parkmark0583249977");
            return client;
        }

    }
}
