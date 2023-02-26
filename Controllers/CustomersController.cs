using HouseStoreApi.Models;
using HouseStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HouseStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    //private readonly HousesService _housesService;
    private readonly CustomersService _customersService;

    public CustomerController(CustomersService customersService){
        
        _customersService = customersService;

    }
        
    [HttpGet]
    public async Task<List<Customer>> Get() =>
        await _customersService.GetAsync();
    
   
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Customer>> Get(string id)
    {
        var customer = await _customersService.GetAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        return customer;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Customer newRetailor)
    {
        await _customersService.CreateAsync(newRetailor);

        return CreatedAtAction(nameof(Get), new { id = newRetailor.Id }, newRetailor);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Customer updatedRetailor)
    {
        var retailor = await _customersService.GetAsync(id);

        if (retailor is null)
        {
            return NotFound();
        }

        updatedRetailor.Id = retailor.Id;

        await _customersService.UpdateAsync(id, updatedRetailor);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var retailor = await _customersService.GetAsync(id);

        if (retailor is null)
        {
            return NotFound();
        }

        await _customersService.RemoveAsync(id);

        return NoContent();
    }
}