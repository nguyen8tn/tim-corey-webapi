using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiSecurity.Constants;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    opts =>
    {
        var title = "Title";
        var description = "Description";
        var term = new Uri("https://localhost:7272/term");

        var license = new OpenApiLicense()
        {
            Name = "This is my full license information or a link to it",
        };

        var contact = new OpenApiContact() {
            Name = "Tim Corey",
            Email = "help@gmail.com",
            Url = new Uri("https://localhost:7272/term"),
        };

        opts.SwaggerDoc("v1", new OpenApiInfo()
        {
            Version = "v1",
            Title = $"{title} v1",
            Description = description,
            TermsOfService= term,
            License = license,
            Contact = contact
        });

        opts.SwaggerDoc("v2", new OpenApiInfo()
        {
            Version = "v2",
            Title = $"{title} v2",
            Description = description,
            TermsOfService = term,
            License = license,
            Contact = contact
        });

    });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(PolicyConstants.MustHaveEmployeeId, policy =>
    {
        policy.RequireClaim("employeeId");
    });

    opts.AddPolicy(PolicyConstants.MustBeTheOwner, policy =>
    {
        policy.RequireClaim("title", "Business Owner");
    });

    opts.AddPolicy(PolicyConstants.MustBeAVeteranEmployee, policy =>
    {
        policy.RequireClaim("employeeId", "E001", "E002", "E003");
    });

    opts.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                builder.Configuration.GetValue<string>("Authentication:SecretKey")))
        };
    });

builder.Services.AddApiVersioning(opts =>
{
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.DefaultApiVersion = new(1, 0);
    opts.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
