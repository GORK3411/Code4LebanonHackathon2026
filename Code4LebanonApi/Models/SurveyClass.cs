using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

public class Survey
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    // We keep schema only as a container to reach sections
    [JsonPropertyName("schema")]
    [NotMapped]
    public SurveySchema? Schema { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("allow_multiple_submissions")]
    public bool AllowMultipleSubmissions { get; set; }

    [JsonPropertyName("requires_auth")]
    public bool RequiresAuth { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime? PublishedAt { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}

public class SurveySchema
{
    // ONLY sections extracted
    [JsonPropertyName("sections")]
    public List<SurveySection>? Sections { get; set; }
}

public class SurveySection
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}