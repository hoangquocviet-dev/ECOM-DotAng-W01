using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using phase_1.Data;
using phase_1.Repositories;
using phase_1.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IProductService, phase_1.Services.ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.ICategoryService, phase_1.Services.CategoryService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.ICartService, phase_1.Services.CartService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IOrderService, phase_1.Services.OrderService>();
builder.Services.AddScoped<ICompanySettingRepository, CompanySettingRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.ICompanySettingService, phase_1.Services.CompanySettingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IUserService, phase_1.Services.UserService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IDashboardService, phase_1.Services.DashboardService>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IWishlistService, phase_1.Services.WishlistService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IReviewService, phase_1.Services.ReviewService>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IVoucherService, phase_1.Services.VoucherService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IBrandService, phase_1.Services.BrandService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IProductImageService, phase_1.Services.ProductImageService>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IMomoService, phase_1.Services.MomoService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IBlogService, phase_1.Services.BlogService>();
builder.Services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IReturnRequestService, phase_1.Services.ReturnRequestService>();
builder.Services.AddScoped<IComboRepository, ComboRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IComboService, phase_1.Services.ComboService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.ISupplierService, phase_1.Services.SupplierService>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IPurchaseOrderService, phase_1.Services.PurchaseOrderService>();
builder.Services.AddScoped<IFlashSaleRepository, FlashSaleRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IFlashSaleService, phase_1.Services.FlashSaleService>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IProductVariantService, phase_1.Services.ProductVariantService>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<phase_1.Services.Interfaces.IBannerService, phase_1.Services.BannerService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHostedService<phase_1.Services.AbandonedCartHostedService>();
builder.Services.AddHttpClient<phase_1.Services.Interfaces.IChatService, phase_1.Services.ChatService>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Z_____L_0889671902_Hoang_Quoc_Viet",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<phase_1.Hubs.NotificationHub>("/hubs/notification");

app.Run();