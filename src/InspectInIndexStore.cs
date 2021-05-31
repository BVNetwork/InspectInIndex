using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find.Api;
using EPiServer.Find.Cms;
using EPiServer.Find.UI;
using EPiServer.Find.UI.Helpers;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.InspectInIndex
{
    [RestStore("inspectinindexstore")]
    public class InspectInIndexStore : RestControllerBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentIndexer _contentIndexer;
        private readonly IPathHelper _pathHelper;
        private readonly IFindUIConfiguration _findUiConfiguration;
        private readonly LanguageRoutingFactory _languageRoutingFactory;

        public InspectInIndexStore(IContentLoader contentLoader, IContentIndexer contentIndexer, IPathHelper pathHelper, IFindUIConfiguration findUiConfiguration, LanguageRoutingFactory languageRoutingFactory)
        {
            _contentLoader = contentLoader;
            _contentIndexer = contentIndexer;
            _pathHelper = pathHelper;
            _findUiConfiguration = findUiConfiguration;
            _languageRoutingFactory = languageRoutingFactory;
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
            _contentIndexer.Index(content);

            var rest = Rest(new { path = GetIndexContentPath(content) });
            return rest;
        }

        public ActionResult Delete(ContentReference id)
        {
            var content = _contentLoader.Get<IContent>(id);
            _contentIndexer.Delete(content);

            return null;
        }

        private string GetIndexContentPath(IContent content)
        {
            string findProxyPath = _pathHelper.EnsureTrailingSlash(_pathHelper.GetPathInModule(_findUiConfiguration.AdminProxyPath));
            return findProxyPath + GetTypeAndIndexId(content) + GetLanguageRoutingParameter(content);
        }

        private string GetTypeAndIndexId(IContent content)
        {
            var type = content.GetOriginalType().FullName.Replace('.', '_');
            var indexId = content.GetIndexId();
            return type + '/' + indexId;
        }

        private string GetLanguageRoutingParameter(IContent content)
        {
            var  languageRouting = GetLanguageRouting(content);

            if (languageRouting != null)
            {
                return $"?language_routing={languageRouting.FieldSuffix}";
            }
            
            return string.Empty;
        }

        private LanguageRouting GetLanguageRouting(IContent content)
        {
            var locale = content as ILocale;

            if (locale == null)
            {
                return null;
            }

            return _languageRoutingFactory.CreateLanguageRouting(locale);
        }
    }
}