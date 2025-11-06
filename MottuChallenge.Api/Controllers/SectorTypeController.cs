using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using MottuChallenge.Api.Hateoas;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.DTOs.Validations;
using MottuChallenge.Application.UseCases.SectorTypes;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Api.Controllers
{
    [Route("api/v{version:apiVersion}/sectors_type")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Sector Types - CRUD operations")]
    [ApiVersion(1.0)]
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
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Create new sector type", Description = "Creates a new sector type")]
        [ProducesResponseType(typeof(SectorTypeResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                var sectorTypeResponse = new SectorTypeResponseDto()
                {
                    Id = createdSectorType.Id,
                    Name = createdSectorType.Name,
                    Links = SectorTypeLinkBuilder.BuildSectorTypeLinks(Url, createdSectorType.Id)
                };

                return CreatedAtAction(nameof(GetById), new { id = createdSectorType.Id }, sectorTypeResponse);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os tipos de setores.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all sector types", Description = "Returns a list of all sector types")]
        [ProducesResponseType(typeof(List<SectorTypeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var sectorTypes = await _getAllSectorTypesUseCase.FindAllSectorTypes();
                foreach (var sectorTypeResponseDto in sectorTypes)
                {
                    sectorTypeResponseDto.Links = SectorTypeLinkBuilder.BuildSectorTypeLinks(Url, sectorTypeResponseDto.Id);
                }

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
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Update sector type", Description = "Updates an existing sector type")]
        [ProducesResponseType(typeof(SectorTypeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] SectorTypeDto sectorTypeDto)
        {
            try
            {
                var updatedSectorType = await _updateSectorTypeUseCase.UpdateSectorTypeById(sectorTypeDto, id);
                var response = new SectorTypeResponseDto()
                {
                    Id = updatedSectorType.Id,
                    Name = updatedSectorType.Name,
                    Links = SectorTypeLinkBuilder.BuildSectorTypeLinks(Url, updatedSectorType.Id)
                };
                return Ok(response);
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
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete sector type", Description = "Deletes a sector type by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get sector type by id", Description = "Retrieves sector type details by id")]
        [ProducesResponseType(typeof(SectorTypeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct = default)
        {
            try
            {
                var sectorType = await _getSectorTypeByIdUseCase.FindSectorTypeById(id);
                var sectorTypeResponse = new SectorTypeResponseDto()
                {
                    Id = sectorType.Id,
                    Name = sectorType.Name,
                    Links = SectorTypeLinkBuilder.BuildSectorTypeLinks(Url, sectorType.Id)
                };

                return Ok(sectorTypeResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
