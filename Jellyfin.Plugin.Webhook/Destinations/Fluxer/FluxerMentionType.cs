namespace Jellyfin.Plugin.Webhook.Destinations.Fluxer;

/// <summary>
/// Fluxer mention type.
/// </summary>
public enum FluxerMentionType
{
    /// <summary>
    /// Mention @everyone.
    /// </summary>
    Everyone = 2,

    /// <summary>
    /// Mention @here.
    /// </summary>
    Here = 1,

    /// <summary>
    /// Mention none.
    /// </summary>
    None = 0
}
