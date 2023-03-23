using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using XperiCad.DigitalDrawingStore.Web.API;

namespace XperiCad.DigitalDrawingStore.Web.Api
{
    // TODO: implement DI for API module
    //public class APIModule
    //{
    //    public IServiceCollection GetServices(Assembly startupAssembly)
    //    {
    //        var services = new ServiceCollection();

    //        services
    //            .AddControllers()
    //            .AddApplicationPart(startupAssembly)
    //            .AddControllersAsServices();

    //        return services;
    //    }
    //}

    public static class Program
    {
        static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
               .AddNegotiate();

            builder.Services.AddAuthorization(OnAuthorization);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void OnAuthorization(AuthorizationOptions options)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var admin = config.GetValue<string>("UserGroups:Admin");
            var user = config.GetValue<string>("UserGroups:User");
            options.AddPolicy(Constants.Authorization.Policies.ADMIN, policy => policy.RequireRole(admin));
            options.AddPolicy(Constants.Authorization.Policies.USER, policy => policy.RequireRole(user, admin));
            options.FallbackPolicy = options.DefaultPolicy;
        }
    }
}
