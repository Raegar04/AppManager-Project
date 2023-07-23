using System.Net;
using System.Net.Mail;

namespace MVC_Identity.Helpers
{
    public class SendMail
    {
        public static void Send(string emailAdress, string body, string subject)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("beliykharkov.n@gmail.com");
            mailMessage.To.Add(emailAdress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("beliykharkov.n@gmail.com", "ttgbjidbvrtaxwdh");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
