using API_Demo.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using API_Demo.Configuration;
using API_Demo.Data;
using API_Demo.Filter;
using API_Demo.Model;
using API_Demo.Repository;
using API_Demo.Services;
using API_Demo.Settings;
using API_Demo.Logger;
using API_Demo.Hubs;
namespace API_Demo;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Serilog
        // for serilog
        // configure Serilog

        //Log.Logger = new LoggerConfiguration()
        //    .MinimumLevel.Information()
        //    //.WriteTo.Console()
        //    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day,
        //        fileSizeLimitBytes: 10 * 1024 * 1024,
        //        rollOnFileSizeLimit: true,
        //        retainedFileCountLimit: 7, // ÌÕ ›Ÿ »¬Œ— 7 „·›«  ›ﬁÿ
        //        shared: true // ·Ê ›Ì √ﬂÀ— „‰ process »Ìﬂ »Ê« ··ÊÃ
        //    )
        //    .CreateLogger();

        //builder.Host.UseSerilog(); // œ„Ã Serilog „⁄ ASP.NET Core
        #endregion

        #region  Configuration
        //// for inject ConnectionStringsOptions itself
        //first way 
        //var connStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringsOptions>();
        //builder.Services.AddSingleton(connStrings);
        //second way 
        //var connectionStringsOptions = new ConnectionStringsOptions();
        //builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStringsOptions);
        //builder.Services.AddSingleton(connectionStringsOptions);
        // for IOptions<ConnectionStringsOptions> && IOptionsSnapshot && IOptionsMonitor

        builder.Services.Configure<ConnectionStringSettings>(builder.Configuration.GetSection("ConnectionStrings"));
        builder.Services.Configure<ExternalLoginSettings>(builder.Configuration.GetSection("ExternalLogin"));
        builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
        builder.Services.Configure<UploadsSettings>(builder.Configuration.GetSection("Uploads"));
        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSetting"));
        builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
        #endregion

        #region CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyPolice", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        #endregion

        #region Authentication (JWT + External Login)
        var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTSettings>();
        var externalLoginOptions = builder.Configuration.GetSection("ExternalLogin").Get<ExternalLoginSettings>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = "Cookies";

        })
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = externalLoginOptions?.Google?.ClientId ?? "";
            options.ClientSecret = externalLoginOptions?.Google?.ClientSecret ?? "";
            options.CallbackPath = "/signin-google";

        })
        .AddGitHub(options =>
        {
            options.ClientId = externalLoginOptions?.Github?.ClientId ?? "";
            options.ClientSecret = externalLoginOptions?.Github?.ClientSecret ?? "";
            options.Scope.Add("user:email");
            options.CallbackPath = "/signin-github";
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtOptions?.Issuer,
                ValidAudience = jwtOptions?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Key ?? "")),
                ClockSkew = TimeSpan.Zero,
            };
            //options.Events = new JwtBearerEvents
            //{
            //    OnChallenge = context =>
            //    {
            //        context.HandleResponse();
            //        context.Response.StatusCode = 401;
            //        context.Response.ContentType = "application/json";
            //        return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
            //    }
            //};
        })
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        //builder.Services.AddAuthorization();
        #endregion

        #region Add Context & Service

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>(); 

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
         .AddRoles<IdentityRole>()
         .AddEntityFrameworkStores<AppDbContext>();

        /*  //this for mvc it cookie base
        //builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        //{
        //    options.Password.RequireDigit = false;
        //    options.Password.RequiredLength = 6;
        //    options.Password.RequireLowercase = false;
        //    options.Password.RequireNonAlphanumeric = false;
        //    options.Password.RequireUppercase = false;
        //})
        //.AddEntityFrameworkStores<AppDbContext>();*/

        builder.Services.AddSingleton<ILogging, Logging>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<PermissionBeasdOnAuthorization>();
        builder.Services.AddTransient<IMailService, MailService>();
        builder.Services.AddTransient<ISMSService, SMSService>();
        builder.Services.AddHttpContextAccessor();
        #endregion

        #region Auto Mapper
        //this for .AutoMapper.Extensions.Microsoft.DependencyInjection
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        //// this for auto mapper but it work to version 12
        //var mapperConfig = new MapperConfiguration(config =>
        //{
        //    config.AddProfile(new MappingProfile());
        //});

        //IMapper mapper = mapperConfig.CreateMapper();
        //builder.Services.AddSingleton(mapper);
        #endregion

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer eyJhb...\""
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        builder.Services.AddControllers();

        //builder.Services.AddControllers(options =>
        //{
        //    //add global  filter
        //     options.Filters.Add<PermissionBeasdOnAuthorization>();
        //});


        //  ”ÃÌ· Œœ„«  SignalR
        builder.Services.AddSignalR();


        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        #region Seed Roles
        // Seed Roles using async
        //using (var scope = app.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    await SeedRolesAsync(services);
        //}
        #endregion

        //app.UseMiddleware<RateLimitPerIpMiddleware>();
        app.MapHealthChecks("/health");


        app.UseMiddleware<RequestTimingMiddleware>();
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseCors("MyPolice");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        //  ”ÃÌ· SignalR
        app.MapHub<ChatHub>("/chathub");

        app.MapGet("/", () => "SignalR Example Running");
        app.Run();
    }
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "User" };

        foreach (var role in roleNames)
        {
            var exists = await roleManager.RoleExistsAsync(role);
            if (!exists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}

