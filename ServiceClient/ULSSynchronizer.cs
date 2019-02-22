using Infrastructure.Utility;
using Newtonsoft.Json;
using ServiceClient.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace ServiceClient
{
    public class ULSSynchronizer
    {
        private bool _isLateReasonCodesSync = false;
        private bool _isAdjustmentReasonCodesSync = false;
        private bool _isRejectionReasonCodesSync = false;
        private bool _isLaborCategoriresSync = false;
        private bool _isTimeZoneSync = false;
        private bool _isValidationRulesSync = false;
        private bool _isNewFile = false;
        private IServiceProvider _settingsProvider;

        public ULSSynchronizer()
        {
            this.GetOrUpdateULSOfflineCacheFile();
            this.ULSConnectionString = string.Empty;            
        }

        public bool IsSyncRequired
        {
            get
            {
                return this.SetSynchronizerStatus();
            }
        }

        public DateTime LastSynchronized { get; private set; }

        public string ULSConnectionString { get; set; }

        public async void Synchronize()
        {
            try
            {
                await this.SyncLateReasonCodes();
                await this.SyncAdjustmentReasonCodes();
                await this.SyncRejectionReasonCodes();
                await this.SyncLaborCategories();
                await this.SyncTimeZones();
                await this.SyncValidationRules();
                this.SetFlags(true);

                LastSynchronized = DateTime.Now; // update the last synchronized property and save it to settings.
                // _settings.SaveSettings<string>("", this.LastSynchronized);
                this._isNewFile = false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SetSynchronizerStatus()
        {
            bool isSyncRequired = true;
            try
            {
                TimeSpan span = DateTime.Now.Subtract(this.LastSynchronized);
                if (span.Hours > 24 || _isNewFile)
                {
                    // cache file was updated 24 hours ago, hence forth need to update it again.
                    isSyncRequired = true;
                    this.SetFlags(false);
                    this.Synchronize(); // updates all the tables for ULS data.
                }
                else
                {
                    // set all the local flags to true to prevent synchronization to get individual ULS table data. 
                    this.SetFlags(true);
                }

                // check individual local flags to make sure all the the tables in offline cache file have sync with ULS data.          
                if (this._isLateReasonCodesSync && this._isAdjustmentReasonCodesSync
                    && this._isRejectionReasonCodesSync && this._isTimeZoneSync)
                {
                    isSyncRequired = false;
                }
            }
            catch
            {

            }
            return isSyncRequired;
        }

        private void SetFlags(bool value)
        {
            this._isLateReasonCodesSync = value;
            this._isAdjustmentReasonCodesSync = value;
            this._isRejectionReasonCodesSync = value;
            this._isLaborCategoriresSync = value;
            this._isTimeZoneSync = value;
            this._isValidationRulesSync = value;
        }

        private void GetOrUpdateULSOfflineCacheFile()
        {
            if (!Directory.Exists(Path.GetDirectoryName(SessionConstants.CacheSourcePath)))
            {
                Directory.CreateDirectory(SessionConstants.CacheSourcePath);
            }

            var newFileName = Path.Combine(SessionConstants.CacheSourcePath, SessionConstants.OfflineCachePath);

            if (!File.Exists(newFileName))
            {
                _isNewFile = true;
                File.Copy(SessionConstants.CacheSourceTemplateFilePath, newFileName);
            }
        }

        private void CreateMapping()
        {
            // need to verify the logic for effective end date for mapping SQL and .NET date time object. ( default value == ? ) 
            //AutoMapper.Mapper.CreateMap<LateReasonCode, MasterDataCache_LateReasonCode>()
            //    .IgnoreAllNonExisting()
            //    .ForMember(d => d.EffectiveEndDate, opt => opt.ResolveUsing(arg => { if (arg.EffectiveEndDate <= DateTime.MinValue) { return DateTime.Today; } return arg.EffectiveEndDate; }));

            //AutoMapper.Mapper.CreateMap<AdjustmentReasonCode, MasterDataCache_AdjustmentReasonCode>()
            // .IgnoreAllNonExisting()
            // .ForMember(d => d.EffectiveEndDate, opt => opt.ResolveUsing(arg => { if (arg.EffectiveEndDate <= DateTime.MinValue) { return DateTime.Today; } return arg.EffectiveEndDate; }));

            //AutoMapper.Mapper.CreateMap<RejectionReasonCode, MasterDataCache_RejectionReasonCode>()
            //    .IgnoreAllNonExisting()
            //    .ForMember(d => d.EffectiveEndDate, opt => opt.ResolveUsing(arg => { if (arg.EffectiveEndDate <= DateTime.MinValue) { return DateTime.Today; } return arg.EffectiveEndDate; }));

            //AutoMapper.Mapper.CreateMap<LaborCategory, MasterDataCache_LaborCategory>()
            //    .IgnoreAllNonExisting()
            //    .ForMember(d => d.EffectiveEndDate, opt => opt.UseValue(DateTime.Today))
            //    .ForMember(o => o.UtilizationEligibleIndicator, opt => opt.ResolveUsing(arg => { if (arg.UtilizationEligibleIndicator == "Y") { return true; } else { return false; } }))
            //    .ForMember(o => o.CustomerFacingIndicator, opt => opt.ResolveUsing(arg => { if (arg.CustomerFacingIndicator == "Y") { return true; } else { return false; } }))
            //    .ForMember(o => o.BillableIndicator, opt => opt.ResolveUsing(arg => { if (arg.BillableIndicator == "Y") { return true; } else { return false; } }));

            //AutoMapper.Mapper.CreateMap<MSIT.ES.WWTK.Kairos.DataEntities.TimeZone, MasterDataCache_TimeZone>()
            //    .IgnoreAllNonExisting();

            //AutoMapper.Mapper.AssertConfigurationIsValid();
        }


        public async Task SyncLateReasonCodes()
        {
            var lateReasonCodes = await ULSDataAccessor.GetLateReasonCodesAsync(DateTime.Now);
            this.WriteLateReasonCodesInToCacheFile(lateReasonCodes);
            this._isLateReasonCodesSync = true;
        }

        public async Task SyncAdjustmentReasonCodes()
        {
            var adjustmentReasonCodes = await ULSDataAccessor.GetAdjustmentReasonCodesAsync(DateTime.Now);
            this.WriteAdjustmentReasonCodesInToCacheFile(adjustmentReasonCodes);
            this._isAdjustmentReasonCodesSync = true;
        }

        public async Task SyncRejectionReasonCodes()
        {
            var rejectionReasonCodes = await ULSDataAccessor.GetRejectionReasonCodesAsync(DateTime.Now);
            this.WriteRejectionReasonCodesInToCacheFile(rejectionReasonCodes);
            this._isRejectionReasonCodesSync = true;
        }

        private async Task SyncLaborCategories()
        {
            var laborCategories = await ULSDataAccessor.GetLaborCategoriesAsync(DateTime.Now);
            this.WriteLaborCategoriesInToCacheFile(laborCategories);
            this._isLaborCategoriresSync = true;
        }

        public async Task SyncTimeZones()
        {
            var timezones = await ULSDataAccessor.GetTimeZonesAsync(DateTime.Now);
            this.WriteTimeZonesInToCacheFile(timezones);
            this._isTimeZoneSync = true;
        }

        public async Task SyncValidationRules()
        {
            var validationRules = await ULSDataAccessor.GetValidationRulesAsync(DateTime.Now);
            this.WriteValidationRulesInToCacheFile(validationRules);
            this._isValidationRulesSync = true;
        }

        private void WriteLateReasonCodesInToCacheFile(LateReasonCodes lateReasonCodes)
        {

        }
        private void WriteAdjustmentReasonCodesInToCacheFile(AdjustmentReasonCodes adjustmentReasonCodes)
        {

        }
        private void WriteRejectionReasonCodesInToCacheFile(RejectionReasonCodes rejectionReasonCodes)
        {

        }
        private void WriteLaborCategoriesInToCacheFile(LaborCategories laborCategories)
        {

        }
        private void WriteTimeZonesInToCacheFile(TimeZones timezones)
        {
            if (timezones != null)
            {
                //using (var context = new ULSMasterDataCacheEntities(this.ULSConnectionString))
                //{
                //    foreach (var timezone in timezones.TimeZoneList)
                //    {
                //        //var tz =  AutoMapper.Mapper.Map<MSIT.ES.WWTK.Kairos.DataEntities.TimeZone, MasterDataCache_TimeZone>(timezone);
                //        //tz.Guid = Guid.NewGuid();
                //        context.MasterDataCache_TimeZone.Add(tz);
                //    }
                //    context.SaveChanges();
                //}
            }

        }
        private void WriteValidationRulesInToCacheFile(ValidationRules validationRules)
        {

        }



        private static class ULSDataAccessor
        {
            //private static ISettingsProvider _settings;
            private static Dictionary<string, DateTime> _webRequestTimes;

            static ULSDataAccessor()
            {
                _webRequestTimes = new Dictionary<string, DateTime>();
            }

            public static async Task<LateReasonCodes> GetLateReasonCodesAsync(DateTime requestTime)
            {
                var requestURL = string.Empty; //_settings.GetSetting<string>("ULSMasterDataLink");
                if (IsWebRequestTriggerRequired(requestURL, requestTime))
                {
                    string data = await HttpOperations.Instance.GetData(SessionConstants.LateReasonCodesLink);
                    return JsonConvert.DeserializeObject<LateReasonCodes>(data);
                }
                return null;
            }

            public static async Task<LaborCategories> GetLaborCategoriesAsync(DateTime requestTime)
            {
                string data = await HttpOperations.Instance.GetData(SessionConstants.LaborCategoriesLink);
                return JsonConvert.DeserializeObject<LaborCategories>(data);
            }

            public static async Task<AdjustmentReasonCodes> GetAdjustmentReasonCodesAsync(DateTime requestTime)
            {
                string data = await HttpOperations.Instance.GetData(SessionConstants.AdjustementReasonCodesLink);
                return JsonConvert.DeserializeObject<AdjustmentReasonCodes>(data);
            }

            public static async Task<RejectionReasonCodes> GetRejectionReasonCodesAsync(DateTime requestTime)
            {
                string data = await HttpOperations.Instance.GetData(SessionConstants.RejectionReasonCodesLink);
                return JsonConvert.DeserializeObject<RejectionReasonCodes>(data);
            }

            public static async Task<TimeZones> GetTimeZonesAsync(DateTime requestTime)
            {
                string data = await HttpOperations.Instance.GetData(SessionConstants.TimeZonesLink);
                return JsonConvert.DeserializeObject<TimeZones>(data);
            }

            public static async Task<ValidationRules> GetValidationRulesAsync(DateTime requestTime)
            {
                string data = await HttpOperations.Instance.GetData(SessionConstants.ValidationRulesLink);
                return JsonConvert.DeserializeObject<ValidationRules>(data);
            }


            // Helper Method to enable throttling while making web requests based on time span [default = 5 minutes]   
            private static bool IsWebRequestTriggerRequired(string requestURL, DateTime requestTime, TimeSpan? span = null)
            {
                bool hasRequestTobeMade = false;
                if (_webRequestTimes != null)
                {
                    DateTime lastRequestTime;
                    if (_webRequestTimes.TryGetValue(requestURL, out lastRequestTime))
                    {
                        var defaultTimeSpan = (span == null) ? new TimeSpan(0, 5, 0) : span;
                        if (requestTime.Subtract(lastRequestTime).TotalMinutes > defaultTimeSpan.Value.TotalMinutes)
                        {
                            // web request trigger is requried : setting the flag to true. 
                            hasRequestTobeMade = true;
                            // updating current request value since last request has crossed the specified time span. 
                            _webRequestTimes[requestURL] = requestTime;
                        }
                    }
                    else
                    {
                        _webRequestTimes.Add(requestURL, requestTime);
                        hasRequestTobeMade = true;
                    }
                }
                return hasRequestTobeMade;
            }

        }
    }
}
