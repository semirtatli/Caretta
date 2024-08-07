using Caretta.Business.Abstract;
using Caretta.Business.Concrete;
using Caretta.Core.Dto.EmailDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Caretta.API.Controllers
{
   
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailInterface _emailService;

        public EmailController(IEmailInterface emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("SendEmail")]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmailAsync(request);

            return Ok();
        }
    }
}
