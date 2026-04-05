using ICMarketsTest.Core;
using ICMarketsTest.Core.BlockchainSnapshots;
using Microsoft.EntityFrameworkCore;

namespace ICMarketsTest.Storage.BlockchainSnapshots;

/// <summary>
/// Repository for blockchain snapshots on top of the EF Core database context
/// </summary>
public sealed class EfCoreBlockchainSnapshotsRepository : IBlockchainSnapshotsRepository
{
    private readonly StorageDbContext _dbContext;
    private readonly IBlockchainSnapshotsMapper _mapper;

    public EfCoreBlockchainSnapshotsRepository(StorageDbContext dbContext, IBlockchainSnapshotsMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(Core.BlockchainSnapshots.BlockchainSnapshot blockchainSnapshot, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map(blockchainSnapshot);
        await _dbContext.BlockchainSnapshots.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Core.BlockchainSnapshots.BlockchainSnapshot?> GetAsync(Guid id, string networkId, CancellationToken cancellationToken = default)
    {
        var snapshot = await _dbContext.BlockchainSnapshots
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.Id == id && s.NetworkId == networkId, cancellationToken);

        return snapshot is null ? null : _mapper.Map(snapshot);
    }

    public async Task<PagedResult<Core.BlockchainSnapshots.BlockchainSnapshot>> GetPageAsync(string networkId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.BlockchainSnapshots
            .Where(s => s.NetworkId == networkId)
            .AsNoTracking();

        var totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Project()
            .ToListAsync(cancellationToken);

        var result = new PagedResult<Core.BlockchainSnapshots.BlockchainSnapshot>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };

        return result;
    }
}