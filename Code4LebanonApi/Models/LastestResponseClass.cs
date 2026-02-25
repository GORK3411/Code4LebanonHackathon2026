using System.Text.Json.Serialization;

namespace Code4LebanonApi.Models
{
    public class LastestResponseClass
    {
        [JsonPropertyName("last_response_date")]
        public DateTime? LastResponseDate { get; set; }

        [JsonPropertyName("response_id")]
        public Guid? ResponseId { get; set; }

        [JsonPropertyName("survey_id")]
        public Guid? SurveyId { get; set; }
    }
}
