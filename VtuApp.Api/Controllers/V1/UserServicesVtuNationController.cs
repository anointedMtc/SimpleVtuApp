namespace VtuApp.Api.Controllers.V1;

[ApiVersion("1.0")]
public class UserServicesVtuNationController : ApiBaseController
{
    [Authorize]
    [HttpPost("buy-airtime-vtuNation")]
    public async Task<ActionResult<BuyAirtimeVtuNationResponse>> BuyAirtimeVtuNation([FromBody]BuyAirtimeRequestVtuNation buyAirtimeRequestVtuNation)
    {
        var result = await Mediator.Send(new BuyAirtimeVtuNationCommand() { BuyAirtimeRequestVtuNation = buyAirtimeRequestVtuNation });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-any-data-vtuNation")]
    public async Task<ActionResult<BuyDataVtuNationResponse>> BuyAnyDataVtuNation([FromBody] BuyDataRequestVtuNation buyDataRequestVtuNation)
    {
        var result = await Mediator.Send(new BuyDataVtuNationCommand() { BuyDataRequestVtuNation = buyDataRequestVtuNation });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-10GB-data-vtuNation")]
    public async Task<ActionResult<Buy10GBVtuNationResponse>> Buy10GBDataVtuNation([FromBody] string phoneNumber)
    {
        var result = await Mediator.Send(new Buy10GBVtuNationCommand() { PhoneNumber = phoneNumber });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-1GB-data-vtuNation")]
    public async Task<ActionResult<Buy1GBVtuNationResponse>> Buy1GBDataVtuNation([FromBody] string phoneNumber)
    {
        var result = await Mediator.Send(new Buy1GBVtuNationCommand() { PhoneNumber = phoneNumber });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-2GB-data-vtuNation")]
    public async Task<ActionResult<Buy2GBVtuNationResponse>> Buy2GBDataVtuNation([FromBody] string phoneNumber)
    {
        var result = await Mediator.Send(new Buy2GBVtuNationCommand() { PhoneNumber = phoneNumber });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-3GB-data-vtuNation")]
    public async Task<ActionResult<Buy3GBVtuNationResponse>> Buy3GBDataVtuNation([FromBody] string phoneNumber)
    {
        var result = await Mediator.Send(new Buy3GBVtuNationCommand() { PhoneNumber = phoneNumber });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-5GB-data-vtuNation")]
    public async Task<ActionResult<Buy5GBVtuNationResponse>> Buy5GBDataVtuNation([FromBody] string phoneNumber)
    {
        var result = await Mediator.Send(new Buy5GBVtuNationCommand() { PhoneNumber = phoneNumber });

        return Ok(result);
    }


    [Authorize]
    [HttpPost("buy-500MB-data-vtuNation")]
    public async Task<ActionResult<Buy500MBVtuNationResponse>> Buy500MBDataVtuNation([FromBody] string phoneNumber)
    {
        var result = await Mediator.Send(new Buy500MBVtuNationCommand() { PhoneNumber = phoneNumber });

        return Ok(result);
    }




    // READS
    [HttpGet("get-available-airtime-networks-vtuNation")]
    public async Task<ActionResult<GetAvailableAirtimeNetworksResponse>> GetAvailableAirtimeNetworksVtuNation()
    {
        var result = await Mediator.Send(new GetAvailableAirtimeNetworksQuery());

        return Ok(result);
    }


    [HttpGet("get-available-data-networks-vtuNation")]
    public async Task<ActionResult<GetAvailableDataNetworksResponse>> GetAvailableDataNetworksVtuNation()
    {
        var result = await Mediator.Send(new GetAvailableDataNetworksQuery());

        return Ok(result);
    }


    [HttpGet("get-9Mobile-data-prices-vtuNation")]
    public async Task<ActionResult<Get9MobileDataPricesResponse>> Get9MobileDataPricesVtuNation()
    {
        var result = await Mediator.Send(new Get9MobileDataPricesQuery());

        return Ok(result);
    }


    [HttpGet("get-airtel-data-prices-vtuNation")]
    public async Task<ActionResult<GetAirtelDataPricesResponse>> GetAirtelDataPricesVtuNation()
    {
        var result = await Mediator.Send(new GetAirtelDataPricesQuery());

        return Ok(result);
    }


    [HttpGet("get-glo-data-prices-vtuNation")]
    public async Task<ActionResult<GetGloDataPricesResponse>> GetGloDataPricesVtuNation()
    {
        var result = await Mediator.Send(new GetGloDataPricesQuery());

        return Ok(result);
    }


    [HttpGet("get-mtn-data-prices-vtuNation")]
    public async Task<ActionResult<GetMtnDataPricesResponse>> GetMtnDataPricesVtuNation()
    {
        var result = await Mediator.Send(new GetMtnDataPricesQuery());

        return Ok(result);
    }


}
