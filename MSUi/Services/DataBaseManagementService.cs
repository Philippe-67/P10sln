
using Microsoft.EntityFrameworkCore;
using MSUi.Data;

namespace MSUi.Services
{

    public class DataBaseManagementService
    {
        public static void MigrationInitialisation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<UserDbContext>().Database.Migrate();
            }
        }
    }
}


