using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Service.Presentation;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly IDataService _dataService;
    
    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpPost]
    public async Task<ActionResult<DataSetDto>> CreateTrainingSet([FromBody]CreateDataSetDto dataSetDto)
    {
        var result = await _dataService.CreateTrainingSet(dataSetDto);

        return Ok(result);
    }
}