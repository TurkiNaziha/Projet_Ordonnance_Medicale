using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using MailKit.Net.Smtp;
using MimeKit;
using WebApplication1.Models.Repository;
using IEmailSender = WebApplication1.Models.Repository.IEmailSender;
using System.Net;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IConfiguration configuration;
		private readonly IEmailSender _emailSender;
		//private readonly IMapper  _mapper;

		// Constructor to inject services
		public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IConfiguration configuration )
		{
			this.userManager = userManager;
			this.configuration = configuration;
			this.roleManager = roleManager;
			this._emailSender = emailSender;
			//this._mapper = mapper;
		}

		//[HttpPost("Register")]
		//public async Task<IActionResult> CreateUser(Account registerUser)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	var existingUser = await userManager.FindByNameAsync(registerUser.Username);
		//	if (existingUser != null)
		//	{
		//		return BadRequest("Utilisateur existant");
		//	}
		//	//if (!await userManager.IsEmailConfirmedAsync(existingUser))
		//	//	return Unauthorized();

		//	// Create a new ApplicationUser
		//	var applicationUser = new ApplicationUser()
		//	{
		//		UserName = registerUser.Username,
		//		Email = registerUser.Email
		//	};

		//	var result = await userManager.CreateAsync(applicationUser, registerUser.Password);
		//	if (result.Succeeded)
		//	{
		//		// Check if the role exists, if not, create it
		//		var roleExist = await roleManager.RoleExistsAsync(registerUser.Role);
		//		if (!roleExist)
		//		{
		//			// Create the role if it doesn't exist
		//			var roleResult = await roleManager.CreateAsync(new IdentityRole(registerUser.Role));
		//			if (!roleResult.Succeeded)
		//			{
		//				return BadRequest("Role creation failed");
		//			}
		//		}

		//		// Assign the role to the user
		//		await userManager.AddToRoleAsync(applicationUser, registerUser.Role);

		//		// Return the created user
		//		return CreatedAtAction(nameof(CreateUser), new { id = applicationUser.Id }, applicationUser);
		//	}

		//	return BadRequest(result.Errors);
		//}




		[HttpPost("Register")]
		public async Task<IActionResult> CreateUser(Account registerUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Predefined list of allowed roles
			var validRoles = new List<string> { "Admin", "Medecin", "Patient", "Pharmacien" };
			if (!validRoles.Contains(registerUser.Role))
			{
				return BadRequest("Invalid role specified. Allowed roles are: Admin, Medecin, Patient, Pharmacien.");
			}

			// Check if the email is already registered
			var emailExists = await userManager.FindByEmailAsync(registerUser.Email);
			if (emailExists != null)
			{
				return BadRequest("Email already exists.");
			}

			// Check if the username already exists
			var existingUser = await userManager.FindByNameAsync(registerUser.Username);
			if (existingUser != null)
			{
				return BadRequest("Username already exists.");
			}

			// Create a new ApplicationUser
			var applicationUser = new ApplicationUser()
			{
				UserName = registerUser.Username,
				Email = registerUser.Email,
				DefaultRole = registerUser.Role,
				Avatar=registerUser.Avatar,
			};

			// Create the user with the provided password
			var result = await userManager.CreateAsync(applicationUser, registerUser.Password);
			if (result.Succeeded)
			{
				// Check if the role exists, if not, create it
				var roleExists = await roleManager.RoleExistsAsync(registerUser.Role);
				if (!roleExists)
				{
					// Create the role
					var roleResult = await roleManager.CreateAsync(new IdentityRole(registerUser.Role));
					if (!roleResult.Succeeded)
					{
						return BadRequest("Role creation failed.");
					}
				}

				// Assign the role to the user
				var roleAssignResult = await userManager.AddToRoleAsync(applicationUser, registerUser.Role);
				if (!roleAssignResult.Succeeded)
				{
					return BadRequest("Failed to assign role to the user.");
				}

				// Map to ApplicationUser (if not already mapped)
				//var userr = _mapper.Map<ApplicationUser>(registerUser);

				// Generate email confirmation token
				//var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);


				// Generate email confirmation token
				var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

				// Encode the token for safe URL usage
				var encodedToken = WebUtility.UrlEncode(token);

				// Create the confirmation URL with the token as a query parameter
				var param = new Dictionary<string, string?>
		{
			{"token", token },
			//{"email", applicationUser.Email }
			{"email", registerUser.Email }
		};

				// Construct the callback URL (ensure Clienturi is not null)
				var callback = QueryHelpers.AddQueryString(registerUser.Clienturi!, param);

				// Create the email message
				//var message = new Message(new[] { applicationUser.Email }, "Email confirmation Token", callback);
				//// Log the email details
				//Console.WriteLine($"Email being sent to: {applicationUser.Email}");
				//Console.WriteLine($"Email Subject: {message.Subject}");
				//Console.WriteLine($"Email Body: {message.Content}");
				// Send the email

				// Create the email message
				var message = new Message(
					new[] { applicationUser.Email },
					"Account Activation - Email Confirmation",
					$@"Hello {applicationUser.UserName},

							Your account has been successfully activated. Please use the following details to log in:

							Username: {applicationUser.UserName}
							Password: {registerUser.Password}

							To confirm your email address, please click the link below:

							{callback}

							Thank you for using our service!");

				// Log the email details
				Console.WriteLine($"Email being sent to: {applicationUser.Email}");
				Console.WriteLine($"Email Subject: {message.Subject}");
				Console.WriteLine($"Email Body: {message.Content}");

				await _emailSender.SendEmailAsync(message);

				// Return the created user
				return CreatedAtAction(nameof(CreateUser), new { id = applicationUser.Id }, applicationUser);
			}

			// Handle errors during user creation
			return BadRequest(result.Errors);
		}



		[HttpPut("Modify/{id}")]
		public async Task<IActionResult> ModifyUser(string id, Account updatedAccount)
		{
			if (!ModelState.IsValid)
			{
				foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				{
					Console.WriteLine(error.ErrorMessage);
				}
				return BadRequest(ModelState);
			}

			var user = await userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			user.UserName = updatedAccount.Username;
			user.Email = updatedAccount.Email;

			var validRoles = new List<string> { "Admin", "Medecin", "Patient", "Pharmacien" };
			if (!validRoles.Contains(updatedAccount.Role))
			{
				return BadRequest("Invalid role specified.");
			}

			var existingRole = await userManager.GetRolesAsync(user);
			if (existingRole.Count > 0)
			{
				await userManager.RemoveFromRolesAsync(user, existingRole);
			}

			await userManager.AddToRoleAsync(user, updatedAccount.Role);

			var result = await userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return Ok(user);
			}
			return BadRequest(result.Errors);
		}

		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			var result = await userManager.DeleteAsync(user);
			if (result.Succeeded)
			{
				return Ok("User deleted successfully.");
			}

			return BadRequest(result.Errors);
		}

		[HttpGet("Get/{id}")]
		public async Task<IActionResult> GetUser(string id)
		{
			var user = await userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			var roles = await userManager.GetRolesAsync(user);
			return Ok(new
				  {
					  user.Id,
					  user.UserName,
					  user.Email,
					  Roles = roles
				  });
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = userManager.Users.ToList(); // Fetch all users
			var userDetails = new List<object>();

			foreach (var user in users)
			{
				var roles = await userManager.GetRolesAsync(user); // Get roles for the user

				// Add the user's details, including roles and avatar
				userDetails.Add(new
				{
					user.Id,
					user.UserName,
					user.Email,
					Avatar = (user as ApplicationUser)?.Avatar, // Cast to ApplicationUser to access Avatar
					Roles = roles // List of roles for the user
				});
			}

			return Ok(userDetails);
		}

		[HttpGet("GetByUserName/{userName}")]
		public async Task<IActionResult> GetUserByUserName(string userName)
		{
			var user = await userManager.FindByNameAsync(userName);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			var roles = await userManager.GetRolesAsync(user);
			return Ok(new
			{
				user.Id,
				user.UserName,
				user.Email,
				Avatar = (user as ApplicationUser)?.Avatar, // Cast to ApplicationUser to access Avatar
				Roles = roles // List of roles for the user
			});
		}


		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDTO login)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Find user by email
			var user = await userManager.FindByEmailAsync(login.Email);
			if (user == null)
			{
				return BadRequest("Invalid email or password.");
			}

			// Verify password
			if (!await userManager.CheckPasswordAsync(user, login.Password))
			{
				return BadRequest("Invalid email or password.");
			}

			// Get user roles
			//var roles = await userManager.GetRolesAsync(user);
			//if (!roles.Contains(login.Role))
			//{
			//	return BadRequest("Unauthorized role.");
			//}

			// Generate claims
			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.NameIdentifier, user.Id),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			//new Claim(ClaimTypes.Role, login.Role) // Include the role claim
		   };

			// Generate signing credentials
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			// Create token
			var token = new JwtSecurityToken(
				issuer: configuration["JWT:Issuer"],
				audience: configuration["JWT:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: credentials
			);

			// Generate response
			var tokenResponse = new
			{
				token = new JwtSecurityTokenHandler().WriteToken(token),
				expiration = token.ValidTo,
				username = user.UserName,
				//role = login.Role
			};

			return Ok(tokenResponse);
		}

		[HttpPost("emailconfirmation")]
		public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
		{
			var decodedToken = WebUtility.UrlDecode(token);
			var user = await userManager.FindByEmailAsync(email);
			if (user is null)
			{
				return BadRequest("Invalid Email");
			}

			Console.WriteLine($"Email: {email}");
			Console.WriteLine($"Token: {token}");
			Console.WriteLine($"Email Confirmed: {user.EmailConfirmed}");

			//var confirmResult = await userManager.ConfirmEmailAsync(user, token);
			var confirmResult = await userManager.ConfirmEmailAsync(user, decodedToken);
			if (!confirmResult.Succeeded)
			{
				Console.WriteLine("Errors during confirmation:");
				foreach (var error in confirmResult.Errors)
				{
					Console.WriteLine($"Code: {error.Code}, Description: {error.Description}");
				}
				return BadRequest("Invalid Email confirmation Request");
			}

			return Ok();
		}



		//[HttpPost("Login")]
		//public async Task<IActionResult> Login(LoginDTO login)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}
		//	Console.WriteLine(login.Email);

		//	// Fetch users with the given email
		//	var users = await userManager.Users
		//		.Where(u => u.NormalizedEmail == login.Email.ToUpper())
		//		.ToListAsync();

		//	if (users.Count == 0)
		//	{
		//		return BadRequest("Wrong Credentials");
		//	}
		//	//if (users.Count > 1)
		//	//{
		//	//	// Return an error if multiple users are found
		//	//	return StatusCode(500, "Duplicate email entries found. Contact support.");
		//	//}

		//	var user = users.Single();
		//	//if (!await userManager.IsEmailConfirmedAsync(user))
		//	//	return BadRequest("Account not acctivate yet");
		//	if (await userManager.CheckPasswordAsync(user, login.Password))
		//	{
		//		var claims = new List<Claim>
		//{
		//	new Claim(ClaimTypes.Name, user.UserName),
		//	new Claim(ClaimTypes.NameIdentifier, user.Id),
		//	new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		//};

		//		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
		//		var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		//		var token = new JwtSecurityToken(
		//			claims: claims,
		//			issuer: configuration["JWT:Issuer"],
		//			audience: configuration["JWT:Audience"],
		//			expires: DateTime.Now.AddHours(1),
		//			signingCredentials: sc
		//		);

		//		var _token = new
		//		{
		//			token = new JwtSecurityTokenHandler().WriteToken(token),
		//			expiration = token.ValidTo,
		//			username = login.Email,
		//		};
		//		return Ok(_token);
		//	}

		//	return BadRequest("Wrong Credentials");
		//}




	}




}

