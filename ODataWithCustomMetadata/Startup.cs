using System.Collections.Generic;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin;
using ODataWithCustomMetadata;
using ODataWithCustomMetadata.Models;

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

            ConfigOData(httpConfiguration);

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
        public void ConfigOData(HttpConfiguration config)
        {
            config.MapODataServiceRoute("ODataRoute", "odata", GetEdmModel());
            config.EnsureInitialized();
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "ODataWithCustomMetadata";
            builder.ContainerName = "DefaultContainer";
            builder.EntitySet<ObjectValidationMetadata>("ValidationMetadata").EntityType.HasKey(x=>x.TypeName);
            var edmModel = builder.GetEdmModel();
            return edmModel;
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
