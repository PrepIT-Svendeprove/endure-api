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
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity could not be created, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The entity has succesfully been created.")]
    public static async Task<IResult> CreateStorageUnitAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [FromServices] IAuditLogService auditLogService,
            [FromBody] CreateStorageUnitDto storageUnit
        )
    {
        try
        {
            var result = await storageUnitService.CreateStorageUnitAsync(storageUnit);

            if (result is not { ServiceResult: ServiceResult.Success })
                return Results.BadRequest(result.StatusCodes);

            return Results.NoContent();
        }
        catch(Exception e)
        {
            await auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                LogData = e.Message,
                LogLevel = LogLevel.Error,
            });

            return Results.InternalServerError();
        }
    }
}
