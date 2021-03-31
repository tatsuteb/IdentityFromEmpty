using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace AppWithIdentity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // �f���p�ɓK���ȃ��[�U�[�X�g�A��p�ӂ���
            var store = new Dictionary<string, string>();
            services.AddSingleton(store);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // ���O�C����ʓ��̃p�X��ς������Ƃ��͂����Őݒ肷��

                    // options.AccessDeniedPath = "/AccessDenied";
                    // options.LoginPath = "/Login";
                    // options.LogoutPath = "/Logout";
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapControllerRoute(
                //     name: "default",
                //     pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
