using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.UseCases.Motorcycles;
using MottuChallenge.Domain.Exceptions;


namespace MottuChallenge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        /// <returns>Retorna a motocicleta criada com status 201.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST /api/motorcycles
        ///     {
        ///        "plate": "ABC1234",
        ///        "model": "Yamaha XJ6",
        ///        "engineType": 0, // 0: COMBUSTION, 1: ELETRIC,
        ///        "lastRevisionDate": "2025-09-26T00:00:00",
        ///        "spotId": "00000000-0000-0000-0000-000000000000" // opcional
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Motocicleta criada com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Recurso relacionado não encontrado.</response>
        [HttpPost]
        public async Task<IActionResult> SaveMotorcycle([FromBody] MotorcycleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var motorcycle = await _createMotorcycleUseCase.SaveMotorcycleAsync(dto);
                return CreatedAtAction(nameof(GetMotorcycleById), new { id = motorcycle.Id }, motorcycle);
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
        /// <param name="page">Número da página (opcional, padrão: 1).</param>
        /// <param name="pageSize">Quantidade de itens por página (opcional, padrão: 10).</param>
        /// <param name="plate">Filtro opcional pela placa da moto.</param>
        /// <returns>Lista paginada de motocicletas.</returns>
        /// <response code="200">Retorna a lista de motocicletas.</response>
        /// <response code="400">Falha de validação.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<MotorcycleResponseDto>), 200)]
        public async Task<IActionResult> GetAllMotorcyclesPaginated(int page = 1, int pageSize = 10, string? plate = null)
        {
            var pageRequest = new PageRequest { Page = page, PageSize = pageSize };
            var filter = new MotorcycleQuery { Plate = plate };

            try
            {
                var paginatedResult = await _getAllMotorcyclesPageableUseCase.FindAllMotorcyclePageable(pageRequest, filter);
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
        /// <param name="id">ID da motocicleta a ser atualizada.</param>
        /// <param name="dto">Objeto contendo os novos dados da motocicleta.</param>
        /// <returns>Status 204 se atualizado com sucesso.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT /api/motorcycles/123e4567-e89b-12d3-a456-426614174000
        ///     {
        ///        plate": "ABC1234",
        ///        "model": "Yamaha XJ6",
        ///        "engineType": 0, // 0: COMBUSTION, 1: ELETRIC,
        ///        "lastRevisionDate": "2025-09-26T00:00:00",
        ///        "spotId": "00000000-0000-0000-0000-000000000000" // opcional
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Motocicleta atualizada com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Motocicleta não encontrada.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] MotorcycleDto dto)
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
        /// <param name="id">ID da motocicleta a ser removida.</param>
        /// <returns>Status 204 se removida com sucesso.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     DELETE /api/motorcycles/123e4567-e89b-12d3-a456-426614174000
        ///
        /// </remarks>
        /// <response code="204">Motocicleta removida com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Motocicleta não encontrada.</response>
        [HttpDelete("{id}")]
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
        /// <param name="id">ID da motocicleta.</param>
        /// <returns>Dados da motocicleta encontrada.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     GET /api/motorcycles/123e4567-e89b-12d3-a456-426614174000
        ///
        /// </remarks>
        /// <response code="200">Motocicleta encontrada.</response>
        /// <response code="404">Motocicleta não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycleById([FromRoute] Guid id)
        {
            try
            {
                var motorcycle = await _getMotorcycleByIdUseCase.FindMotorcycleById(id);
                return Ok(motorcycle);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
