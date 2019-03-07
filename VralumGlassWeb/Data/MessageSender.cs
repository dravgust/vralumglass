using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;

namespace VralumGlassWeb.Data
{
	public class EmailSettings
	{
		public string PrimaryDomain { get; set; }

		public int PrimaryPort { get; set; }

		public string SecondaryDomain { get; set; }

		public int SecondaryPort { get; set; }

		public string UsernameEmail { get; set; }

		public string UsernamePassword { get; set; }

		public string FromEmail { get; set; }

		public string ToEmail { get; set; }

		public string CcEmail { get; set; }
	}

	public class MessageSender : IEmailSender
	{
		private EmailSettings EmailSettings { get; }

		public MessageSender(IOptions<EmailSettings> emailSettings)
		{
			EmailSettings = emailSettings.Value;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Execute(email, subject, htmlMessage);
		}

		private async Task Execute(string email, string subject, string message)
		{
			try
			{
				var toEmail = string.IsNullOrEmpty(email) ? EmailSettings.ToEmail : email;

				using (var mailMessage = new MailMessage())
				{
					mailMessage.From = new MailAddress(EmailSettings.FromEmail, "dr@vgust");
					mailMessage.To.Add(toEmail);
					mailMessage.Body = message;
					mailMessage.IsBodyHtml = true;
					mailMessage.Subject = subject;

					//mailMessage.Attachments.Add(new Attachment());

					using (var client = new SmtpClient())
					{
						client.EnableSsl = true;
						client.UseDefaultCredentials = false;
						client.Credentials = new NetworkCredential(EmailSettings.UsernameEmail, EmailSettings.UsernamePassword);
						client.Host = EmailSettings.PrimaryDomain;
						client.Port = EmailSettings.PrimaryPort;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;

						await client.SendMailAsync(mailMessage);
					}
				}

			}
			catch (Exception e)
			{
				Trace.WriteLine(e.Message);
			}
		}
	}
}
