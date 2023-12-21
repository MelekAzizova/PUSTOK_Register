using System.Net;
using System.Net.Mail;
using WebApplicationPustok.ExternalService.Interfaces;

namespace WebApplicationPustok.ExternalService.Impliments
{
	public class EmailService: IEmailService
	{
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		IConfiguration _configuration { get; }

		public void Send(string toMail, string header, string body, bool isHtml = true)
		{
			SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));

			smtpClient.EnableSsl = true;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);




			MailAddress from = new MailAddress(_configuration["Email:Username"], "Azizova");
			MailAddress to = new MailAddress(toMail);
			MailMessage message = new MailMessage(from, to);
			message.Body =body;
			message.Subject =header;
			message.IsBodyHtml = isHtml;
			smtpClient.Send(message);

		}
	}
}
