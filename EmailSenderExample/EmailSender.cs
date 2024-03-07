using MailKit.Net.Smtp;
using MimeKit;


namespace EmailSenderExample
{
    static class EmailSender
    {
        public static void Send(string to,string message)
        {
            MimeMessage mimeMessage = new();
            MailboxAddress mailboxAddressFrom = new("Admin", "ibrahimdemircik1@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);
            MailboxAddress mailboxAddressTo = new("User", to);
            mimeMessage.To.Add(mailboxAddressTo);

            BodyBuilder bodyBuilder = new();
            bodyBuilder.TextBody = message;

            mimeMessage.Subject = "RabbitMQ & SignalR";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("ibrahimdemircik1@gmail.com", "izeanupantmqopdf");
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
    }
}
