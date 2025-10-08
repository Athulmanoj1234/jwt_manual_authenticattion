using Azure.Core;
using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models.Enities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//registering jwt service
//This registers the authentication service in the dependency injection (DI) container.
//It tells ASP.NET Core that the app will use authentication and specifies which type.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //    Defines the default method the app uses to authenticate users.
    //? Here, it’s set to JwtBearerDefaults.AuthenticationScheme, meaning JWT Bearer tokens will be used.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //DefaultChallengeScheme: Defines what happens when an unauthorized request is made(e.g., 401 Unauthorized).
    //? Also set to JWT Bearer, meaning the system will challenge using JWT.
}).AddJwtBearer(options =>
//This adds the JWT Bearer token handler — the logic or method that reads and validates the JWT tokens sent by clients
{
    //This object contains all the rules for validating an incoming JWT.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        //This validation prevents tokens from untrusted sources from being accepted.
        //ie the backend who issued tokens only should be given so if the token generates in this backend url and frontend gets the token and they sent to another backend who also accepts the token will be considered as unauthorised
        ValidateAudience = true,
        // ie the frontend url which is given   Token is only accepted if aud matches this value.Think of audience as “who the token is for.”
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //What it does: Validates the token’s signature using the key you configured.Why it matters: Ensures the token wasn’t tampered with and was signed by your trusted backend.
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
