using System.Runtime.CompilerServices;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.Storage.BlockchainSnapshots;

public interface IBlockchainSnapshotsMapper
{
    BlockchainSnapshot Map(ICMarketsTest.Core.BlockchainSnapshots.BlockchainSnapshot snapshot);
    ICMarketsTest.Core.BlockchainSnapshots.BlockchainSnapshot Map(BlockchainSnapshot snapshot);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class BlockchainSnapshotsMapper : IBlockchainSnapshotsMapper
{
    public partial BlockchainSnapshot Map(Core.BlockchainSnapshots.BlockchainSnapshot snapshot);

    public partial Core.BlockchainSnapshots.BlockchainSnapshot Map(BlockchainSnapshot snapshot);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DateTime MapDateTimeOffset(DateTimeOffset dateTime) => dateTime.UtcDateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DateTimeOffset MapDateTime(DateTime dateTime) => new(dateTime, TimeSpan.Zero);
}

/// <summary>
/// Mapperly also generates projections for IQueryable and EF Core:
/// https://mapperly.riok.app/docs/configuration/queryable-projections/
/// </summary>
[Mapper]
public static partial class BlockchainSnapshotsProjector
{
    public static partial IQueryable<Core.BlockchainSnapshots.BlockchainSnapshot> Project(this IQueryable<BlockchainSnapshot> query);
}