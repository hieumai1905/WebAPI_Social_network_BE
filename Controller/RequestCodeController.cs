using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;
using Web_Social_network_BE.Sockets.SendMails;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api")]
    [ApiController]
    public class RequestCodeController : ControllerBase
    {
        private readonly IRequestCodeRepository _requestCodeRepository;

        public RequestCodeController(IRequestCodeRepository requestCodeRepository)
        {
            _requestCodeRepository = requestCodeRepository;
        }

        [HttpPost("codes/refresh")]
        public async Task<ActionResult<Request>> RefreshCode([FromBody] LoginModel account)
        {
            try
            {
                var codes = await _requestCodeRepository.RefreshCode(account.Email);
                try
                {
                    Mail.SendMail(account.Email, "Confirm code!", codes.RequestCode.ToString(),
                        account.Email);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while sending the email: {ex.Message}");
                }

                return Ok(codes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}