using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.WebTools
{
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="content">邮件内容</param>
        /// <param name="title">邮件标题</param>
        /// <param name="subject">邮件摘要</param>
        /// <returns></returns>
        public bool SendMail(string email, string content, string fromTitle, string title, string subject)
        {
            bool result = true;
            try {
                //From Address    
                string FromAddress = "account_service_01@163.com";
                string FromAdressTitle = fromTitle;
                //To Address    
                string ToAddress = email;
                string ToAdressTitle = title;
                string Subject = subject;

                string BodyContent = content;

                //Smtp Server    
                string SmtpServer = "smtp.163.com";
                //Smtp Port Number    
                int SmtpPortNumber = 25;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress
                                        (FromAdressTitle,
                                         FromAddress
                                         ));
                mimeMessage.To.Add(new MailboxAddress
                                         (ToAdressTitle,
                                         ToAddress
                                         ));
                mimeMessage.Subject = Subject; //Subject  
                mimeMessage.Body = new TextPart("plain") {
                    Text = BodyContent
                };

                using (var client = new SmtpClient()) {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    client.Authenticate(
                        "account_service_01@163.com",
                        "qingcloud001"
                        );
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex) {
                result = false;
                throw ex;
            }

            return result;
        }
    }
}
