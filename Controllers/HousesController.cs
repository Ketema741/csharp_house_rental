using HouseStoreApi.Models;
using HouseStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace HouseStoreApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class HousesController : ControllerBase
{
    private readonly HousesService _housesService;
    //private readonly RetailorsService _retailorService;

    public HousesController(HousesService housesService)
    {
        _housesService = housesService;
        // _retailorService = retailorsService;

    }

    [HttpGet, AllowAnonymous]

    public async Task<List<House>> Get() =>
        await _housesService.GetAsync();

    // [HttpGet("retailors")]
    // public async Task<List<Retailor>> GetRetailor() =>
    //     await _retailorService.GetAsync();

    [HttpGet("{id:length(24)}"), AllowAnonymous]
    public async Task<ActionResult<House>> Get(string id)
    {
        var house = await _housesService.GetAsync(id);

        if (house is null)
        {
            return NotFound();
        }

        return house;
    }

    [Authorize(Roles = "Realtor")]
    [HttpPost]
    public async Task<IActionResult> Post(House newHouse)
    {
        if (newHouse == null)
        {
            return BadRequest("The newHouse object is null.");
        }

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        var idClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null)
        {
            return BadRequest("The realtor ID claim is missing or has a different name.");
        }
        
        var realtorId = idClaim.Value;

         if (realtorId == "")
        {
            return BadRequest("The realtor ID claim is missing or has a different name.");
        }
        Console.WriteLine(realtorId);
        newHouse.realtorId = realtorId;
        await _housesService.CreateAsync(newHouse);

        return CreatedAtAction(nameof(Get), new { id = newHouse.Id }, newHouse);
    }



    [Authorize(Roles = "Realtor")]
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, House updatedHouse)
    {

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        var idClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null)
        {
            return BadRequest("The realtor ID claim is missing or has a different name.");
        }
        
        var realtorId = idClaim.Value;

        var house = await _housesService.GetAsync(id);

        if ( house == null )
        {
            return NotFound();
        }
         if (realtorId == "" || house.realtorId != realtorId)
        {
            
            return BadRequest("House does not belongs to this user");
        }
        
        updatedHouse.Id = house.Id;

        await _housesService.UpdateAsync(id, updatedHouse);

        return NoContent();
    }

    [Authorize(Roles = "Realtor")]
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        var idClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null)
        {
            return BadRequest("The realtor ID claim is missing or has a different name.");
        }
        
        var realtorId = idClaim.Value;
        var house = await _housesService.GetAsync(id);

         if ( house == null )
        {
            return NotFound();
        }
         if (realtorId == "" || house.realtorId != realtorId)
        {
            
            return BadRequest("House does not belongs to this user");
        }

        await _housesService.RemoveAsync(id);

        return Ok(new { message = "House removed successfully" });
    }
}