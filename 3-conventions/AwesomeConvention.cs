using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

public class AwesomeConvention : IApplicationModelConvention
{

    public void Apply(ApplicationModel application)
    {
        var controllers = Assembly.GetExecutingAssembly().GetExportedTypes()
                              .Where(t => t.Name.EndsWith("Api"));

        foreach (var controller in controllers)
        {
            var controllerName = controller.Name.Replace("Api", string.Empty);
            var model = new ControllerModel(controller.GetTypeInfo(), controller.GetCustomAttributes().ToArray());
            model.ControllerName = controllerName;
            model.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = $"{controller.Namespace.Replace(".", "/")}/{controllerName}"
                }
            });

            foreach (var action in controller.GetMethods()?.Where(p => p.IsPublic && p.Module.Name == controller.Module.Name))
            {
                var httpMethod = ResolveHttpMethod(action.Name);

                var actionModel = new ActionModel(action, new object[] { httpMethod })
                {
                    ActionName = action.Name,
                    Controller = model
                };

                actionModel.Selectors.Add(new SelectorModel());

                model.Actions.Add(actionModel);
            }
            application.Controllers.Add(model);
        }


    }

    private HttpMethodAttribute ResolveHttpMethod(string name)
    {
        switch (name.ToUpper())
        {
            case "GET":
                return new HttpGetAttribute();
            case "PUT":
                return new HttpGetAttribute();
            case "POST":
                return new HttpGetAttribute();
            case "DELETE":
                return new HttpGetAttribute();

            default:
                return null;
        }
    }
}

