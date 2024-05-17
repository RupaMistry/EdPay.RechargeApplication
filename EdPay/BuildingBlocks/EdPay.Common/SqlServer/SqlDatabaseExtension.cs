using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EdPay.Common.SqlServer
{
    /// <summary>
    /// Common SQLServer setup extension class.
    /// </summary>
    public static class SqlDatabaseExtension
    {
        /// <summary>
        /// Setups and registers given DBContext.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbConnection"></param> 
        public static void Setup<TContext>(this IServiceCollection services, string dbConnection) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(dbConnection,
                    o => o.MigrationsHistoryTable(
                        tableName: HistoryRepository.DefaultTableName)));
        }
    }
}