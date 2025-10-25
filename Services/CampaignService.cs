using Appnext_AdCampaign.Models;
using System.Text.Json;

namespace Appnext_AdCampaign.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly List<CampaignReadModel> _campaigns = new();
        private readonly ILogger<CampaignService> _logger;

        public CampaignService(ILogger<CampaignService> logger)
        {
            _logger = logger;

            var filePath = "campaigns.json";
            if (!File.Exists(filePath))
            {
                _logger.LogError("Campaigns file '{FilePath}' not found.", filePath);
                return;
            }

            try
            {
                var json = File.ReadAllText("campaigns.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var wrapper = JsonSerializer.Deserialize<CampaignWrapper?>(json, options);

                if (wrapper?.Campaigns is { Count: > 0 } campaignsFromWrapper)
                {
                    _campaigns.AddRange(campaignsFromWrapper);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to initialize campaigns from '{FilePath}'. Exception type: {ExceptionType}. Message: {ExceptionMessage}.",
                    filePath,
                    ex.GetType().Name,
                    ex.Message
                );
            }
        }

        public IEnumerable<CampaignReadModel> GetAllCampaigns()
        {
            return _campaigns;
        }

        public CampaignReadModel? UpdateCampaignStatus(int id, string newStatus)
        {
            var campaign = _campaigns.FirstOrDefault(c => c.Id == id);

            if (campaign == null)
            {
                return null;
            }

            if (!ValidateTransition(campaign.Status, newStatus))
            {
                return null;
            }

            campaign.Status = newStatus;
            return campaign;
        }

        private bool ValidateTransition(string currentStatus, string newStatus)
        {
            if (!Enum.TryParse<CampaignStatusEnum>(currentStatus, ignoreCase: true, out var current))
                return false;
            if (!Enum.TryParse<CampaignStatusEnum>(newStatus, ignoreCase: true, out var next))
                return false;

            if (current == CampaignStatusEnum.Active)
            {
                // Active -> Paused allowed
                if (next == CampaignStatusEnum.Paused)
                    return true;
                return false;
            }

            return true;
        }
    }

    public class CampaignWrapper
    {
        public List<CampaignReadModel> Campaigns { get; set; } = new();
    }
}
