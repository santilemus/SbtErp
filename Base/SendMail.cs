using System;
using System.Linq;
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
            MailMessage mailMsg = new MailMessage(mailFrom, mailTo, subject, msg);
            SmtpClient mailClient = new SmtpClient(MailHost, MailPort);
            mailClient.Send(mailMsg);
        }

        public void Send(string mailFrom, string mailTo, string subject, string msg, string fileName)
        {
            MailMessage mailMsg = new MailMessage(mailFrom, mailTo, subject, msg);
            Attachment adjunto = new Attachment(fileName);
            SmtpClient mailClient = new SmtpClient(MailHost, MailPort);
            mailClient.Send(mailMsg);
        }

        public string Smtp { get; set; }
        public int Port { get; set; }
    }
}
