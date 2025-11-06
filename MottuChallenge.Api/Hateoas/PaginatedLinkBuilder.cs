using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Response;

namespace MottuChallenge.Api.Hateoas;

public static class PaginatedLinkBuilder
{
    public static List<HateoasLink> BuildPaginatedLinks(string method, string controllerName, IUrlHelper url, int pageNumber, int pageSize, int totalPages)
    {
        
        var links = new List<HateoasLink>();

        var selfUrl = url.Action(method, controllerName, new { page = pageNumber, pageSize });
        links.Add(new HateoasLink("self", selfUrl, "GET"));

        if (pageNumber < totalPages)
        {
            var nextPage = pageNumber + 1;
            var nextUrl = url.Action(method, controllerName, new { page = nextPage, pageSize });
            links.Add(new HateoasLink("next", nextUrl, "GET"));
        }

        if (pageNumber > 1)
        {
            var prevPage = pageNumber - 1;
            var prevUrl = url.Action(method, controllerName, new { page = prevPage, pageSize });
            links.Add(new HateoasLink("prev", prevUrl, "GET"));
        }

        return links;
    }
}