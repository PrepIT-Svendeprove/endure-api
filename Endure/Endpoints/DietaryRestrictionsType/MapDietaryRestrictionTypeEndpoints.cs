namespace Endure.Endpoints.DietaryRestrictionsType;

public static class MapDietaryRestrictionTypeEndpoints
{
    public static RouteGroupBuilder MapDietaryRestrictionsTypeApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/dietaryrestrictionstype");
        group.WithTags("DietaryRestrictionsType");

        group.MapGet("/", GetDietaryRestrictionType.GetDietaryRestrictionTypesAsync);

        group.MapPost("/", PostDietaryRestrictionType.CreateDietaryRestrictionsTypeAsync);

        group.MapDelete("/", DeleteDietaryRestrictionType.DeleteDietaryRestrictionTypesAsync);

        return route;
    }
}
