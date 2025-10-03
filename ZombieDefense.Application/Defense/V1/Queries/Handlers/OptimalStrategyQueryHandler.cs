using MediatR;
using ZombieDefense.Application.Defense.V1.DTOs;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Infraestructure.Repositories.RepositoryBase;

namespace ZombieDefense.Application.Defense.V1.Queries.Handlers;

public class OptimalStrategyQueryHandler(IZombieDefenseRepository<Zombie> _zombieRepository,
                                         IZombieDefenseRepository<Simulation> _simulationRepository,
                                         IZombieDefenseRepository<Eliminated> _eliminatedRepository) : IRequestHandler<OptimalStrategyQuery, DefenseResultDto>
{
    public async Task<DefenseResultDto> Handle(OptimalStrategyQuery request, CancellationToken cancellationToken)
    {
        int bullets = request.Bullets;
        int seconds = request.SecondsAvailable;
        var zombies = await _zombieRepository.ListAsync(cancellationToken);
        var result = CalculateOptimalStrategy(zombies.ToList(), request.Bullets, request.SecondsAvailable);
        var simulation = new Simulation
        {
            TimeAvailableSeconds = request.SecondsAvailable,
            BulletsAvailable = request.Bullets,
            SimulationDate = DateTime.UtcNow,
            CreatedBy = "System",
        };
        await _simulationRepository.AddAsync(simulation);

        foreach (var zombie in result.ZombiesEliminated)
        {
            var zombieENtity = await _zombieRepository.GetByIdAsync(zombie.Id);

            var eliminated = new Eliminated
            {
                ZombieId = zombieENtity.Id,
                SimulationId = simulation.Id,
                PointsEarned = zombie.Score,
                CreatedBy = "System"
            };

            await _eliminatedRepository.AddAsync(eliminated);
        }

        return result;
    }


    private DefenseResultDto CalculateOptimalStrategy(List<Zombie> zombies, int bullets, int seconds)
    {
        var memo = new Dictionary<(int, int, int), DefenseResultDto>();

        DefenseResultDto Solve(int index, int remainingBullets, int remainingSeconds)
        {
            if (index >= zombies.Count || remainingBullets <= 0 || remainingSeconds <= 0)
                return new DefenseResultDto();

            if (memo.TryGetValue((index, remainingBullets, remainingSeconds), out var cached))
                return cached;

            var zombie = zombies[index];

            var exclude = Solve(index + 1, remainingBullets, remainingSeconds);

            DefenseResultDto include = new();
            if (remainingBullets >= zombie.BulletsRequired && remainingSeconds >= zombie.ShotTimeSeconds)
            {
                var next = Solve(index + 1, remainingBullets - zombie.BulletsRequired, remainingSeconds - zombie.ShotTimeSeconds);

                include = new DefenseResultDto
                {
                    TotalScore = next.TotalScore + zombie.Score,
                    BulletsUsed = next.BulletsUsed + zombie.BulletsRequired,
                    TimeUsed = next.TimeUsed + zombie.ShotTimeSeconds,
                    ZombiesEliminated = new List<Zombie>(next.ZombiesEliminated) { zombie }
                };
            }

            var best = (include.TotalScore > exclude.TotalScore) ? include : exclude;

            best.ZombiesEliminated = best.ZombiesEliminated
                .OrderByDescending(z => z.ThreatLevel)
                .ToList();

            memo[(index, remainingBullets, remainingSeconds)] = best;
            return best;
        }

        return Solve(0, bullets, seconds);
    }
}
