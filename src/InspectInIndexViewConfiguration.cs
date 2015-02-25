using System;
using System.Configuration;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell;

namespace EPiCode.InspectInIndex
{
    [ServiceConfiguration(typeof(ViewConfiguration))]
    public class InspectInIndexViewConfiguration : ViewConfiguration<IContentData>
    {
        public InspectInIndexViewConfiguration()
        {
            string roles = ConfigurationManager.AppSettings["InspectInIndexRoles"];
            if (string.IsNullOrEmpty(roles))
            {
                roles = "Administrators";
            }
            foreach (var role in roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!PrincipalInfo.CurrentPrincipal.IsInRole(role))
                {
                    HideFromViewMenu = true;
                }
                else
                {
                    HideFromViewMenu = false;
                    break;
                }
            }       
            Key = "InspectInIndex";
            Name = "Inspect in index";
            Description = "Show content in Find index";
            IconClass = "iii-icon";
            ControllerType = "inspectinindex/InspectInIndexView";
        }
    }
}