namespace WebApplication1.Models.Repository
{
	public interface IEmailSender
	{
		void SendEmail(Message message);
		Task SendEmailAsync(Message message);
	}
}
