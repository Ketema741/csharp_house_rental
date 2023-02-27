using HouseStoreApi.Models;
using HouseStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace HouseStoreApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

[ApiController]
[Route("api/[controller]")]
public class RetailorsController : ControllerBase
{
    //private readonly HousesService _housesService;
    private readonly RetailorsService _retailorsService;

    public RetailorsController(RetailorsService retailorsService)
    {

        _retailorsService = retailorsService;

    }

    [HttpGet, AllowAnonymous]
    public async Task<List<Retailor>> Get() =>
        await _retailorsService.GetAsync();


    [HttpGet("{id:length(24)}"), AllowAnonymous]
    public async Task<ActionResult<Retailor>> Get(string id)
    {
        var retailor = await _retailorsService.GetAsync(id);

        if (retailor is null)
        {
            return NotFound();
        }

        return retailor;
    }



    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Retailor updatedRetailor)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            return BadRequest("log in first");
        }
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        var idClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null)
        {
            return BadRequest("The realtor ID claim is missing or has a different name.");
        }

        var realtorId = idClaim.Value;

        if (realtorId == "" || realtorId != id)
        {
            return BadRequest("The realtor ID claim is missing or you are not allowed to access this!!!");
        }
        var retailor = await _retailorsService.GetAsync(id);

        if (retailor is null)
        {
            return NotFound();
        }

        updatedRetailor.Id = retailor.Id;

        await _retailorsService.UpdateAsync(id, updatedRetailor);

        var updated = await _retailorsService.GetAsync(id);
        return Ok(updated);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            return BadRequest("log in first");
        }
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        var idClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null)
        {
            return BadRequest("The realtor ID claim is missing or has a different name.");
        }

        var realtorId = idClaim.Value;

        if (realtorId == "" && realtorId != id)
        {
            return BadRequest("The realtor ID claim is missing or you are not allowed to access this!!!");
        }
        var retailor = await _retailorsService.GetAsync(id);

        if (retailor == null || realtorId != id)
        {
            return BadRequest("The realtor does not exist or you are not allowed to access this!!!");

        }

        await _retailorsService.RemoveAsync(id);

        return NoContent();
    }
}