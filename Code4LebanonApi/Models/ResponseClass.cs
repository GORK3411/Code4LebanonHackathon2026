using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class SurveyResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("survey_id")]
    public string SurveyId { get; set; }

    [JsonPropertyName("respondent_email")]
    public string RespondentEmail { get; set; }

    [JsonPropertyName("respondent_phone")]
    public string RespondentPhone { get; set; }

    [JsonPropertyName("respondent_name")]
    public string RespondentName { get; set; }

    [JsonPropertyName("responses")]
    public Dictionary<string, object> Responses { get; set; }

    [JsonPropertyName("utm_source")]
    public string UtmSource { get; set; }

    [JsonPropertyName("utm_medium")]
    public string UtmMedium { get; set; }

    [JsonPropertyName("geo_country")]
    public string GeoCountry { get; set; }

    [JsonPropertyName("geo_region")]
    public string GeoRegion { get; set; }

    [JsonPropertyName("geo_city")]
    public string GeoCity { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}