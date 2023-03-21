using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;

/// <summary>
/// Add Route mapping component to the hubs
/// </summary>
public static class HubMappingExtension
{
    public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder endpoints)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type hub in assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(Hub))))
            {
                RouteAttribute routeAttribute = hub.GetCustomAttributes(typeof(RouteAttribute), true).FirstOrDefault() as RouteAttribute;
                if (!string.IsNullOrWhiteSpace(routeAttribute?.Template))
                {
                    MethodInfo mapHubMethod =
                        typeof(HubEndpointRouteBuilderExtensions).GetMethod(
                            nameof(HubEndpointRouteBuilderExtensions.MapHub),
                            new Type[] { typeof(IEndpointRouteBuilder), typeof(string) });
                    mapHubMethod = mapHubMethod.MakeGenericMethod(hub);
                    mapHubMethod.Invoke(null, new object[] { endpoints, routeAttribute.Template });
                }
            }
        }
        return endpoints;
    }
}