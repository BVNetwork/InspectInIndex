using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Find.UI;
using EPiServer.Find.UI.Helpers;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.InspectInIndex
{
    [RestStore("inspectinindexstore")]
    public class InspectInIndexStore : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPathHelper _pathHelper;
        private readonly IFindUIConfiguration _findUiConfiguration;

        public InspectInIndexStore(IContentLoader contentLoader, IPathHelper pathHelper, IFindUIConfiguration findUiConfiguration)
        {
            _contentLoader = contentLoader;
            _pathHelper = pathHelper;
            _findUiConfiguration = findUiConfiguration;
        }

        [HttpGet]
        public ActionResult Get(ContentReference id)
        {           
            var content = _contentLoader.Get<IContent>(id);
            var rest = Rest(new { path = GetIndexContentPath(content) });
            return rest;
        }

        [HttpPost]
        public ActionResult Post(ContentReference reference)
        {
            var content = _contentLoader.Get<IContent>(reference);
            SearchClient.Instance.Index(content, x => x.Refresh = true);
            var rest = Rest(new { path = GetIndexContentPath(content) });
            return rest;
        }

        public ActionResult Delete(ContentReference id)
        {   
            var content = _contentLoader.Get<IContent>(id);
            SearchClient.Instance.Delete(content.GetOriginalType(), content.GetIndexId(), null);
            return null;
        }

        private string GetIndexContentPath(IContent content)
        {
            string findProxyPath = _pathHelper.EnsureTrailingSlash(_pathHelper.GetPathInModule(_findUiConfiguration.AdminProxyPath));
            return findProxyPath + GetTypeAndIndexId(content);
        }

        private string GetTypeAndIndexId(IContent content)
        {
            var type = content.GetOriginalType().FullName.Replace('.', '_');
            var indexId = content.GetIndexId();
            return type + '/' + indexId;
        }
    }
}