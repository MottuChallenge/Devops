// csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using MottuChallenge.Api.Hateoas;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.DTOs.Validations;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.UseCases.Yards;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [Route("api/v{version:apiVersion}/yards")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Yards - CRUD operations")]
    [ApiVersion(1.0)]
    public class YardController : ControllerBase
    {
        private readonly CreateYardUseCase _createYardUseCase;
        private readonly GetAllYardsUseCase _getAllYardsUseCase;
        private readonly GetYardByIdUseCase _getYardByIdUseCase;
        private readonly UpdateYardUseCase _updateYardUseCase;
        private readonly DeleteYardUseCase _deleteYardUseCase;

        public YardController(
            CreateYardUseCase createYardUseCase,
            GetAllYardsUseCase getAllYardsUseCase,
            GetYardByIdUseCase getYardByIdUseCase,
            UpdateYardUseCase updateYardUseCase,
            DeleteYardUseCase deleteYardUseCase)
        {
            _createYardUseCase = createYardUseCase;
            _getAllYardsUseCase = getAllYardsUseCase;
            _getYardByIdUseCase = getYardByIdUseCase;
            _updateYardUseCase = updateYardUseCase;
            _deleteYardUseCase = deleteYardUseCase;
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Create new yard", Description = "Creates a new yard")]
        [ProducesResponseType(typeof(YardResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateYardDto createYardDto)
        {
            var validator = new CreateYardDtoValidator();
            var result = validator.Validate(createYardDto);

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
                var createdYard = await _createYardUseCase.SaveYard(createYardDto);
                var addressResponse = new AddressResponseDto()
                {
                    ZipCode = createdYard.Address.ZipCode,
                    Street = createdYard.Address.Street,
                    Number = createdYard.Address.Number,
                    City = createdYard.Address.City,
                    State = createdYard.Address.State
                };

                var pointResponses = createdYard.Points.Select(p => new PointResponseDto()
                {
                    PointOrder = p.PointOrder,
                    X = p.X,
                    Y = p.Y
                }).ToList();
                var yardResponse = new YardResponseDto()
                {
                    Id = createdYard.Id,
                    Name = createdYard.Name,
                    Address = addressResponse,
                    Points = pointResponses,
                    Links = YardLinkBuilder.BuildYardLinks(Url, createdYard.Id)
                };

                return CreatedAtAction(nameof(GetById), new { id = createdYard.Id }, yardResponse);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os pátios.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all yards", Description = "Returns a list of all yards")]
        [ProducesResponseType(typeof(List<YardResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllYardsAsync()
        {
            try
            {
                var yards = await _getAllYardsUseCase.FindAllYards();
                foreach (var yardResponseDto in yards)
                {
                    yardResponseDto.Links = YardLinkBuilder.BuildYardLinks(Url, yardResponseDto.Id);
                }

                return Ok(yards);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Consulta um pátio pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get yard by id", Description = "Retrieves yard details by id")]
        [ProducesResponseType(typeof(YardResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var yard = await _getYardByIdUseCase.FindYardById(id);
                yard.Links = YardLinkBuilder.BuildYardLinks(Url, yard.Id);

                return Ok(yard);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza o nome de um pátio existente pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Update yard name", Description = "Updates the yard name by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateYardName([FromRoute] Guid id, [FromBody] UpdateYardDto dto)
        {
            try
            {
                await _updateYardUseCase.UpdateYardNameAsync(id, dto);
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
        /// Remove um pátio pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete yard", Description = "Deletes a yard by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteYard([FromRoute] Guid id)
        {
            try
            {
                await _deleteYardUseCase.DeleteYardAsync(id);
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
        /// Lista pátios paginados.
        /// </summary>
        [HttpGet("paginated")]
        [SwaggerOperation(Summary = "Get paginated yards", Description = "Returns a paginated list of yards")]
        [ProducesResponseType(typeof(PaginatedResult<YardResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            CancellationToken ct = default)
        {
            var pageRequest = new PageRequest
            {
                Page = page,
                PageSize = pageSize
            };

            var filter = new YardQuery
            {
                Name = name
            };

            try
            {
                var result = await _getAllYardsUseCase.FindAllYardPageable(pageRequest, filter, ct);
                result.Links = PaginatedLinkBuilder.BuildPaginatedLinks("GetAllPaginated", "yards", Url, page, pageSize, result.TotalPages);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
