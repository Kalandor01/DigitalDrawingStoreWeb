using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using XperiCad.DigitalDrawingStore.Web.API.Authorization;
using XperiCad.DigitalDrawingStore.Web.API.Core;

namespace DigitalDrawingStore.Web.UI
{
    public static class Program
    {
        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
            builder.Services.AddAuthorization(OnAuthorization);
            builder.Services.AddTransient<ISecurityFacade, SecurityFacade>();
            builder.Services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();


            // TODO: implement DI for APIModule
            //var apiModule = new APIModule();
            //var apiModuleServices = apiModule.GetServices(typeof(Program).Assembly);
            //var apiModuleAssembly = typeof(XperiCad.DigitalDrawingStore.Web.Api.Program).Assembly;
            //builder.Services.AddControllersWithViews()
            //    .PartManager.ApplicationParts.Add(new AssemblyPart(apiModuleAssembly));

            //foreach (var service in apiModuleServices)
            //{
            //    builder.Services.Add(service);
            //}

            var ddsWebApiModule = new DdsWebApiModule();
            ddsWebApiModule.InitializeModule();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void OnAuthorization(AuthorizationOptions options)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var admin = config.GetValue<string>("UserGroups:Admin");
            var user = config.GetValue<string>("UserGroups:User");
            options.AddPolicy(Constants.Autorization.Policies.ADMIN, policy => policy.RequireRole(admin));
            options.AddPolicy(Constants.Autorization.Policies.USER, policy => policy.RequireRole(user, admin));
            options.FallbackPolicy = options.DefaultPolicy;
        }
    }
}