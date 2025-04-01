using Microsoft.EntityFrameworkCore;
using QuestApp.Models.Domain;

namespace QuestApp.Data
{
    public class QuestAppDbContext:DbContext
    {
        public QuestAppDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
                
        }
        //These properties will create tables in Database
        public DbSet<Question> questions { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<User> users { get; set; }

    }
}
