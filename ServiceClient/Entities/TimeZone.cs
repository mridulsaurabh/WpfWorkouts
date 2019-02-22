namespace ServiceClient.Entities
{
  public class TimeZone
  {
    public string UTCOffset { get; set; }

    public string Id { get; set; }

    public string DisplayName { get; set; }

    public string DaylightName { get; set; }

    public string StandardName { get; set; }

    public bool SupportsDaylightSavingTime { get; set; }
  }
}
