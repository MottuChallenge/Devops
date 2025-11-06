using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Response;

namespace MottuChallenge.Api.Hateoas
{
    public static class YardLinkBuilder
    {
        public static List<HateoasLink> BuildYardLinks(IUrlHelper urlHelper, Guid id)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("self", urlHelper.Action("GetById", "Yard", new { id })!, "GET"),
                new HateoasLink("update", urlHelper.Action("UpdateYardName", "Yard", new { id })!, "PUT"),
                new HateoasLink("delete", urlHelper.Action("DeleteYard", "Yard", new { id })!, "DELETE"),
                new HateoasLink("all", urlHelper.Action("GetAllYardsAsync", "Yard")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "Yard")!, "POST"),
                new HateoasLink("paginated", urlHelper.Action("GetAllPaginated", "Yard")!, "GET")
            };
        }

        public static List<HateoasLink> BuildCollectionLinks(IUrlHelper urlHelper)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("self", urlHelper.Action("GetAllYardsAsync", "Yard")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "Yard")!, "POST"),
                new HateoasLink("paginated", urlHelper.Action("GetAllPaginated", "Yard")!, "GET")
            };
        }

        public static List<HateoasLink> BuildDeleteLinks(IUrlHelper urlHelper)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("all", urlHelper.Action("GetAllYardsAsync", "Yard")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "Yard")!, "POST"),
                new HateoasLink("paginated", urlHelper.Action("GetAllPaginated", "Yard")!, "GET")
            };
        }
    }
}