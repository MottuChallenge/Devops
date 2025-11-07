using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Response;

namespace MottuChallenge.Api.Hateoas
{
    public static class SectorTypeLinkBuilder
    {
        public static List<HateoasLink> BuildSectorTypeLinks(IUrlHelper urlHelper, Guid id)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("self", urlHelper.Action("GetById", "SectorType", new { id })!, "GET"),
                new HateoasLink("update", urlHelper.Action("Put", "SectorType", new { id })!, "PUT"),
                new HateoasLink("delete", urlHelper.Action("Delete", "SectorType", new { id })!, "DELETE"),
                new HateoasLink("all", urlHelper.Action("Get", "SectorType")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "SectorType")!, "POST")
            };
        }

        public static List<HateoasLink> BuildCollectionLinks(IUrlHelper urlHelper)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("self", urlHelper.Action("Get", "SectorType")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "SectorType")!, "POST")
            };
        }

        public static List<HateoasLink> BuildDeleteLinks(IUrlHelper urlHelper)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("all", urlHelper.Action("Get", "SectorType")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "SectorType")!, "POST")
            };
        }
    }
}