using TechAssessment_C_.Models;
using Microsoft.EntityFrameworkCore;

namespace TechAssessment_C_.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Products> Products { get; set; } 
    }
}
