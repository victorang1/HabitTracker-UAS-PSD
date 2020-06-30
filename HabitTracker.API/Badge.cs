using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace HabitTracker.Api
{
  public class Badge
  {
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("name")]
    public String Name { get; set; }

    [JsonPropertyName("description")]
    public String Description { get; set; }

    [JsonPropertyName("user_id")]
    public Guid UserID { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
  }
}
