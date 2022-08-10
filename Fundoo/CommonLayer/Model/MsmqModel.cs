using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MsmqModel
    {
        MessageQueue msqQueue = new MessageQueue();

        public void MsmqSend(string token)
        {
            msqQueue.Path = @".\private$\Token"; //Windows MSMQ Path
            if(!MessageQueue.Exists(msqQueue.Path))
            {
                MessageQueue.Create(msqQueue.Path);
            }
            msqQueue.Formatter = new XmlMessageFormatter(new Type[] {typeof(string) });
            msqQueue.ReceiveCompleted += MsqQueue_ReceiveCompleted;
            msqQueue.Send(token);
            msqQueue.BeginReceive();
            msqQueue.Close();
        }

        private void MsqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = msqQueue.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            string subject = "Fundoo Notes Password Reset";
            string body = token;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("projectvs47@gmail.com", "asdjthfyrzqshuvv"),
                EnableSsl = true
            };
            smtpClient.Send("projectvs47@gmail.com", "projectvs47@gmail.com", subject, body);
            msqQueue.BeginReceive();
        }
    }
}
