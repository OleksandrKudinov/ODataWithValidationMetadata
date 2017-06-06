using System.Linq;
using System.Web.OData;

namespace ODataWithCustomMetadata.Controllers
{
    using System.Web.Http;

    [EnableQuery]
    public class ValidationMetadataController : ODataController
    {
        public IHttpActionResult GetValues()
        {
            return Ok(ValidationMetadataSource.Instance.ValidationMetadatas.AsQueryable());
        }
    }
}
