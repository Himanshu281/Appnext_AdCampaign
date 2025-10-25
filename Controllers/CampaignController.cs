using Appnext_AdCampaign.Attributes;
using Appnext_AdCampaign.Models;
using Appnext_AdCampaign.Services;
using Microsoft.AspNetCore.Mvc;

namespace Appnext_AdCampaign.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public ActionResult GetAllCampaigns()
        {
            var campaigns = _campaignService.GetAllCampaigns();
            return Ok(campaigns);
        }

        [ApiKey]
        [HttpPost("{id}")] // Can be patch since we are updating a resource partially
        public async Task<ActionResult> PostCampaignById(int id, CampaignWriteModel request)
        {
            // To stimulate delay
            await Task.Delay(2000);

            var updatedCampaign = _campaignService.UpdateCampaignStatus(id, request.NewStatus);

            if(updatedCampaign == null)
            {
                return BadRequest("Invalid status transition or campaign not found.");
            }

            return Ok(updatedCampaign);
        }
    }
}
