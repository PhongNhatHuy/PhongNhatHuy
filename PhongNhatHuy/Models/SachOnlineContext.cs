using System.Data.Entity;

namespace PhongNhatHuy.SachOnline.Models
{
    public class SachOnlineContext : DbContext
    {
        public SachOnlineContext() : base("name=SachOnlineConnection")
        {
        }

        public DbSet<ThongTinCaNhan> ThongTinCaNhans { get; set; }
    }
}