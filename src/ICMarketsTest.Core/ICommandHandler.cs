namespace ICMarketsTest.Core;

public interface ICommandHandler<in TCommand>
{
    /// <summary>
    /// Handles command
    /// </summary>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}