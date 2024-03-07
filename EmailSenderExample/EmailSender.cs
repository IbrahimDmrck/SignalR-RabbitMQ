using MailKit.Net.Smtp;
using MimeKit;


namespace EmailSenderExample
{
    static class EmailSender
    {
        public static void Send(string to,string message)
        {
            MimeMessage mimeMessage = new();
            MailboxAddress mailboxAddressFrom = new("Admin", "sender mail addres");
            mimeMessage.From.Add(mailboxAddressFrom);
            MailboxAddress mailboxAddressTo = new("User", to);
            mimeMessage.To.Add(mailboxAddressTo);

            BodyBuilder bodyBuilder = new();
            bodyBuilder.TextBody = message;

            mimeMessage.Subject = "RabbitMQ & SignalR";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("your mail address", "your application password");
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
    }
}
