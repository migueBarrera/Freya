using Microsoft.AspNetCore.Http;

namespace FreyaApi.Tests.Controllers;

internal class ControllerBaseBuilder
{
    internal TestController Build()
    {
        return new TestController();
    }
    
    internal TestController Build(HttpContext httpContext)
    {
        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        return new TestController() { ControllerContext = controllerContext };
    }
}

internal class TestController : ControllerBase
{

}
