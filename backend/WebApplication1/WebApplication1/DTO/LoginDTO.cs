using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
	public class LoginDTO
	{

		[Required]
		[EmailAddress]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		[StringLength(256)]
		public string Password { get; set; }

		//[Required]
		//[StringLength(50)]
		//public string Role { get; set; }
	}
}
