using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.DTOs.Validations;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.UseCases.Yards;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [Route("api/yards")]
    [ApiController]
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
        /// <param name="createYardDto">Objeto contendo os dados do pátio.</param>
        /// <returns>Retorna o pátio criado com status 201.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST /api/yards
        ///     {
        ///        "name": "Pátio Central",
        ///        "cep": "12345-678",
        ///        "number": "100",
        ///        "points": [
        ///          { "pointOrder": 1, "x": 10.5, "y": 20.5 },
        ///          { "pointOrder": 2, "x": 15.0, "y": 25.0 },
        ///          { "pointOrder": 3, "x": 20.5, "y": 20.5 }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Pátio criado com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        [HttpPost]
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
                return CreatedAtAction(nameof(GetById), new { id = createdYard.Id }, createdYard);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os pátios.
        /// </summary>
        /// <returns>Lista de pátios.</returns>
        /// <response code="200">Retorna a lista de pátios.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllYardsAsync()
        {
            try
            {
                var yards = await _getAllYardsUseCase.FindAllYards();
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
        /// <param name="id">ID do pátio.</param>
        /// <returns>Dados do pátio encontrado.</returns>
        /// <response code="200">Pátio encontrado.</response>
        /// <response code="404">Pátio não encontrado.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var yard = await _getYardByIdUseCase.FindYardById(id);
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
        /// <param name="id">ID do pátio.</param>
        /// <param name="dto">Objeto contendo o novo nome do pátio.</param>
        /// <returns>Status 204 se atualizado com sucesso.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT /api/yards/123e4567-e89b-12d3-a456-426614174000
        ///     {
        ///        "name": "Pátio Renovado"
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
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
        /// <param name="id">ID do pátio.</param>
        /// <returns>Status 204 se removido com sucesso.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     DELETE /api/yards/123e4567-e89b-12d3-a456-426614174000
        ///
        /// </remarks>
        [HttpDelete("{id}")]
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
        /// <param name="page">Número da página (default: 1).</param>
        /// <param name="pageSize">Tamanho da página (default: 10).</param>
        /// <param name="name">Filtro pelo nome do pátio (opcional).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Resultado paginado de pátios.</returns>
        [HttpGet("paginated")]
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
