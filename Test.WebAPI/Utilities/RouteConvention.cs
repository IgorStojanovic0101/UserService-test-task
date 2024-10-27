

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.RegularExpressions;

namespace Test.WebAPI.Utilities
{
    public class RouteConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var controllerNamespace = controller.ControllerType.Namespace;
                var controllerName = AddHyphenBetweenCamelCase(controller.ControllerName);


                controller.Filters.Add(new ApiControllerAttribute());

                foreach (var action in controller.Actions)
                {
                    var actionName = AddHyphenBetweenCamelCase(action.ActionName);


                    action.Selectors[0].AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = $"api/{controllerName}/{actionName}"
                    };
                }
            }
        }

        private string AddHyphenBetweenCamelCase(string input)
        {
            return Regex.Replace(input, "(?<=.)([A-Z])", "-$1").ToLower();
        }
    }
}
