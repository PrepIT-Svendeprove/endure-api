using Endure.Service.Models.Dto.ProductBatchDtos;
using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ProductBatch;

public class GetProductBatch
{
    [EndpointName("GetPaginatedProductsByProductId")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "No entities were found")]
    [ProducesResponseType<PaginatedResult<ProductBatchDto>>(StatusCodes.Status200OK, Description = "Found entities with the requested id.")]
    public static async Task<IResult> GetPaginatedProductsByProductIdAsync(
            [FromServices] IProductBatchService productBatchService,
            [AsParameters] ProductBatchFilter filter
        )
    {
        try
        {
            var result = await productBatchService.GetPaginatedProductsByProductId(filter);

            return Results.Ok(result);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetPaginatedProductsByWarehouseId")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "No entities were found")]
    [ProducesResponseType<PaginatedResult<ProductBatchDto>>(StatusCodes.Status200OK, Description = "Found entities with the requested id.")]
    public static async Task<IResult> GetPaginatedProductsByWarehouseIdAsync(
            [FromServices] IProductBatchService productBatchService,
            [AsParameters] ProductBatchFilter filter
        )
    {
        try
        {
            var result = await productBatchService.GetPaginatedProductsByWarehouseId(filter);
            
            return Results.Ok(result);
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetpaginatedProductsByStorageUnitId")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "No entities were found")]
    [ProducesResponseType<PaginatedResult<ProductBatchDto>>(StatusCodes.Status200OK, Description = "Found entities with the requested ids.")]
    public static async Task<IResult> GetPaginatedProductsByStorageUnitIdAsync(
            [FromServices] IProductBatchService productBatchService,
            [AsParameters] ProductBatchFilter filter
        )
    {
        try
        {
            var result = await productBatchService.GetPaginatedProductsByStorageUnitId(filter);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError();
        }
    }


    [EndpointName("GetProductBatchCountInWarehouse")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetProductBatchCountAsync(
            [FromServices] IProductBatchService productService,
            [FromRoute] string warehouseId
        )
    {
        try
        {
            if (!Guid.TryParse(warehouseId, out Guid parsedWarehouseId))
                return Results.UnprocessableEntity();

            return Results.Ok(await productService.GetProductBatchCountAsync(parsedWarehouseId));
        }
        catch (Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
