using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer.Data
{
    public class ShippingContextFactory : IDesignTimeDbContextFactory<ShippingContext>
    {
        public ShippingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShippingContext>();

            optionsBuilder.UseSqlServer(
               "Server=.;Database=Shipping_DB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ShippingContext(optionsBuilder.Options);
        }
    }
}