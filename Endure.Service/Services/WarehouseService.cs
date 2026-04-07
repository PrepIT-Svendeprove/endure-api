using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.WarehouseDtos;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class WarehouseService(DatabaseContext context)
    : BaseService<Warehouse>(context), IWarehouseService
{
    public async Task<Guid> GetRootWarehouseIdAsync()
    {
        return await _context
                .Warehouse
                .Where(x => x.IsRoot)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
    }

    public async Task<WarehouseDto?> GetRootWarehouseAsync()
    {
        return await _context
                .Warehouse
                .Where(x => !x.IsDeleted)
                .MapToWarehouseDto()
                .FirstOrDefaultAsync(x => x.IsRoot);
    }

    public async Task<List<WarehouseDto>> GetAllSubWarehousesAsync()
    {
        return await _context
                .Warehouse
                .Where(x => !x.IsRoot && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .MapToWarehouseDto()
                .ToListAsync();
    }

    public async Task<List<WarehouseDto>> GetAllByIdAsync(Guid id)
    {
        return await _context
                .Warehouse
                .Where(x => !x.IsRoot && !x.IsDeleted && x.Id == id)
                .OrderByDescending(x => x.CreatedAt)
                .MapToWarehouseDto()
                .ToListAsync();
    }

    public async Task<List<WarehouseDto>> GetAllByParentIdAsync(Guid id)
    {
        return await _context
                    .Warehouse
                    .Where(x => !x.IsRoot && !x.IsDeleted && x.ParentWarehouseId == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .MapToWarehouseDto()
                    .ToListAsync();
    }

    public async Task<WarehouseDto?> CreateWarehouseAsync(CreateWarehouseDto entity)
    {
        var mappedEntity = entity.MapToWarehouse();
        Warehouse? rootWarehouse;

        if (entity.ParentWarehouseId is null)
        {
            rootWarehouse = await _context.Warehouse.FirstOrDefaultAsync(x => x.Id == mappedEntity.Id && !x.IsDeleted);

            mappedEntity.ParentWarehouseId = rootWarehouse?.ParentWarehouseId;
        }

        await _context.AddAsync(mappedEntity);


        return await _context.SaveChangesAsync() > 0 ? mappedEntity.MapToWarehouseDto() : null;
    }

    public async Task<bool> UpdateWarehouseAsync(UpdateWarehouseDto entity)
    {
        var rootId = await GetRootWarehouseIdAsync();

        if (rootId != entity.Id)
            return false;

        var mappedEntity = entity.MapToWarehouse();
        _context.Update(mappedEntity);

        return await _context.SaveChangesAsync() > 0;
    }
}

/// <summary>
/// This service includes CRUD functionality for the Warehouse entity.
/// </summary>
public interface IWarehouseService : IBaseService
{
    /// <summary>
    /// Creates a new warehouse, if <see cref="CreateWarehouseDto.IsRoot" /> is set on the <paramref name="entity"/> it will set the current root warehouse and set the newly created as the root warehouse.
    /// </summary>
    /// <returns>The mapped entity of CreateWarehouseDto.</returns>
    Task<WarehouseDto?> CreateWarehouseAsync(CreateWarehouseDto entity);

    /// <summary>
    /// Retrievs all the warehouses that are not marked as the root warehouse.
    /// </summary>
    /// <returns></returns>
    Task<List<WarehouseDto>> GetAllSubWarehousesAsync();
    Task<List<WarehouseDto>> GetAllByIdAsync(Guid id);
    Task<List<WarehouseDto>> GetAllByParentIdAsync(Guid id);

    /// <summary>
    /// Retrieves the current warehouse that is marked as the root warehouse.
    /// </summary>
    Task<WarehouseDto?> GetRootWarehouseAsync();

    /// <summary>
    /// Updates an existing warehouse.
    /// </summary>
    Task<bool> UpdateWarehouseAsync(UpdateWarehouseDto entity);
    Task<Guid> GetRootWarehouseIdAsync();
}
