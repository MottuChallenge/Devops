using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Validations;
using MottuChallenge.Application.UseCases.SectorTypes;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [Route("api/sectors_type")]
    [ApiController]
    public class SectorTypeController : ControllerBase
    {
        private readonly CreateSectorTypeUseCase _createSectorTypeUseCase;
        private readonly GetAllSectorTypesUseCase _getAllSectorTypesUseCase;
        private readonly UpdateSectorTypeUseCase _updateSectorTypeUseCase;
        private readonly DeleteSectorTypeUseCase _deleteSectorTypeUseCase;
        private readonly GetSectorTypeByIdUseCase _getSectorTypeByIdUseCase;

        public SectorTypeController(
            CreateSectorTypeUseCase createSectorTypeUseCase,
            GetAllSectorTypesUseCase getAllSectorTypesUseCase,
            UpdateSectorTypeUseCase updateSectorTypeUseCase,
            DeleteSectorTypeUseCase deleteSectorTypeUseCase,
            GetSectorTypeByIdUseCase getSectorTypeByIdUseCase)
        {
            _createSectorTypeUseCase = createSectorTypeUseCase;
            _getAllSectorTypesUseCase = getAllSectorTypesUseCase;
            _updateSectorTypeUseCase = updateSectorTypeUseCase;
            _deleteSectorTypeUseCase = deleteSectorTypeUseCase;
            _getSectorTypeByIdUseCase = getSectorTypeByIdUseCase;
        }

        /// <summary>
        /// Cria um novo tipo de setor.
        /// </summary>
        /// <param name="sectorTypeCreateDto">Objeto contendo os dados do tipo de setor.</param>
        /// <returns>Retorna o tipo de setor criado com status 201.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST /api/sectors_type
        ///     {
        ///        "name": "Setor VIP"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Tipo de setor criado com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SectorTypeDto sectorTypeCreateDto)
        {
            var validator = new SectorTypeDtoValidator();
            var result = validator.Validate(sectorTypeCreateDto);

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
                var createdSectorType = await _createSectorTypeUseCase.SaveSectorType(sectorTypeCreateDto);
                return CreatedAtAction(nameof(GetById), new { id = createdSectorType.Id }, createdSectorType);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os tipos de setores.
        /// </summary>
        /// <returns>Lista de tipos de setores.</returns>
        /// <response code="200">Retorna a lista de tipos de setores.</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var sectorTypes = await _getAllSectorTypesUseCase.FindAllSectorTypes();
                return Ok(sectorTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um tipo de setor existente pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de setor a ser atualizado.</param>
        /// <param name="sectorTypeDto">Objeto contendo os novos dados do tipo de setor.</param>
        /// <returns>Tipo de setor atualizado.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT /api/sectors_type/123e4567-e89b-12d3-a456-426614174000
        ///     {
        ///        "name": "Setor Premium"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Tipo de setor atualizado com sucesso.</response>
        /// <response code="400">Falha de validação.</response>
        /// <response code="404">Tipo de setor não encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] SectorTypeDto sectorTypeDto)
        {
            try
            {
                var updatedSectorType = await _updateSectorTypeUseCase.UpdateSectorTypeById(sectorTypeDto, id);
                return Ok(updatedSectorType);
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
        /// Remove um tipo de setor pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de setor a ser removido.</param>
        /// <returns>Status 204 se removido com sucesso.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     DELETE /api/sectors_type/123e4567-e89b-12d3-a456-426614174000
        ///
        /// </remarks>
        /// <response code="204">Tipo de setor removido com sucesso.</response>
        /// <response code="404">Tipo de setor não encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _deleteSectorTypeUseCase.DeleteSectorTypeById(id);
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
        /// Consulta um tipo de setor pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de setor.</param>
        /// <returns>Dados do tipo de setor encontrado.</returns>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     GET /api/sectors_type/123e4567-e89b-12d3-a456-426614174000
        ///
        /// </remarks>
        /// <response code="200">Tipo de setor encontrado.</response>
        /// <response code="404">Tipo de setor não encontrado.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var sectorType = await _getSectorTypeByIdUseCase.FindSectorTypeById(id);
                return Ok(sectorType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
