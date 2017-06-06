using Microsoft.Owin;
using ODataWithCustomMetadata;

[assembly: OwinStartup(typeof(Startup))]
namespace ODataWithCustomMetadata
{
    using System.Web.Http;
    using Microsoft.Owin;
    using Microsoft.Owin.Extensions;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.StaticFiles;
    using Owin;
    using ValidationMetadataExtractor;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();

            // Configure Web API Routes:
            // - Enable Attribute Mapping
            // - Enable Default routes at /api.
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(httpConfiguration);

            // Make ./public the default root of the static files in our Web Application.
            app.UseFileServer(new FileServerOptions
            {
                RequestPath = new PathString(string.Empty),
                FileSystem = new PhysicalFileSystem("./public"),
                EnableDirectoryBrowsing = true,
            });

            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }

    public class ValidationMetadataSource
    {
        public static ValidationMetadataSource Instance = new ValidationMetadataSource();
        private ValidationMetadataSource()
        {
            ValidationMetadatas = new List<ObjectValidationMetadata>();
            Initialize(); ;
        }

        public void Reset()
        {
            ValidationMetadatas.Clear();
        }

        public void Initialize()
        {
            Reset();
            ValidationMetadatas.Add(new ObjectValidationMetadata(typeof(Person)));
            ValidationMetadatas.Add(new ObjectValidationMetadata(typeof(Campus)));
        }

        public List<ObjectValidationMetadata> ValidationMetadatas;
    }
}
