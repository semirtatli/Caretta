global using Caretta.Business.Concrete;
global using Caretta.Core.Dto.EmailDto;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using Caretta.Business.Abstract;
using Caretta.Business.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Caretta.Business.Validation;
using Hangfire;
using Caretta.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using MediatR;
using Caretta.Business.Handlers;
using Caretta.API;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>());
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>());
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CommentCreateDtoValidator>());
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContentCreateDtoValidator>());
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RoleCreateDtoValidator>());
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserCreateDtoValidator>());




var assemblies = Assembly.Load("Caretta.Business");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));


//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllCategoriesQueryHandler>());






builder.Services.AddHangfire(configuration =>
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddDbContext<CarettaContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-5G69SA1\\SQLEXPRESS;Database=Caretta_Db;Trusted_Connection=True;TrustServerCertificate=True;");
});


builder.Services.AddScoped<ICategoryInterface, CategoryService>();
builder.Services.AddScoped<ICommentInterface, CommentService>();
builder.Services.AddScoped<IContentInterface, ContentService>();
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IContentCategoriesInterface, ContentCategoriesService>();
//builder.Services.AddScoped<IContentTypeInterface, ContentTypeService>();
builder.Services.AddScoped<IRoleInterface, RoleService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IUserRoleInterface, UserRoleService>();
builder.Services.AddScoped<IEmailInterface, EmailService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();
builder.Services.AddScoped<UserContextService>();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey


    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseHangfireDashboard();
app.UseHangfireServer();

RecurringJob.AddOrUpdate<IEmailInterface>(
    "send-daily-emails",
    emailService => emailService.SendDailyEmailsAsync(),
    "7 7 * * *");

RecurringJob.AddOrUpdate<IContentInterface>(
    "update-content-status",
    contentService => contentService.UpdateContentStatusAsync(),
    "*/1 * * * *");

app.MapControllers();



app.Run();
