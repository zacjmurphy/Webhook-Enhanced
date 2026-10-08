namespace Jellyfin.Plugin.Webhook.Destinations.Fluxer;

/// <summary>
/// Fluxer specific options.
/// </summary>
public class FluxerOption : BaseOption
{
    /// <summary>
    /// Gets or sets the embed color.
    /// </summary>
    public string? EmbedColor { get; set; }

    /// <summary>
    /// Gets or sets the avatar url.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Gets or sets the bot username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the mention type.
    /// </summary>
    public FluxerMentionType MentionType { get; set; }
}
