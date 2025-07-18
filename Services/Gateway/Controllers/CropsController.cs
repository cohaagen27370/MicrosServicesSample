using CommonDto.Models;
using Gateway.Repositories;
using Microsoft.AspNetCore.Mvc;
using Gateway.Extensions;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class CropsController(ICropRepository cropRepository) : ControllerBase
{
    
    [HttpPost("")]
    public async Task<IActionResult> AddCrop([FromBody] CropDto crop)
    {
        return Ok(await cropRepository.AddCrop(crop.ToCrop()));
    }
    
    [HttpPut("{cropId:guid}")]
    public async Task<IActionResult> UpdateCrop([FromRoute] Guid cropId, [FromBody] CropDto crop)
    {
        return Ok(await cropRepository.UpdateCrop(cropId, crop.ToCrop()));            
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllCrops()
    {
        return Ok((await cropRepository.GetAllCrops()).Select(x => x.ToCropDto()));   
    }
}