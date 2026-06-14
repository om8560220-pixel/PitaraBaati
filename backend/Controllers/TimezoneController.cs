using Microsoft.AspNetCore.Mvc;

namespace PitaraBaati.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimezoneController : ControllerBase
    {
        private readonly ILogger<TimezoneController> _logger;

        public TimezoneController(ILogger<TimezoneController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all supported timezones
        /// </summary>
        [HttpGet]
        public IActionResult GetAllTimezones()
        {
            try
            {
                var timezones = TimeZoneInfo.GetSystemTimeZones()
                    .Select(tz => new
                    {
                        id = tz.Id,
                        displayName = tz.DisplayName,
                        standardName = tz.StandardName,
                        baseUtcOffset = tz.BaseUtcOffset.TotalHours
                    })
                    .OrderBy(tz => tz.baseUtcOffset)
                    .ToList();

                return Ok(timezones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving timezones");
                return StatusCode(500, new { message = "Error retrieving timezones" });
            }
        }

        /// <summary>
        /// Get current time in a specific timezone
        /// </summary>
        /// <param name="timezoneId">The timezone ID (e.g., "America/New_York")</param>
        [HttpGet("{timezoneId}")]
        public IActionResult GetTimeInTimezone(string timezoneId)
        {
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                var utcNow = DateTime.UtcNow;
                var timeInTimezone = TimeZoneInfo.ConvertTime(utcNow, TimeZoneInfo.Utc, timezone);

                return Ok(new
                {
                    timezoneId = timezone.Id,
                    displayName = timezone.DisplayName,
                    currentTime = timeInTimezone,
                    utcTime = utcNow,
                    offset = timezone.GetUtcOffset(timeInTimezone),
                    isDaylightSavingTime = timezone.IsDaylightSavingTime(timeInTimezone)
                });
            }
            catch (TimeZoneNotFoundException)
            {
                _logger.LogWarning($"Timezone not found: {timezoneId}");
                return NotFound(new { message = $"Timezone '{timezoneId}' not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving time for timezone: {timezoneId}");
                return StatusCode(500, new { message = "Error retrieving timezone data" });
            }
        }

        /// <summary>
        /// Get current time in multiple timezones
        /// </summary>
        [HttpPost("multiple")]
        public IActionResult GetTimeInMultipleTimezones([FromBody] List<string> timezoneIds)
        {
            try
            {
                if (timezoneIds == null || timezoneIds.Count == 0)
                {
                    return BadRequest(new { message = "At least one timezone ID is required" });
                }

                var utcNow = DateTime.UtcNow;
                var results = new List<object>();

                foreach (var timezoneId in timezoneIds)
                {
                    try
                    {
                        var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                        var timeInTimezone = TimeZoneInfo.ConvertTime(utcNow, TimeZoneInfo.Utc, timezone);

                        results.Add(new
                        {
                            timezoneId = timezone.Id,
                            displayName = timezone.DisplayName,
                            currentTime = timeInTimezone,
                            offset = timezone.GetUtcOffset(timeInTimezone),
                            isDaylightSavingTime = timezone.IsDaylightSavingTime(timeInTimezone)
                        });
                    }
                    catch (TimeZoneNotFoundException)
                    {
                        results.Add(new
                        {
                            timezoneId = timezoneId,
                            error = $"Timezone '{timezoneId}' not found"
                        });
                    }
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving times for multiple timezones");
                return StatusCode(500, new { message = "Error retrieving timezone data" });
            }
        }
    }
}
