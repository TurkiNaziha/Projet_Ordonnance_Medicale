using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Models.Repository;

using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models.Data.TP7.Models.Data;
var builder = WebApplication.CreateBuilder(args);

var cnx = builder.Configuration.GetConnectionString("dbcon");
builder.Services.AddDbContext<DsContext>(
	options => options.UseSqlServer(cnx)
	);
// Add services to the container.

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>() ;
builder.Services.AddSingleton(emailConfig);

builder.Services.AddDbContext<DsContext>(
	options => options.UseSqlServer(cnx)
	);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IPharmacienRepository, PharmacienRepository>();
builder.Services.AddScoped<IDemandeRepository, DemandeRepository>();
builder.Services.AddScoped<IMedecinRepository,MedecinRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DsContext>().AddDefaultTokenProviders();
// Enable CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", builder =>
	{
		builder.WithOrigins("http://localhost:3000") // Allow only this frontend domain
			   .AllowCredentials() // Allow credentials (cookies, headers)
			   .AllowAnyHeader() // Allow any headers
			   .AllowAnyMethod(); // Allow any methods (GET, POST, etc.)
	});
});



//	.AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	o.TokenValidationParameters = new TokenValidationParameters
	{

		IssuerSigningKey = new SymmetricSecurityKey
		(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ClockSkew = TimeSpan.Zero
	};
});

var app = builder.Build();

// Seed roles after the app is built
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
	await SeederData.SeedRolesAsync(roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply the CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Use CORS


app.UseAuthentication();



app.MapControllers();

app.Run();
