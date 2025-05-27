using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class ApplicationUser: IdentityUser
	{
		[Required]
		[EmailAddress]
		[MaxLength(100)]
		public override string Email { get; set; }

		public string DefaultRole { get; set; }
		public string Avatar { get; set; }
	}
}
