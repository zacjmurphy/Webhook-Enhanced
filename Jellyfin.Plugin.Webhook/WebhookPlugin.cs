using System;
using System.Collections.Generic;
using Jellyfin.Plugin.Webhook.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Webhook;

/// <summary>
/// Plugin entrypoint.
/// </summary>
public class WebhookPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly Guid _id = new("e210f13a-88f4-49ba-912c-09122eb38e2f");

    /// <summary>
    /// Initializes a new instance of the <see cref="WebhookPlugin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
    public WebhookPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets current plugin instance.
    /// </summary>
    public static WebhookPlugin? Instance { get; private set; }

    /// <inheritdoc />
    public override Guid Id => _id;

    /// <inheritdoc />
    public override string Name => "Webhook Enhanced";

    /// <inheritdoc />
    public override string Description => "Sends notifications to services & platforms via webhooks.";

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        var prefix = GetType().Namespace;
        yield return new PluginPageInfo
        {
            Name = Name,
            EnableInMainMenu = true,
            EmbeddedResourcePath = prefix + ".Configuration.Web.config.html"
        };

        yield return new PluginPageInfo
        {
            Name = $"{Name}.js",
            EmbeddedResourcePath = prefix + ".Configuration.Web.config.js"
        };
    }
}