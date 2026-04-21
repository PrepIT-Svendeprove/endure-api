using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.StorageUnit;

public class DeleteStorageUnit
{
    [EndpointName("DeleteStorageUnit")]
    [EndpointSummary("""
            Deletes a storageunit, if the storage unit contains productbatches that are not deleted, they will be marked as deleted.
            If it contains sub- storageunits, you will not be able to delete the storageunit, before you have marked the sub- units as deleted.
        """)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status409Conflict, Description = "The storageunit contains sub- storageunits.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The storageunit were not deleted.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The storageunit were succesfully deleted.")]
    public static async Task<IResult> DeleteStorageUnitAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [FromServices] IAuditLogService auditLogService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse identifier to Guid.");

            if (await storageUnitService.HasSubStorageUnitsAsync(parsedId))
                return Results.Conflict();

            var result = await storageUnitService.SoftDeleteEntity(parsedId);

            if (result is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest();
        }
        catch(Exception ex)
        {
            await auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                LogData = ex.Message,
                LogLevel = Service.Models.Enums.LogLevel.Error,
            });

            return Results.InternalServerError();
        }
    }
}
