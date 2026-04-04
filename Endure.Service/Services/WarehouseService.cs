using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Dto.Warehouse;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Endure.Service.Services;

internal class WarehouseService(DatabaseContext context) : IWarehouseService
{
    private readonly DatabaseContext _context = context;    

    public async Task<WarehouseDto?> GetCurrentRootWarehouseAsync()
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
                .MapToWarehouseDto()
                .ToListAsync();
    } 

    public async Task<List<WarehouseDto>> GetAllByIdAsync(Guid id)
    {
        return await _context
                .Warehouse
                .Where(x => !x.IsRoot && !x.IsDeleted && x.Id == id)
                .MapToWarehouseDto()
                .ToListAsync();
    }

    public async Task<List<WarehouseDto>> GetAllByParentIdAsync(Guid id)
    {
        return await _context
                    .Warehouse
                    .Where(x => !x.IsRoot && !x.IsDeleted && x.ParentWarehouseId == id)
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

        if (entity.IsRoot)
            await RemoveRootEntity(mappedEntity.Id);

        return await _context.SaveChangesAsync() > 0 ? mappedEntity.MapToWarehouseDto() : null;
    }

    public async Task<bool> UpdateWarehouseAsync(UpdateWarehouseDto entity)
    {
        var mappedEntity = entity.MapToWarehouse();
        _context.Update(mappedEntity);

        if (entity.IsRoot)
            await RemoveRootEntity(mappedEntity.Id);

        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Soft deletes a warehouse.
    /// </summary>
    public async Task<bool> DeleteWarehouseAsync(Guid entityId)
    {
        return await _context.Warehouse
            .Where(x => x.Id == entityId && !x.IsDeleted)
            .ExecuteUpdateAsync(
                x => x.SetProperty(y => y.IsDeleted, true)
            ) > 0;
    }

    /// <summary>
    /// Sets IsRoot to false, of the current root warehouse.
    /// </summary>
    private async Task RemoveRootEntity(Guid id, Warehouse? entity = null)
    {
        var currentRootWarehouse = entity is null ? await _context
                                            .Warehouse
                                            .FirstOrDefaultAsync(x => x.IsRoot && x.Id != id) : entity;

        if (currentRootWarehouse is null)
            return;

        currentRootWarehouse.IsRoot = false;
        _context.Update(currentRootWarehouse);
    }
}

/// <summary>
/// This service includes CRUD functionality for the Warehouse entity.
/// </summary>
public interface IWarehouseService
{
    /// <summary>
    /// Creates a new warehouse, if <see cref="CreateWarehouseDto.IsRoot" /> is set on the <paramref name="entity"/> it will set the current root warehouse and set the newly created as the root warehouse.
    /// </summary>
    /// <returns>The mapped entity of CreateWarehouseDto.</returns>
    Task<WarehouseDto?> CreateWarehouseAsync(CreateWarehouseDto entity);
    Task<bool> DeleteWarehouseAsync(Guid entityId);

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
    Task<WarehouseDto?> GetCurrentRootWarehouseAsync();

    /// <summary>
    /// Updates an existing warehouse.
    /// </summary>
    Task<bool> UpdateWarehouseAsync(UpdateWarehouseDto entity);
}
