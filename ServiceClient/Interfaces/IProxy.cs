using ServiceClient.Entities;
using System;
using System.Threading.Tasks;

namespace ServiceClient.Interfaces
{
    public interface IProxy
    {
        Task<AdjustmentReasonCodes> GetAdjustmentReasonCodes();
        Task<LaborCategories> GetLateCategories();
        Task<LateReasonCodes> GetLateReasonCodes();
        Task<RejectionReasonCodes> GetRejectionReasonCodes();
        Task<TimeZones> GetTimeZones();
    }
}
