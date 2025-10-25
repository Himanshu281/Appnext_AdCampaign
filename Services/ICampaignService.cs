using Appnext_AdCampaign.Models;

namespace Appnext_AdCampaign.Services
{
    public interface ICampaignService
    {
        IEnumerable<CampaignReadModel> GetAllCampaigns();
        CampaignReadModel? UpdateCampaignStatus(int id, string newStatus);
    }
}
