namespace ICMarketsTest.Core;

public interface ICommandExecutor<in TCommand>
{
    /// <summary>
    /// Executes command
    /// </summary>
    Task ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
}