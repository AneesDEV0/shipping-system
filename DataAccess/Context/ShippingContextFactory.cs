using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class ShippingContextFactory : IDesignTimeDbContextFactory<ShippingContext>
    {
        public ShippingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShippingContext>();

            // 🔴 ضع نفس الـ Connection String اللي عندك في appsettings.json
            optionsBuilder.UseSqlServer("Server=DESKTOP-0VMM96V;Database=Shipping;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ShippingContext(optionsBuilder.Options);
        }
    }
}

