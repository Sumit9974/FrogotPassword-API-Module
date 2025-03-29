using MailKit.Net.Smtp;
using MimeKit; 

namespace ForgotPasswordModule.Services
{
    public class EmailService
    {
        private readonly IConfiguration conf;
        public string otp { get; set; } = String.Empty;
        public EmailService(IConfiguration conf)
        {
            this.conf = conf;
        }

        public async Task sendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Forgot", "code.breaker.geek@gmail.com"));

            email.To.Add(new MailboxAddress("", toEmail));

            email.Subject = subject;

            email.Body = new TextPart("plain") { Text = body};

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("code.breaker.geek@gmail.com", "fpsh tmbe yjlr vekn");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }

    }
}
