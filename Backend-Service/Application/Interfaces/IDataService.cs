using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;

namespace Backend_Service.Application.Interfaces;

public interface IDataService
{
    Task<DataSetDto> CreateTrainingSet(CreateDataSetDto dataSetDto);
    Task<DataSetDto[]> GetDataSets();
}