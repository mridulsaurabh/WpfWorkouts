namespace Infrastructure.Utility
{
  public class SessionConstants
  {
      public const string LateReasonCodesLink = "https://wwtkulsdev.cloudapp.net/Api/MasterData/LateReasonCodes";
      public const string AdjustementReasonCodesLink = "https://wwtkulsdev.cloudapp.net/Api/MasterData/AdjustmentReasonCodes";
      public const string RejectionReasonCodesLink = "https://wwtkulsdev.cloudapp.net/Api/MasterData/RejectionReasonCodes";
      public const string LaborCategoriesLink = "https://wwtkulsdev.cloudapp.net/Api/MasterData/LaborCategories";
      public const string TimeZonesLink = "https://wwtkulsdev.cloudapp.net/Api/MasterData/Timezones";
      public const string ValidationRulesLink = "https://wwtkulsdev.cloudapp.net/Api/MasterData/ValidationRules";
      
      public const string IgnoreAuthorizationValue = "IgnoreAuthValue";
      public const string ApplicationMediaType = "application/json";
      public static string CacheSourcePath = "";

      public static string OfflineCachePath { get; set; }

      public static string CacheSourceTemplateFilePath { get; set; }
  }

}
