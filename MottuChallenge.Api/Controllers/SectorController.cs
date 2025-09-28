using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.DTOs.Validations;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.UseCases.Sectors;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [Route("api/sectors")]
    [ApiController]
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
        /// <param name="sectorCreateDto">Objeto com os dados do setor a ser criado.</param>
        /// <returns>Retorna o setor criado com status 201.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST /api/sectors
        ///     {
        ///        "yardId": "123e4567-e89b-12d3-a456-426614174000",
        ///        "sectorTypeId": "987e6543-e21b-12d3-a456-426614174999",
        ///        "points": [
        ///           { "pointOrder": 1, "x": 10.5, "y": 20.7 },
        ///           { "pointOrder": 2, "x": 15.2, "y": 25.3 },
        ///           { "pointOrder": 3, "x": 12.8, "y": 22.1 }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Setor criado com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Recurso relacionado não encontrado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(SectorResponseDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Post([FromBody] SectorCreateDto sectorCreateDto)
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
        /// <returns>Lista de setores.</returns>
        /// <response code="200">Retorna a lista de setores.</response>
        /// <response code="400">Falha ao processar a requisição.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<SectorResponseDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAllSectorsAsync()
        {
            try
            {
                var sectors = await _getAllSectorsUseCase.FindAllSectors();
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
        /// <param name="id">ID do setor.</param>
        /// <returns>Setor encontrado.</returns>
        /// <response code="200">Setor encontrado.</response>
        /// <response code="404">Setor não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SectorResponseDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var sector = await _getSectorByIdUseCase.FindSectorById(id);
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
        /// <param name="id">ID do setor a ser atualizado.</param>
        /// <param name="dto">Objeto com os novos dados do setor.</param>
        /// <returns>Status 204 se atualizado com sucesso.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT /api/sectors/123e4567-e89b-12d3-a456-426614174000
        ///     {
        ///        "sectorTypeId": "11111111-1111-1111-1111-111111111111"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Setor atualizado com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Setor não encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
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
        /// <param name="id">ID do setor a ser removido.</param>
        /// <returns>Status 204 se removido com sucesso.</returns>
        /// <response code="204">Setor removido com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Setor não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
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
        /// <param name="page">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Itens por página (padrão: 10).</param>
        /// <param name="yardId">Filtro opcional por pátio.</param>
        /// <param name="sectorTypeId">Filtro opcional por tipo de setor.</param>
        /// <param name="ct">Token de cancelamento (não aparece no Swagger).</param>
        /// <returns>Resultado paginado de setores.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     GET /api/sectors/paginated?page=1&pageSize=10&yardId=00000000-0000-0000-0000-000000000000&sectorTypeId=11111111-1111-1111-1111-111111111111
        ///
        /// </remarks>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(PaginatedResult<SectorResponseDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
