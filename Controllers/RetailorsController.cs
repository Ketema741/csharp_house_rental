using HouseStoreApi.Models;
using HouseStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HouseStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RetailorsController : ControllerBase
{
    //private readonly HousesService _housesService;
    private readonly RetailorsService _retailorsService;

    public RetailorsController(RetailorsService retailorsService){
        
        _retailorsService = retailorsService;

    }
        
    [HttpGet]
    public async Task<List<Retailor>> Get() =>
        await _retailorsService.GetAsync();
    
   
    [HttpGet("{id:length(24)}")]
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
        var retailor = await _retailorsService.GetAsync(id);

        if (retailor is null)
        {
            return NotFound();
        }

        updatedRetailor.Id = retailor.Id;

        await _retailorsService.UpdateAsync(id, updatedRetailor);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var retailor = await _retailorsService.GetAsync(id);

        if (retailor is null)
        {
            return NotFound();
        }

        await _retailorsService.RemoveAsync(id);

        return NoContent();
    }
}