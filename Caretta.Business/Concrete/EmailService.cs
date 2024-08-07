using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Caretta.Core.Dto.EmailDto;
using Microsoft.Extensions.Configuration;

using Caretta.Business.Abstract;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Caretta.Business.Concrete
{
    public class EmailService : IEmailInterface
    {
        private readonly IConfiguration _config;
        private readonly CarettaContext _context;
        public EmailService(IConfiguration config, CarettaContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task SendDailyEmailsAsync()
        {
            var users = await _context.Users.Where(u => u.IsDeleted == false).ToListAsync();
            //foreach (var user in users)
            //{
                var emailDto = new EmailDto
                {
                    To = "semirtatli@outlook.com",
                    Subject = $"Daily Update at {DateTime.Now} for", //{user.Name}",
                    Body = "Here is your daily update..."
                };
                 await SendEmailAsync(emailDto);
            //}
        }

        public async Task SendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
