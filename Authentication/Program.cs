using Authentication.Common;
using Authentication.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var appSettings = appSettingsSection.Get<AppSettings>();

builder.Services.Configure<AppSettings>(options =>
{
    appSettingsSection.Bind(options);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JWT";
    options.DefaultChallengeScheme = "JWT";
})
   .AddJwtBearer((options) =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateIssuerSigningKey = true,
           ValidateLifetime = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey))
       };
   });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsOptions =>
    {
        corsOptions.WithOrigins(appSettings.AllowedOrigins);
        corsOptions.AllowAnyHeader();
        corsOptions.AllowAnyMethod();
        corsOptions.AllowCredentials();
    });
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(ErrorHandler));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthentication();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
