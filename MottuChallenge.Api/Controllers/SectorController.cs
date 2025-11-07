using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using MottuChallenge.Api.Hateoas;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.DTOs.Validations;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.UseCases.Sectors;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [Route("api/v{version:apiVersion}/sectors")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Sectors - CRUD operations")]
    [ApiVersion(1.0)]
    public class SectorController : ControllerBase
    {
        private readonly CreateSectorUseCase _createSectorUseCase;
        private readonly GetAllSectorsUseCase _getAllSectorsUseCase;
        private readonly GetSectorByIdUseCase _getSectorByIdUseCase;
        private readonly UpdateSectorUseCase _updateSectorUseCase;
        private readonly DeleteSectorUseCase _deleteSectorUseCase;

        public SectorController(
            CreateSectorUseCase createSectorUseCase,
            GetAllSectorsUseCase getAllSectorsUseCase,
            GetSectorByIdUseCase getSectorByIdUseCase,
            UpdateSectorUseCase updateSectorUseCase,
            DeleteSectorUseCase deleteSectorUseCase)
        {
            _createSectorUseCase = createSectorUseCase;
            _getAllSectorsUseCase = getAllSectorsUseCase;
            _getSectorByIdUseCase = getSectorByIdUseCase;
            _updateSectorUseCase = updateSectorUseCase;
            _deleteSectorUseCase = deleteSectorUseCase;
        }

        /// <summary>
        /// Cria um novo setor.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Create new sector", Description = "Creates a new sector")]
        [ProducesResponseType(typeof(SectorResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] SectorCreateDto sectorCreateDto, CancellationToken ct)
        {
            var validator = new SectorCreateDtoValidator();
            var result = validator.Validate(sectorCreateDto);

            if (!result.IsValid)
            {
                var modelState = new ModelStateDictionary();
                foreach (var failure in result.Errors)
                {
                    modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                return ValidationProblem(modelState);
            }

            try
            {
                var createdSector = await _createSectorUseCase.SaveSector(sectorCreateDto);
                return CreatedAtAction(nameof(GetById), new { id = createdSector.Id }, createdSector);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os setores.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all sectors", Description = "Returns a list of all sectors")]
        [ProducesResponseType(typeof(List<SectorResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSectorsAsync()
        {
            try
            {
                var sectors = await _getAllSectorsUseCase.FindAllSectors();
                foreach (var sectorResponseDto in sectors)
                {
                    sectorResponseDto.Links = SectorLinkBuilder.BuildSectorLinks(Url, sectorResponseDto.Id);
                }
                return Ok(sectors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Consulta um setor pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get sector by id", Description = "Retrieves sector details by id")]
        [ProducesResponseType(typeof(SectorResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var sector = await _getSectorByIdUseCase.FindSectorById(id);
                sector.Links = SectorLinkBuilder.BuildSectorLinks(Url, id);

                return Ok(sector);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um setor existente.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Update sector", Description = "Updates an existing sector")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSectorType([FromRoute] Guid id, [FromBody] UpdateSectorDto dto)
        {
            try
            {
                await _updateSectorUseCase.UpdateSectorTypeAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove um setor pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete sector", Description = "Deletes a sector by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteSector([FromRoute] Guid id)
        {
            try
            {
                await _deleteSectorUseCase.DeleteSectorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista setores com paginação e filtros opcionais.
        /// </summary>
        [HttpGet("paginated")]
        [SwaggerOperation(Summary = "Get paginated sectors", Description = "Returns a paginated list of sectors")]
        [ProducesResponseType(typeof(PaginatedResult<SectorResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? yardId = null,
            [FromQuery] Guid? sectorTypeId = null,
            CancellationToken ct = default)
        {
            try
            {
                var pageRequest = new PageRequest
                {
                    Page = page,
                    PageSize = pageSize
                };

                var filter = new SectorQuery
                {
                    YardId = yardId.GetValueOrDefault(Guid.Empty),
                    SectorTypeId = sectorTypeId.GetValueOrDefault(Guid.Empty)
                };

                var result = await _getAllSectorsUseCase.FindAllSectorPageable(pageRequest, filter, ct);
                result.Links = PaginatedLinkBuilder.BuildPaginatedLinks("GetAllPaginated", "sectors", Url, page, pageSize, result.TotalPages);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
