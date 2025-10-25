namespace Appnext_AdCampaign.Models
{
    public class CampaignReadModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public required double Budget { get; set; }
    }
}
