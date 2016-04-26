using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Extensions
{
    public static class PetaPocoExtensions
    {
        public static bool DoesTableExist(this Database db, string tableName)
        {
            var result =
                db.ExecuteScalar<long>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName",
                                       new { TableName = tableName });

            return result > 0;
        }
    }
}