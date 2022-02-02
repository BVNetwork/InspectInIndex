using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell;

namespace EPiCode.InspectInIndex
{
    [ServiceConfiguration(typeof(ViewConfiguration))]
    public class InspectInIndexViewConfiguration : ViewConfiguration<IContentData>
    {
        public InspectInIndexViewConfiguration()
        {
            //TODO: Find a solution to restrict access to plugin
            // HideFromViewMenu is no longer evaluated for each editor. Not sure if this behaviour is
            // intended or a bug.
            // 
            //string roles = ConfigurationManager.AppSettings["EPiCode.InspectInIndex.AllowedRoles"];
            //if (string.IsNullOrEmpty(roles))
            //{
            //    roles = "CmsAdmins,SearchAdmins";
            //}
            //foreach (var role in roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    if (!PrincipalInfo.CurrentPrincipal.IsInRole(role.Trim()))
            //    {
            //        HideFromViewMenu = true;
            //    }
            //    else
            //    {
            //        HideFromViewMenu = false;
            //        break;
            //    }
            //}

            Key = "InspectInIndex";
            Name = "Inspect in index";
            Description = "Show content in Find index";
            IconClass = "iii-icon";
            ControllerType = "inspectinindex/InspectInIndexView";
        }
    }
}