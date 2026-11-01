namespace Starry.Infrastructure.Agent;

public sealed class DriftEventArgs : EventArgs
{
    public DriftEvent Drift { get; }

    public DriftEventArgs(DriftEvent drift)
    {
        ArgumentNullException.ThrowIfNull(drift);
        Drift = drift;
    }
}
