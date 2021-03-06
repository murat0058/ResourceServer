﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Owin.Builder;
using Owin;
using Thinktecture.IdentityServer.AccessTokenValidation;

namespace ResourceServer
{
    using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
    using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
        }

        public void Configure(IApplicationBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            //app.Map("/action", application =>
            //{
            //    application.Use(async (context, next) =>
            //    {
            //        await context.Response.WriteAsync("Resource");
            //    });
            //});

            //app.UseOwin(pipe =>
            //{
            //    pipe(next =>
            //    {
            //        var builder = new AppBuilder();
            //        var provider = app.ApplicationServices.GetService<IDataProtectionProvider>();

            //        builder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
            //        {
            //            var dataProtection = provider.CreateProtector(string.Join(",", purposes));
            //            return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
            //        });

            //        builder.UseIdentityServerBearerTokenAuthentication(
            //            new IdentityServerBearerTokenAuthenticationOptions
            //            {
            //                Authority = "http://localhost:5000/core",
            //                ValidationMode = ValidationMode.ValidationEndpoint
            //            });

            //        return builder.Build<AppFunc>();
            //    });
            //});

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            var options = new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:5000/core",
                ValidationMode = ValidationMode.ValidationEndpoint
            };

            app.UseAppBuilder(builder => { builder.UseIdentityServerBearerTokenAuthentication(options); }, "ResourceServer");

            app.Map("/action", application =>
            {
                application.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Resource");
                });
            });

        }
    }
}
