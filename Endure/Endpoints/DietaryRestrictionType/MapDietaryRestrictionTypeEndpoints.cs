namespace Endure.Endpoints.DietaryRestrictionType;

public static class MapDietaryRestrictionTypeEndpoints
{
    public static RouteGroupBuilder MapDietaryRestrictionsTypeApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/dietaryrestrictiontype");
        group.WithTags("DietaryRestrictionType");

        group.MapGet("/", GetDietaryRestrictionType.GetDietaryRestrictionTypesAsync);

        group.MapPost("/", PostDietaryRestrictionType.CreateDietaryRestrictionsTypeAsync);

        group.MapDelete("/{id}", DeleteDietaryRestrictionType.DeleteDietaryRestrictionTypesAsync);

        return route;
    }
}
