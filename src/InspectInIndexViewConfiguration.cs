using System.Security.Claims;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using Microsoft.AspNetCore.Authorization;

namespace EPiCode.InspectInIndex
{
    [ServiceConfiguration(typeof(ViewConfiguration))]
    public class InspectInIndexViewConfiguration : ViewConfiguration<IContentData>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IPrincipalAccessor _principalAccessor;

        public InspectInIndexViewConfiguration(IAuthorizationService authorizationService, IPrincipalAccessor principalAccessor) : base()
        {
            _authorizationService = authorizationService;
            _principalAccessor = principalAccessor;
            
            Key = "InspectInIndex";
            Name = "Inspect in index";
            Description = "Show content in Find index";
            IconClass = "iii-icon";
            ControllerType = "inspectinindex/InspectInIndexView";
        }
        
        public override bool HideFromViewMenu
        {
            get
            {
                if(_principalAccessor.Principal is ClaimsPrincipal principal)
                {
                    var result = _authorizationService.AuthorizeAsync(principal, Constants.PolicyName).Result;
                    return result.Succeeded == false;
                }

                return true;
            }
        }
    }
}