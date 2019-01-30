using System.Collections.Generic;
using AutoMapper;
using CoreCodeCamp.Controllers;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodeCamp
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<CampContext>();
      services.AddScoped<ICampRepository, CampRepository>();
      services.AddAutoMapper();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                //options.ApiVersionReader=ApiVersionReader.Combine(
                    
                //    new HeaderApiVersionReader("X-Version"),
                //    new QueryStringApiVersionReader("ver", "version")
                //    );

                options.ApiVersionReader=new UrlSegmentApiVersionReader();


                //options.Conventions.Controller<TalksController>().HasApiVersion(new ApiVersion(1, 0));
                //options.Conventions.Controller<TalksController>().HasApiVersion(new ApiVersion(1, 0))
                //    .Action(c=>c.Delete(default(string),default(int))).MapToApiVersions(new List<ApiVersion>
                //    {
                //        new ApiVersion(1,1)
                //    });

            });

            services.AddMvc(options => options.EnableEndpointRouting = false)
            //services.AddMvc()
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      
      app.UseMvc();
    }
  }
}
