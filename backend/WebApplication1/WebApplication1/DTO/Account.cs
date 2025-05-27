using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
	public class Account
	{
		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		[StringLength(256)]
		public string Password { get; set; }

		[Required]
		[RoleValidation]

		public string Role { get; set; }

		[StringLength(500)]
		public string Avatar { get; set; }

		[Required]
		public bool IsActive { get; set; }

		public string? Clienturi { get; set; }
	}

	// Custom validation attribute for role
	public class RoleValidationAttribute : ValidationAttribute
	{
		private static readonly string[] AllowedRoles = { "Admin", "Medecin", "Patient", "Pharmacien" };

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is string role && AllowedRoles.Contains(role))
			{
				return ValidationResult.Success;
			}

			return new ValidationResult($"The role must be one of the following: {string.Join(", ", AllowedRoles)}");
		}
	}
}

