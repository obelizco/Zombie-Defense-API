using Carter;
using MediatR;
using ZombieDefense.Application.Defense.V1.Queries;
using ZombieDefense.Application.Defense.V1.Queries.Handlers;

namespace ZombieDefense.API.Features.Defense.Endpoints
{
    public class OptimalStrategyEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet($"defense/optimal-strategy",
                async (int bullets,int secondsAvailable, ISender sender) =>
                {
                    return Results.Ok(await sender.Send(new OptimalStrategyQuery(bullets, secondsAvailable)) );
                });
        }
    }
}
