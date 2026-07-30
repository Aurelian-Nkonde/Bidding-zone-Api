using bidding_zone_api.AppContext;
using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using bidding_zone_api.Dtos.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using bidding_zone_api.Configuration;
using Microsoft.AspNetCore.Authentication;
using bidding_zone_api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Firing the web application");
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is not found");


    builder.Services.AddDbContext<AppDbContext>(options =>
    {
    options.UseNpgsql(connectionString); 
    });
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Bidding Zone API",
            Version = "v1"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter a valid JWT token"
        });

        options.AddSecurityRequirement(document =>
        {
            var requirement = new OpenApiSecurityRequirement();
            requirement.Add(new OpenApiSecuritySchemeReference("Bearer", document), new List<string>());
            return requirement;
        });
    });

    builder.Services.AddScoped<IValidator<AddressDto>, AddressValidator>();
    builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
    builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();

    builder.Services.AddScoped<IValidator<CreateItemDto>, CreateItemValidator>();
    builder.Services.AddScoped<IValidator<UpdateItemStatusDtos>,UpdateItemStatusValidator>();
    builder.Services.AddScoped<IValidator<UpdateItemDto>, UpdateItemValidator>();

    builder.Services.AddScoped<IValidator<UpdateBidDto>, UpdateBidValidator>();
    builder.Services.AddScoped<IValidator<CreateBidDto>, CreateBidValidator>();
    builder.Services.AddScoped<IValidator<UpdateBidStatusDto>, UpdateBidStatusValidator>();

    builder.Services.AddScoped<IValidator<LoginDto>,  LoginValidator>();

    builder.Services.AddScoped<IUsersService, UsersService>();
    builder.Services.AddScoped<IItemsService, ItemService>();
    builder.Services.AddScoped<IBidService, BidService>();
    
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());
        
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

    builder.Services.AddScoped<IAuthorizationHandler, IsUserOwnerHandler>();
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("IsOwnerPolicy", policy =>
        {
            policy.Requirements.Add(new IsUserOwnerRequirement());
        });
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bidding Zone API v1");
        });
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

} catch(Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}finally
{
    Log.CloseAndFlush();
}

