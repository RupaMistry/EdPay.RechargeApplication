 namespace EdPay.RechargeApp.Api.Controllers
{
    /// <summary>
    /// MobilePlans API Controller
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    [Route("api/mobileplans")] 
    [ApiController] 
    public class MobilePlanController(ITopupPlanService<TopupPlan> service, ILogger<MobilePlanController> logger) : ControllerBase
    {
        private readonly ITopupPlanService<TopupPlan> _planService = service ?? throw new ArgumentNullException(nameof(service));
        private readonly ILogger<MobilePlanController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets a list of available Topup Plans.
        /// </summary>
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(IReadOnlyList<TopupPlan>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)] 
        public async Task<ActionResult<IReadOnlyList<TopupPlan>>> GetTopupPlans()
        {
            // Fetch and return all topup Plans in system
            var topupPlans = await _planService.GetTopupPlans();

            // if list is empty, return NotFound() else success response.
            if (topupPlans == null)
            {
                _logger.LogInformation("No TopupPlans found in database");
                return NotFound();
            }

            _logger.LogInformation("TopupPlans list returned successfully.");
            return Ok(new { Data = topupPlans });
        }
    }
}