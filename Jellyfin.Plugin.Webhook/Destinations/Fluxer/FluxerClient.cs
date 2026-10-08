using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Webhook.Destinations.Fluxer;

/// <summary>
/// Client for the <see cref="FluxerOption"/>.
/// </summary>
public class FluxerClient : BaseClient, IWebhookClient<FluxerOption>
{
    private readonly ILogger<FluxerClient> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluxerClient"/> class.
    /// </summary>
    /// <param name="logger">Instance of the <see cref="ILogger{FluxerDestination}"/> interface.</param>
    /// <param name="httpClientFactory">Instance of the<see cref="IHttpClientFactory"/> interface.</param>
    public FluxerClient(ILogger<FluxerClient> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public async Task SendAsync(FluxerOption option, Dictionary<string, object> data)
    {
        try
        {
            if (string.IsNullOrEmpty(option.WebhookUri))
            {
                throw new ArgumentException(nameof(option.WebhookUri));
            }

            if (!SendWebhook(_logger, option, data))
            {
                return;
            }

            // Add Fluxer specific properties.
            data["MentionType"] = GetMentionType(option.MentionType);
            if (!string.IsNullOrEmpty(option.EmbedColor))
            {
                data["EmbedColor"] = FormatColorCode(option.EmbedColor);
            }

            if (!string.IsNullOrEmpty(option.AvatarUrl))
            {
                data["AvatarUrl"] = option.AvatarUrl;
            }

            if (!string.IsNullOrEmpty(option.Username))
            {
                data["Username"] = option.Username;
                data["BotUsername"] = option.Username;
            }

            var body = option.GetMessageBody(data);
            if (!SendMessageBody(_logger, option, body))
            {
                return;
            }

            _logger.LogDebug("SendAsync Body: {@Body}", body);
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(option.WebhookUri))
            {
                Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            await SendAsync(_httpClientFactory, request, _logger).ConfigureAwait(false);
        }
        catch (HttpRequestException e)
        {
            _logger.LogWarning(e, "Error sending notification");
        }
    }

    private static int FormatColorCode(string hexCode)
    {
        return int.Parse(hexCode[1..6], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    private static string GetMentionType(FluxerMentionType mentionType)
    {
        return mentionType switch
        {
            FluxerMentionType.Everyone => "@everyone",
            FluxerMentionType.Here => "@here",
            _ => string.Empty
        };
    }
}
