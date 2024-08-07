using Caretta.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caretta.Core.Dto.ContentDto;

namespace Caretta.Business.Abstract
{
    public interface IContentInterface
    {
        Task<IEnumerable<ContentGetDto>> GetAllContentsAsync();
        Task<ContentGetByIdDto> GetContentByIdAsync(Guid contentId);
        Task<IEnumerable<ContentGetDto>> GetPopularContentsAsync();
        Task<ContentCreateDto> CreateOrUpdateContentAsync(Guid? contentId, ContentCreateDto contentDto);
        /*Task<ContentCreateDto> CreateContentAsync(ContentCreateDto contentDto);
        Task UpdateContentAsync(Guid contentId, ContentCreateDto contentDto);*/
        Task DeleteContentAsync(Guid contentId);
        Task<IEnumerable<ContentGetApprovalDto>> ApprovalWaitingContentsAsync();
        Task<IEnumerable<ContentGetApprovalDto>> EditorApprovalWaitingContentsAsync();
        Task<IEnumerable<ContentGetApprovalDto>> AdminApprovalWaitingContentsAsync();
        Task<IEnumerable<ContentGetApprovalDto>> UserApprovalWaitingContentsAsync();
        Task ApproveContentAsync(Guid contentId);
        Task<ContentGetByIdDto> LikeContentAsync(Guid contentId);

        Task<ContentGetByIdDto> UnlikeContentAsync(Guid contentId);
        Task RejectContentAsync(Guid contentId, ContentRejectDto contentRejectDto);
        Task UpdateContentStatusAsync();
        Task SendContentToApprovalAsync(Guid Id);
        Task NotifyUsersOfNewContent(List<Guid> categoryIds, Guid contentId);
    }
}
