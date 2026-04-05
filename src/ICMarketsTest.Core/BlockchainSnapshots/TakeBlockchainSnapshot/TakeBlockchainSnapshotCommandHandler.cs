namespace ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;

public sealed class TakeBlockchainSnapshotCommandHandler : ITakeBlockchainSnapshotCommandHandler
{
    private readonly IBlockchainSnapshotsProvider _provider;
    private readonly IBlockchainSnapshotsRepository _repository;

    public TakeBlockchainSnapshotCommandHandler(
        IBlockchainSnapshotsProvider provider,
        IBlockchainSnapshotsRepository repository)
    {
        _provider = provider;
        _repository = repository;
    }

    public async Task HandleAsync(TakeBlockchainSnapshotCommand command, CancellationToken cancellationToken = default)
    {
        var payload = await _provider.GetBlockchainSnapshotAsync(
            command.NetworkName,
            command.NetworkType,
            cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var id = Guid.CreateVersion7(now);
        var blockchainSnapshot = new BlockchainSnapshot
        {
            Id = id,
            CreatedAt = now,
            Payload = payload,
            NetworkId = command.NetworkId
        };

        await _repository.AddAsync(blockchainSnapshot, cancellationToken);
    }
}