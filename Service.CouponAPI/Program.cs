using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Service.CouponAPI.Data;
using Service.CouponAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<Coupon>("Coupons");
    return modelBuilder.GetEdmModel();
}

builder.Services.AddControllers().AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(100)
                .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddAutoMapper(typeof(Program)); // Add AutoMapper configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }
    }
}