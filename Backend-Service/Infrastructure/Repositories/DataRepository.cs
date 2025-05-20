using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Contexts;
using Backend_Service.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Service.Infrastructure.Repositories;

public class DataRepository : IDataRepository
{
    private readonly ImageContext _context;

    public DataRepository(ImageContext context)
    {
        _context = context;
    }
    
    public async Task<DataSet> CreateDataSet(DataSet dataSet)
    {
        await _context.DataSets.AddAsync(dataSet);
        await _context.SaveChangesAsync();
        return (await _context.DataSets
            .Where(d => d.DataSetName == dataSet.DataSetName)
            .FirstOrDefaultAsync())!;
    }
}