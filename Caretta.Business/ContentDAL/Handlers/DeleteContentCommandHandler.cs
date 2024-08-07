using AutoMapper;
using Caretta.Business.Commands;
using Caretta.Business.Concrete;
using Caretta.Business.ContentDAL.Commands;
using Caretta.Core.Entity;
using Caretta.Core.Exceptions;
using Caretta.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.ContentDAL.Handlers
{
    public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public DeleteContentCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
        {




            var content = await _context.Contents
                .Include(c => c.ContentCategories)
                    .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(c => c.Id == request.ContentId);

            if (content == null)
            {
                throw new NotFoundException("Content to be deleted not found.");
            }
            if (content.IsDeleted)
            {
                throw new DeletedException("Content to be deleted is already deleted.");
            }

            foreach (var comment in content.Comments)
            {
                //await _commentService.DeleteCommentAsync(comment.Id);
            }

            content.IsDeleted = true;
            _context.Contents.Update(content);

            

            await _context.SaveChangesAsync();

            return content.Id;
        }
    }
}
