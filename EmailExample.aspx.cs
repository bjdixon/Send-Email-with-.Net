using System;
using System.Text;
using System.Web.UI;
using System.Net.Mail;
using System.Configuration;

public partial class EmailExample : System.Web.UI.Page {
        /*
        You'll need to make sure the following is configured in the web.config file:
        <system.net>
                <mailSettings>
                        <smtp from="name@email.com">
                        <network host="localhost" password="" userName="" />
                        </smtp>
                </mailSettings>
        </system.net>
        */

        protected void Page_Load(object sender, EventArgs e) {
                SendMail("address@domain.com", "subject 1", "Here is a message that I'd like to send");
                SendMail("sender@domain.com", "cc@domain.com", "subject 2", "Here is another message that I'd like to send"); 
        }

        private void SendMail(string to, string cc, string subject, string content) {
                //create message
                MailMessage message = CreateMessage(to, subject, content);
                //add CC
                try {
                    message.CC.Add(new MailAddress(cc));
                } catch (FormatException) {
                    // Die here
                }
                //send the message
                SmtpClient mailClient = new SmtpClient();
                mailClient.Send(message);
        }

        private void SendMail(string to, string subject, string content) {
                //create message
                MailMessage message = CreateMessage(to, subject, content);
                //send the message
                SmtpClient mailClient = new SmtpClient();
                mailClient.Send(message);
        }

        private MailMessage CreateMessage(string to, string subject, string content) {
                //create the mail object
                MailMessage message1 = new MailMessage();

                try {
                    //recipient 
                    message1.To.Add(new MailAddress(to));
                } catch (FormatException) {
                    // Die here
                }

                //balance of Mailer properties
                message1.From = new MailAddress(ConfigurationManager.AppSettings["emailAddressInWebConfig"]);
                //emailAddressInWebConfig line looks like this: <add key="emailAddressInWebConfig" value="name@email.com" /> and belongs in the <appSettings> section.
                message1.Subject = subject;
                message1.IsBodyHtml = true;

                //build the email body
                StringBuilder myStringBuilder = new StringBuilder("<STYLE type=text/css>");
                myStringBuilder.Append("<!--");
                myStringBuilder.Append("p {font-size: 12px;}");
                myStringBuilder.Append("-->");
                myStringBuilder.Append("</style>");
                myStringBuilder.Append(content);
                                
                message1.Body = myStringBuilder.ToString();

                return message1;
                
        }
}