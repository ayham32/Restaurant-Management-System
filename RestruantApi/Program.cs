using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantApi;
using RestaurantBusiness.InterfaceServices;
using RestaurantBusiness.Services;
using RestaurantBusiness.Validation.BillsValidation;
using RestaurantBusiness.Validation.CategoriesValidation;
using RestaurantBusiness.Validation.CustomersValidation;
using RestaurantBusiness.Validation.EmployeesValidation;
using RestaurantBusiness.Validation.OrderItemsValidation;
using RestaurantBusiness.Validation.OrdersValidation;
using RestaurantBusiness.Validation.PeopleValidation;
using RestaurantBusiness.Validation.ProductsValidation;
using RestaurantBusiness.Validation.UsersValidation;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.InterfaceRepository;
using RestaurantDataAccess.Repository;
using RestaurantDataAccess.UintOfWork;
using RestuarantDataAccess.UintOfWork;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// تسجيل الـ DbContext ليكون متاحاً لكل الطبقات (Dependency Injection)
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardEmployeeRepository, DashboardEmployeeRepository>();


builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<IOrdersService,OrdersService>();
builder.Services.AddScoped<IBillServices,BillServices>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IDashboardService,DashboardService>();
builder.Services.AddScoped<IDashboardEmployeeService, DashboardEmployeeService>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWorkService>();

var key = builder.Configuration["JWT_SECRET_KEY:Key"];


if (string.IsNullOrWhiteSpace(key))
{
    throw new Exception("JWT secret key is not configured.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters define how incoming JWTs will be validated.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ensures the token was issued by a trusted issuer.
            ValidateIssuer = true,


            // Ensures the token is intended for this API (audience check).
            ValidateAudience = true,


            // Ensures the token has not expired.
            ValidateLifetime = true,


            // Ensures the token signature is valid and was signed by the API.
            ValidateIssuerSigningKey = true,


            // The expected issuer value (must match the issuer used when creating the JWT).
            ValidIssuer = "RestaurantApi",


            // The expected audience value (must match the audience used when creating the JWT).
            ValidAudience = "RestaurantApiUsers",


            // The secret key used to validate the JWT signature.
            // This must be the same key used when generating the token.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });



builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});


builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePersonValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCustomerValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEmployeeValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderItemValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderItemValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBillValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBillValidation>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
// ===============================
// 1) Define the JWT Bearer security scheme
// ===============================
//
// This tells Swagger that our API uses JWT Bearer authentication
// through the HTTP Authorization header.
options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    // The name of the HTTP header where the token will be sent.
    Name = "Authorization",


    // Indicates this is an HTTP authentication scheme.
    Type = SecuritySchemeType.Http,


    // Specifies the authentication scheme name.
    // Must be exactly "Bearer" for JWT Bearer tokens.
    Scheme = "Bearer",


    // Optional metadata to describe the token format.
    BearerFormat = "JWT",


    // Specifies that the token is sent in the request header.
    In = ParameterLocation.Header,


    // Text shown in Swagger UI to guide the user.
    Description = "Enter: Bearer {your JWT token}"
});

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


            // No scopes are required for JWT Bearer authentication.
            // This array is empty because JWT does not use OAuth scopes here.
            new string[] {}
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("RestaurantApiCorsPolicy", policy =>
    {
        policy
            //.WithOrigins(
            //    "https://localhost:7115",
            //    "http://localhost:5018"
            //)
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("RestaurantApiCorsPolicy");

app.UseRateLimiter();


app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many login attempts. Please try again later.");
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    await next();


    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var path = context.Request.Path.ToString();


        // ✅ Centralized security log for authorization abuse
        app.Logger.LogWarning(
            "Forbidden access. UserId={UserId}, Path={Path}, IP={IP}",
            userId,
            path,
            ip
        );
    }
});

app.UseStaticFiles();

app.MapControllers();

app.Run();
