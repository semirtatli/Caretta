using Caretta.Core.Dto.ContentDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.ContentDAL.Queries
{
    public class ApprovalWaitingContentsForUserQuery : IRequest<IEnumerable<ContentGetApprovalDto>> { }
}
