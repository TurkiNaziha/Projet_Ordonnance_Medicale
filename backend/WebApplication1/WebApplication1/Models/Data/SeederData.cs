

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace WebApplication1.Models.Data
{


	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using TP7.Models;

	namespace TP7.Models.Data
	{
		public class SeederData
		{
			public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
			{
				var roles = new List<string> { "admin", "Medecin", "Patient", "Pharmacien" };

				foreach (var role in roles)
				{
					if (!await roleManager.RoleExistsAsync(role))
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}
			}
		}
	}



}
