using AutoMapper;
using Caretta.Core.Dto;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Dto.CommentDto;
using Caretta.Core.Dto.ContentDto;
using Caretta.Core.Dto.UserDto;
using Caretta.Core.Dto.RoleDto;
using Caretta.Core.Entity;

namespace Caretta.API


{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {


            CreateMap<Category, CategoryCreateDto>();
            CreateMap<CategoryCreateDto, Category>();

            CreateMap<Category, CategoryGetDto>();
            CreateMap<CategoryGetDto, Category>();

            CreateMap<Category, CategoryGetByIdDto>()
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.ContentCategories.Select(cc => cc.Content)));
            CreateMap<CategoryGetByIdDto, Category>();

            CreateMap<Comment, CommentGetDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
            CreateMap<CommentGetDto, Comment>();

            CreateMap<Comment, CommentGetContentDto>();
            CreateMap<CommentGetContentDto, Comment>();

            CreateMap<Comment, CommentApproveDto>();
            CreateMap<CommentApproveDto, Comment>();

            CreateMap<Comment, CommentApproveUpdateDto>();
            CreateMap<CommentApproveUpdateDto, Comment>();

            CreateMap<Comment, CommentGetContentDto>();
            CreateMap<CommentGetContentDto, Comment>();

            CreateMap<Comment, CommentCreateDto>();
            CreateMap<CommentCreateDto, Comment>();

            CreateMap<Content, ContentCreateDto>();
            CreateMap<ContentCreateDto, Content>();

            CreateMap<Content, ContentGetDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ContentCategories.Select(cc => cc.Category)));
            CreateMap<ContentGetDto, Content>();

            CreateMap<Content, ContentGetByIdDto>() 
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            CreateMap<ContentGetByIdDto, Content>();

            CreateMap<Content, ContentGetByIdDto>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore());
            CreateMap<Category, CategoryGetDto>();

            CreateMap<Content, ContentGetCategoryDto>();
            CreateMap<ContentGetCategoryDto, Content>();

            CreateMap<Content, ContentApprovalDto>();
            CreateMap<ContentApprovalDto, Content>();

            CreateMap<Content, ContentGetApprovalDto>();
            CreateMap<ContentGetApprovalDto, Content>();

            CreateMap<Content, ContentRejectDto>();
            CreateMap<ContentRejectDto, Content>();

            CreateMap<UserRole, UserRoleDto>();
            CreateMap<UserRoleDto, UserRole>();

            CreateMap<ContentCategories, ContentCategoryDto>();
            CreateMap<ContentCategoryDto, ContentCategories>();

            CreateMap<Role, RoleCreateDto>();
            CreateMap<RoleCreateDto, Role>();

            CreateMap<Role, RoleGetDto>();
            CreateMap<RoleGetDto, Role>();

            CreateMap<Role, RoleGetByIdDto>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.UserRoles.Select(cc => cc.User)));
            CreateMap<RoleGetByIdDto, Role>();

            CreateMap<User, UserCreateDto>();
            CreateMap<UserCreateDto, User>();

            CreateMap<User, UserGetDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(cc => cc.Role)));
            CreateMap<UserGetDto, User>();

            CreateMap<User, UserGetRoleDto>();
            CreateMap<UserGetRoleDto, User>();

            CreateMap<User, SignUpDto>();
            CreateMap<SignUpDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<User, LoginDto>();
            CreateMap<LoginDto, User>();
        }
    }
}
