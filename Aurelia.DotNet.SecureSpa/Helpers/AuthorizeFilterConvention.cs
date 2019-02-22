using Aurelia.DotNet.Spa.Security;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Aurelia.DotNet.Spa
{
    public class AuthorizeFilterConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Filters.Add(new AuthorizeFilter(Policies.Default));
        }
    }
}
