using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MottuChallenge.Api.Hateoas;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.UseCases.Motorcycles;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Motorcycles - CRUD operations")]
    [ApiVersion(1.0)]
    public class MotorcyclesController : ControllerBase
    {
        private readonly CreateMotorcycleUseCase _createMotorcycleUseCase;
        private readonly GetAllMotorcyclesPageableUseCase _getAllMotorcyclesPageableUseCase;
        private readonly UpdateMotorcycleUseCase _updateMotorcycleUseCase;
        private readonly DeleteMotorcycleUseCase _deleteMotorcycleUseCase;
        private readonly GetMotorcycleByIdUseCase _getMotorcycleByIdUseCase;

        public MotorcyclesController(
            CreateMotorcycleUseCase createMotorcycleUseCase,
            GetAllMotorcyclesPageableUseCase getAllMotorcyclesPageableUseCase,
            UpdateMotorcycleUseCase updateMotorcycleUseCase,
            DeleteMotorcycleUseCase deleteMotorcycleUseCase,
            GetMotorcycleByIdUseCase getMotorcycleByIdUseCase)
        {
            _createMotorcycleUseCase = createMotorcycleUseCase;
            _getAllMotorcyclesPageableUseCase = getAllMotorcyclesPageableUseCase;
            _updateMotorcycleUseCase = updateMotorcycleUseCase;
            _deleteMotorcycleUseCase = deleteMotorcycleUseCase;
            _getMotorcycleByIdUseCase = getMotorcycleByIdUseCase;
        }

        /// <summary>
        /// Cria uma nova motocicleta.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados da motocicleta.</param>
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Create a new motorcycle", Description = "Creates a new motorcycle record")]
        [ProducesResponseType(typeof(MotorcycleResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveMotorcycle([FromBody] MotorcycleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var motorcycle = await _createMotorcycleUseCase.SaveMotorcycleAsync(dto);
                var motorcycleReponse = new MotorcycleResponseDto()
                {
                    Id = motorcycle.Id,
                    Model = motorcycle.Model,
                    EngineType = motorcycle.EngineType,
                    Plate = motorcycle.Plate,
                    LastRevisionDate = motorcycle.LastRevisionDate,
                    SpotId = motorcycle.SpotId,
                    Links = MotorcycleLinkBuilder.BuildMotorcycleLinks(Url, motorcycle.Id)
                };

                return CreatedAtAction(nameof(GetMotorcycleById), new { id = motorcycle.Id }, motorcycleReponse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todas as motocicletas com paginação.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get paginated motorcycles", Description = "Returns a paginated list of motorcycles")]
        [ProducesResponseType(typeof(PaginatedResult<MotorcycleResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMotorcyclesPaginated(int page = 1, int pageSize = 10, string? plate = null, CancellationToken ct = default)
        {
            var pageRequest = new PageRequest { Page = page, PageSize = pageSize };
            var filter = new MotorcycleQuery { Plate = plate };

            try
            {
                var paginatedResult = await _getAllMotorcyclesPageableUseCase.FindAllMotorcyclePageable(pageRequest, filter, ct);
                paginatedResult.Links = PaginatedLinkBuilder.BuildPaginatedLinks("GetAllMotorcyclesPaginated", "Motorcycles", Url, page, pageSize, paginatedResult.TotalPages);

                return Ok(paginatedResult);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza uma motocicleta existente pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Update motorcycle by id", Description = "Updates an existing motorcycle")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] MotorcycleDto dto, CancellationToken ct)
        {
            try
            {
                await _updateMotorcycleUseCase.UpdateMotorcycleAsync(id, dto);
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
        /// Remove uma motocicleta pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete motorcycle", Description = "Deletes a motorcycle by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMotorcycle([FromRoute] Guid id)
        {
            try
            {
                await _deleteMotorcycleUseCase.DeleteMotorcycleAsync(id);
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
        /// Consulta uma motocicleta pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get motorcycle by id", Description = "Retrieves motorcycle details by id")]
        [ProducesResponseType(typeof(MotorcycleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMotorcycleById([FromRoute] Guid id)
        {
            try
            {
                var motorcycle = await _getMotorcycleByIdUseCase.FindMotorcycleById(id);
                var motorcycleReponse = new MotorcycleResponseDto()
                {
                    Id = motorcycle.Id,
                    Model = motorcycle.Model,
                    EngineType = motorcycle.EngineType,
                    Plate = motorcycle.Plate,
                    LastRevisionDate = motorcycle.LastRevisionDate,
                    SpotId = motorcycle.SpotId,
                    Links = MotorcycleLinkBuilder.BuildMotorcycleLinks(Url, motorcycle.Id)
                };

                return Ok(motorcycleReponse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
