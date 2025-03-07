using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Account;
using Infrastructure.Profiles;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//baroi dastrasii har user ba dannihoi khud
builder.Services.AddHttpContextAccessor();
// DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IAccountService>(sp =>
    new AccountService(
        sp.GetRequiredService<UserManager<User>>(),
        sp.GetRequiredService<RoleManager<IdentityRole<int>>>(),
        sp.GetRequiredService<IConfiguration>(),
        builder.Environment.WebRootPath
    ));
builder.Services.AddScoped<IChooseUsRepository, ChooseUsRepository>();
// builder.Services.AddScoped<IChooseUsService, ChooseUsService>();
builder.Services.AddScoped<SeedData>();


builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
var uploadPath = builder.Configuration.GetValue<string>("UploadPath") ?? "wwwroot";
builder.Services.AddScoped<IUserService>(sp =>
    new UserService(
        sp.GetRequiredService<IUserRepository>(),
        sp.GetRequiredService<IMapper>(),
        sp.GetRequiredService<IHttpContextAccessor>(),
        sp.GetRequiredService<UserManager<User>>(),
        uploadPath
    ));
builder.Services.AddScoped<IChooseUsService>(sp =>
    new ChooseUsService(
        sp.GetRequiredService<IChooseUsRepository>(),
        sp.GetRequiredService<IMapper>(),
        uploadPath 
    ));

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService>(cr => 
    new CourseService(
    cr.GetRequiredService<ICourseRepository>(),
        uploadPath
    ));

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService>(nw=> 
    new NewsService(
        nw.GetRequiredService<INewsRepository>(),
            uploadPath
        ));

builder.Services.AddAutoMapper(typeof(EntityProfile));
builder.Services.AddMemoryCache();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите JWT через Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.AddControllers();


var env = builder.Environment;
builder.Services.AddScoped<IBannerService>(sp =>
    new BannerService(
        sp.GetRequiredService<IBannerRepository>(),
        sp.GetRequiredService<IMapper>(),
        env.WebRootPath
    )
);

var app = builder.Build();


try
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    var seeder = serviceProvider.GetRequiredService<SeedData>();
    await seeder.SeedRole();
    await seeder.SeedUser();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

// Настройка статических файлов
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
