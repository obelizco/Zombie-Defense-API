using MediatR;
using ZombieDefense.Application.Defense.V1.DTOs;

namespace ZombieDefense.Application.Defense.V1.Queries
{
    public record struct OptimalStrategyQuery
    (
        int Bullets,
        int SecondsAvailable
    ) : IRequest<DefenseResultDto>;
}
