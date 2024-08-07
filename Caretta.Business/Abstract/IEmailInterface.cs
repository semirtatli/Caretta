using Caretta.Core.Dto.EmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Abstract
{
    public interface IEmailInterface
    {
        Task SendDailyEmailsAsync();
        Task SendEmailAsync(EmailDto request);
    }
}
