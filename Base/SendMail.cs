using System.Net.Mail;

namespace SBT.Apps.Base.Module
{
    public class SendMail
    {
        protected string MailHost { get; set; }
        protected int MailPort { get; set; }


        public SendMail()
        {

        }

        public SendMail(string mailHost, int mailPort) : base()
        {
            MailHost = mailHost;
            MailPort = mailPort;
        }

        public void Send(string mailFrom, string mailTo, string subject, string msg)
        {
            using (SmtpClient mailClient = new SmtpClient(MailHost, MailPort))
            {
                using (MailMessage mailMsg = new MailMessage(mailFrom, mailTo, subject, msg))
                {
                    mailClient.Send(mailMsg);
                }
            }
        }

        public void Send(string mailFrom, string mailTo, string subject, string msg, string fileName)
        {
            using (SmtpClient mailClient = new SmtpClient(MailHost, MailPort))
            {
                using (MailMessage mailMsg = new MailMessage(mailFrom, mailTo, subject, msg))
                {
                    mailMsg.Attachments.Add(new Attachment(fileName));
                    mailClient.Send(mailMsg);
                }
            }
        }

        public string Smtp { get; set; }
        public int Port { get; set; }
    }
}
