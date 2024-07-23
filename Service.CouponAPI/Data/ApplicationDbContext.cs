using Microsoft.EntityFrameworkCore;
using Service.CouponAPI.Models;
using System.Collections.Generic;

namespace Service.CouponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
