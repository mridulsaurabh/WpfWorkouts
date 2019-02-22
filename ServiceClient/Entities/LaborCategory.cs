using System;

namespace ServiceClient.Entities
{
  public class LaborCategory
  {
    public string UtilizationEligibleIndicator { get; set; }

    public string CustomerFacingIndicator { get; set; }

    public string BillableIndicator { get; set; }

    public string CategoryGroup { get; set; }

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
