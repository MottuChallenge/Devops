using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Response;

namespace MottuChallenge.Api.Hateoas
{
    public static class SectorLinkBuilder
    {
        public static List<HateoasLink> BuildSectorLinks(IUrlHelper urlHelper, Guid id)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("self", urlHelper.Action("GetById", "Sector", new { id })!, "GET"),
                new HateoasLink("update", urlHelper.Action("UpdateSectorType", "Sector", new { id })!, "PUT"),
                new HateoasLink("delete", urlHelper.Action("DeleteSector", "Sector", new { id })!, "DELETE"),
                new HateoasLink("all", urlHelper.Action("GetAllSectorsAsync", "Sector")!, "GET"),
                new HateoasLink("paginated", urlHelper.Action("GetAllPaginated", "Sector")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "Sector")!, "POST")
            };
        }

        public static List<HateoasLink> BuildCollectionLinks(IUrlHelper urlHelper, int page, int pageSize, Guid? yardId, Guid? sectorTypeId)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("self", urlHelper.Action("GetAllPaginated", "Sector", new { page, pageSize, yardId, sectorTypeId })!, "GET"),
                new HateoasLink("all", urlHelper.Action("GetAllSectorsAsync", "Sector")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "Sector")!, "POST")
            };
        }

        public static List<HateoasLink> BuildDeleteLinks(IUrlHelper urlHelper)
        {
            return new List<HateoasLink>
            {
                new HateoasLink("all", urlHelper.Action("GetAllSectorsAsync", "Sector")!, "GET"),
                new HateoasLink("create", urlHelper.Action("Post", "Sector")!, "POST")
            };
        }
    }
}