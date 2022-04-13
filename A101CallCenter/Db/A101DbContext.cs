using A101CallCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace A101CallCenter.Db
{
    public class A101DbContext : DbContext
    {
        public A101DbContext(DbContextOptions<A101DbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
