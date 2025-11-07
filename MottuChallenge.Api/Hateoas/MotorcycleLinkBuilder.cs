using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Response;

namespace MottuChallenge.Api.Hateoas;

public static class MotorcycleLinkBuilder
{
    public static List<HateoasLink> BuildMotorcycleLinks(IUrlHelper urlHelper, Guid id)
    {
        return new List<HateoasLink>
        {
            new HateoasLink("self", urlHelper.Action("GetMotorcycleById", "Motorcycles", new { id })!, "GET"),
            new HateoasLink("update", urlHelper.Action("Update", "Motorcycles", new { id })!, "PUT"),
            new HateoasLink("delete", urlHelper.Action("DeleteMotorcycle", "Motorcycles", new { id })!, "DELETE"),
            new HateoasLink("all", urlHelper.Action("GetAllMotorcyclesPaginated", "Motorcycles")!, "GET"),
            new HateoasLink("create", urlHelper.Action("SaveMotorcycle", "Motorcycles")!, "POST")
        };
    }

    public static List<HateoasLink> BuildCollectionLinks(IUrlHelper urlHelper, int page, int pageSize, string? plate)
    {
        return new List<HateoasLink>
        {
            new HateoasLink("self", urlHelper.Action("GetAllMotorcyclesPaginated", "Motorcycles", new { page, pageSize, plate })!, "GET"),
            new HateoasLink("create", urlHelper.Action("SaveMotorcycle", "Motorcycles")!, "POST")
        };
    }

    public static List<HateoasLink> BuildDeleteLinks(IUrlHelper urlHelper)
    {
        return new List<HateoasLink>
        {
            new HateoasLink("all", urlHelper.Action("GetAllMotorcyclesPaginated", "Motorcycles")!, "GET"),
            new HateoasLink("create", urlHelper.Action("SaveMotorcycle", "Motorcycles")!, "POST")
        };
    }
}
    