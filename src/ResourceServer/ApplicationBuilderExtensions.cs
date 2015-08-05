using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.Owin.Builder;
using Owin;

namespace ResourceServer
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> configure, string owinHostAppName)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var builder = new AppBuilder();
                    builder.Properties["builder.DefaultApp"] = next;
                    builder.Properties["host.AppName"] = owinHostAppName;
                    configure(builder);
                    return builder.Build<AppFunc>();
                });
            });
            return app;
        }
    }
}
