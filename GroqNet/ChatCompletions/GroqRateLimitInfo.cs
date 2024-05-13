using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace GroqNet.ChatCompletions;

/// <summary>
/// Inform on current rate limits applicable to the API key and associated organization.
/// When the rate limit is reached a 429 Too Many Requests HTTP status code is returned.
/// Note, retry-after is only set if you hit the rate limit and status code 429 is returned. 
/// The other headers are always included.
/// </summary>
public class GroqRateLimitInfo
{
    /// <summary>
    /// Time in seconds to wait before retrying the request.
    /// </summary>
    public int RetryAfter { get; private set; }

    /// <summary>
    /// Maximum number of allowed requests per day.
    /// </summary>
    public int LimitRequests { get; private set; }

    /// <summary>
    /// Maximum number of allowed tokens per minute.
    /// </summary>
    public int LimitTokens { get; private set; }

    /// <summary>
    /// Number of requests remaining today.
    /// </summary>
    public int RemainingRequests { get; private set; }

    /// <summary>
    /// Number of tokens remaining this minute.
    /// </summary>
    public int RemainingTokens { get; private set; }

    /// <summary>
    /// Time remaining until the Requests Per Day (RPD) rate limit resets.
    /// </summary>
    public TimeSpan ResetRequests { get; private set; }

    /// <summary>
    /// Time remaining until the Tokens Per Minute (TPM) limit resets.
    /// </summary>
    public TimeSpan ResetTokens { get; private set; }

    public static GroqRateLimitInfo FromHeaders(HttpResponseHeaders headers)
    {
        var info = new GroqRateLimitInfo();

        if (headers.TryGetValues("retry-after", out var retryAfter))
            info.RetryAfter = int.Parse(retryAfter.FirstOrDefault() ?? "0");

        if (headers.TryGetValues("x-ratelimit-limit-requests", out var limitRequests))
            info.LimitRequests = int.Parse(limitRequests.FirstOrDefault() ?? "0");

        if (headers.TryGetValues("x-ratelimit-limit-tokens", out var limitTokens))
            info.LimitTokens = int.Parse(limitTokens.FirstOrDefault() ?? "0");

        if (headers.TryGetValues("x-ratelimit-remaining-requests", out var remainingRequests))
            info.RemainingRequests = int.Parse(remainingRequests.FirstOrDefault() ?? "0");

        if (headers.TryGetValues("x-ratelimit-remaining-tokens", out var remainingTokens))
            info.RemainingTokens = int.Parse(remainingTokens.FirstOrDefault() ?? "0");

        if (headers.TryGetValues("x-ratelimit-reset-requests", out var requestReset))
            info.ResetRequests = ParseDuration(requestReset.FirstOrDefault() ?? "0");

        if (headers.TryGetValues("x-ratelimit-reset-tokens", out var tokenReset))
            info.ResetTokens = ParseDuration(tokenReset.FirstOrDefault() ?? "0");

        return info;
    }

    private static TimeSpan ParseDuration(string duration)
    {
        var pattern = new Regex(@"(?:(?<hours>\d+)h)?(?:(?<minutes>\d+)m)?(?:(?<seconds>\d+\.\d+)s)?");
        Match match = pattern.Match(duration);

        if (match.Success)
        {
            int hours = int.Parse(match.Groups["hours"].Value == "" ? "0" : match.Groups["hours"].Value);
            int minutes = int.Parse(match.Groups["minutes"].Value == "" ? "0" : match.Groups["minutes"].Value);
            double seconds = double.Parse(match.Groups["seconds"].Value == "" ? "0" : match.Groups["seconds"].Value, CultureInfo.InvariantCulture);

            return new TimeSpan(0, hours, minutes, (int)seconds, (int)((seconds - (int)seconds) * 1000));
        }
        else
        {
            throw new FormatException("Invalid duration format.");
        }
    }
}
