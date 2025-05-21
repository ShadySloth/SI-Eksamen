using Backend_Service.Domain.Entities;

namespace Backend_Service.Infrastructure.Interfaces;

public interface IDataRepository
{
    Task<DataSet> CreateDataSet(DataSet dataSet);
    Task<DataSet[]> GetDataSets();
}