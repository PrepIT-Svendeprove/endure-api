namespace Endure.Endpoints.DietaryRestriction;

public static class MapDietaryRestrictionEndpoints
{
    public static RouteGroupBuilder MapDietaryRestrictionsApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/dietaryrestriction");
        group.WithTags("DietaryRestriction");

        group.MapGet("/{cpr}", GetDietaryRestriction.GetDietaryRestrictionByCprAsync);

        group.MapPost("/", PostDietaryRestriction.CreateDietaryRestrictionAsync);

        group.MapDelete("/{id}", DeleteDietaryRestriction.DeleteDietaryRestrictionAsync);

        return route;
    }
}
