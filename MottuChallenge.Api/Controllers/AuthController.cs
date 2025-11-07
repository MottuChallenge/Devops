using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.UseCases.Employees;
using MottuChallenge.Infrastructure.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuChallenge.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[SwaggerTag("Authentication - Employee Registration and Login")]
[ApiVersion(2.0)]
public class AuthController : ControllerBase
{
    private readonly CreateEmployeeUseCase _createEmployeeUseCase;
    private readonly LoginUseCase _loginUseCase;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        CreateEmployeeUseCase createEmployeeUseCase,
        LoginUseCase loginUseCase,
        JwtTokenService jwtTokenService)
    {
        _createEmployeeUseCase = createEmployeeUseCase;
        _loginUseCase = loginUseCase;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    [Consumes("application/json")]
    [SwaggerOperation(Summary = "Create new Employee", Description = "Creates a new Employee")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateEmployeeRequest request, CancellationToken ct)
    {
        var result = await _createEmployeeUseCase.createEmployee(request.Name, request.Email, request.YardId, request.Password);
        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    [Consumes("application/json")]
    [SwaggerOperation(Summary = "Employee Login", Description = "Authenticates an employee and returns a JWT token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var employee = await _loginUseCase.LoginEmployee(request.Email, request.Password, ct);
        if (employee == null) return Unauthorized("Invalid credentials");
        var token = _jwtTokenService.GenerateToken(employee.Email);
        return Ok(token);
    }
}
