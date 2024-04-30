using BeniceSoft.OpenAuthing.Entities.Positions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Repositories;

public class PositionRepository : EfCoreRepository<AuthingDbContext, Position, Guid>, IPositionRepository
{
    public PositionRepository(IDbContextProvider<AuthingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}