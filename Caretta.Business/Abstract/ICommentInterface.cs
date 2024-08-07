using Caretta.Core.Entity;
using Caretta.Core.Dto.CommentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Abstract
{
    public interface ICommentInterface
    {
        Task<IEnumerable<CommentGetDto>> GetAllCommentsAsync();
        Task<CommentGetDto> GetCommentByIdAsync(Guid commentId);
        Task<IEnumerable<ContentWithCommentsDto>> GetAllCommentsByUserAsync(Guid userId);
        Task<CommentCreateDto> CreateOrUpdateCommentAsync(Guid? commentId, CommentCreateDto commentDto);
        //Task<CommentCreateDto> CreateCommentAsync(CommentCreateDto comment);
        //Task UpdateCommentAsync(Guid commentId, CommentCreateDto comment);
        //Task<IEnumerable<CommentApproveDto>> GetUnApprovedCommentsAsnyc();
        Task<CommentGetDto> LikeCommentAsync(Guid commentId);
        Task<IEnumerable<CommentApproveDto>> ApproveCommentsAsync(Guid? commentId);//, CommentApproveUpdateDto? commentDto);
        Task DeleteCommentAsync(Guid commentId);
    }
}
