using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HouseStoreApi.Configuration;
using HouseStoreApi.Services;
using HouseStoreApi.Models;
using System.Security.Claims;


namespace HouseStoreApi.controllers;

[ApiController]
[Route("api/[Controller]")]
public class AuthController : ControllerBase
{


    private readonly ILogger<AuthController> _logger;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly RetailorsService _realtorServices;


    public AuthController(ILogger<AuthController> logger, IOptions<JwtConfiguration> jwtConfiguration, RetailorsService realtorServices)
    {
        _logger = logger;
        _jwtConfiguration = jwtConfiguration.Value;
        _realtorServices = realtorServices;
    }

    [HttpGet]
    public async Task<ActionResult<List<Retailor>>> Get() { return Ok(await _realtorServices.GetAsync()); }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(Retailor user)
    {
        var realtor = await _realtorServices.GetAsyncEmail(user.email);

        if (realtor == null)
        {
            return BadRequest("Email is not registered");
        }

        if (realtor.password != user.password)
        {
            return BadRequest("Password is incorrect");
        }

        var token = GenerateToken(realtor, "Realtor");

        // Return the token and a message indicating successful login
        return Ok(new { message = "Logged in successfully", token });
    }



    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Retailor userDto)
    {
        // Check if the email already exists in the database
        var existingUser = await _realtorServices.GetAsyncEmail(userDto.email);
        if (existingUser != null)
        {
            return BadRequest("Email already exists");
        }

        // Create a new user
        var user = new Retailor
        {
            fullName = userDto.fullName,
            phone = userDto.phone,
            email = userDto.email,
            password = userDto.password,
            specializations = userDto.specializations,
            experienceYear = userDto.experienceYear,
            description = userDto.description,
            activityRange = userDto.activityRange
        };

        // Add the user to the database
        await _realtorServices.CreateAsync(user);

        // Get the newly created user from the database to get the ID
        var createdUser = await _realtorServices.GetAsyncEmail(user.email);

        // Generate a token for the user
        var token = GenerateToken(createdUser, "Realtor");

        // Return the user and the token in the response
        return CreatedAtAction(nameof(Get), new { token = token }, user);
    }

    private string GenerateToken(Retailor user, string role)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
        var tokenDesription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id), new Claim("role", role) }),
            Expires = DateTime.Now.AddDays(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDesription);
        return tokenHandler.WriteToken(token);
    }



}