using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Dto.StorageUnitDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;
using LogLevel = Endure.Service.Models.Enums.LogLevel;

namespace Endure.Endpoints.StorageUnit;

public class PostStorageUnit
{
    [EndpointName("CreateStorageUnit")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> CreateStorageUnitAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [FromServices] IAuditLogService auditLogService,
            [FromBody] CreateStorageUnitDto storageUnit
        )
    {
        try
        {
            var result = await storageUnitService.CreateStorageUnitAsync(storageUnit);

            if (result is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest();
        }
        catch(Exception e)
        {
            await auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                LogData = e.Message,
                LogLevel = LogLevel.Error,
                ModuleType = ModuleType.Inventory
            });

            return Results.InternalServerError();
        }
    }
}
