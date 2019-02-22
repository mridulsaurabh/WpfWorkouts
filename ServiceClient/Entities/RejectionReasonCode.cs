using System;

namespace ServiceClient.Entities
{
  public class RejectionReasonCode
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime EffectiveStartDate { get; set; }

    public DateTime EffectiveEndDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }
  }
}
