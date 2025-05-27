using MimeKit;

namespace WebApplication1.Models
{
	public class Message
	{
		public List<MailboxAddress> To { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }

		public Message(IEnumerable<string> to, string subject, string content)
		{
			To = new List<MailboxAddress>();
			foreach (var email in to)
			{
				To.Add(MailboxAddress.Parse(email)); // Ensure valid email addresses are added
			}
			Subject = subject;
			Content = content;
		}

	}


}
