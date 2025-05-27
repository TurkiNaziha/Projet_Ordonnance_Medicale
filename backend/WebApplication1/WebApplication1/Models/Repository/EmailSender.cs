using MimeKit;
////using System.Net.Mail;

//using MailKit.Security;
using System.Text;
using System.Collections.Generic;
using MailKit.Net.Smtp;


namespace WebApplication1.Models.Repository
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailConfiguration _emailConfig;
		public EmailSender(EmailConfiguration emailConfiguration) {
			_emailConfig = emailConfiguration;
		
		}
		public void SendEmail(Message message)
		{
			var emailMessage = CreateEmailMessage(message);
			Send(emailMessage);
		}

		private MimeMessage CreateEmailMessage(Message message)
		{
			var emailMessage = new MimeMessage();
			emailMessage.From.Add(new MailboxAddress("", _emailConfig.From));
			emailMessage.To.AddRange(message.To);
			emailMessage.Subject = message.Subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
			return emailMessage;

		}


		private void Send(MimeMessage mailMessage)
		{
			using (var client = new SmtpClient())
			{
				try
				{
					// Ensure _emailConfig.Port is an integer
					int port = int.Parse(_emailConfig.Port); // Convert to integer if it's a string

					// Use SecureSocketOptions (e.g., StartTls or SslOnConnect)
					client.Connect(_emailConfig.SmtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);

					// Optional: Remove unwanted authentication mechanisms
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					// Authenticate with the server using the username and password
					client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

					// Send the email
					client.Send(mailMessage);
				}
				catch (Exception ex)
				{
					// Handle exceptions here
					throw new Exception("Error sending email", ex);
				}
				finally
				{
					// Disconnect from the SMTP server
					client.Disconnect(true);

					// Dispose of the client
					client.Dispose();
				}
			}
		}




		public async Task SendEmailAsync(Message message)
		{
			var mailMessage= CreateEmailMessage(message);

			await SendAsync(mailMessage);
		}


		private async Task SendAsync(MimeMessage mailMessage)
		{
			using (var client = new SmtpClient())
			{
				try
				{
					foreach (var recipient in mailMessage.To)
					{
						Console.WriteLine($"Recipient: {recipient}");
					}

					// Ensure _emailConfig.Port is an integer
					int port = int.Parse(_emailConfig.Port); // Convert to integer if it's a string

					// Use SecureSocketOptions (e.g., StartTls or SslOnConnect)
					//await client.ConnectAsync(_emailConfig.SmtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
					await client.ConnectAsync(_emailConfig.SmtpServer, 587, MailKit.Security.SecureSocketOptions.StartTls);

					// Optional: Remove unwanted authentication mechanisms
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					// Authenticate with the server using the username and password
					await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

					// Send the email
					await client.SendAsync(mailMessage);
				}
				catch (Exception ex)
				{
					// Handle exceptions here
					throw new Exception("Error sending email", ex);
				}
				finally
				{
					// Disconnect from the SMTP server
					await client.DisconnectAsync(true);

					// Dispose of the client
					client.Dispose();
				}
			}

		}


	}
}
