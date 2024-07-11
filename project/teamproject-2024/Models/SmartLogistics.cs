using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogisticsSystem.Models
{
    public class SmartLogistics
    {
        public int Id { get; set; }
        public string Division { get; set; }
        public string Product { get; set; }

        public DateTime Date { get; set; }

        public static readonly string SELECT_QUERY = @"SELECT [Id]
                                                              ,[Division]
                                                              ,[Product]
                                                              ,[Date]
                                                         FROM [dbo].[SmartLogistics]";

        public static readonly string DELETE_QUERY = @"DELETE FROM [dbo].[SmartLogistics] WHERE Id = @id";

        public static readonly string UPDATE_QUERY = @"UPDATE [dbo].[SmartLogistics]
                                                          SET [Division] = @Division
                                                             ,[Product] = @Product
                                                             ,[Date] = @Date
                                                        WHERE Id = @Id";

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[SmartLogistics]
                                                               ([Division]
                                                                ,[Product]
                                                                ,[Date])
                                                            VALUES
                                                               (@Division
                                                               ,@Product
                                                               ,@Date)";

        public static readonly string SELECT_QUERY_RED = @"SELECT COUNT(*)
                                                              FROM [dbo].[SmartLogistics]
                                                             WHERE Division LIKE '%RED%';";

        public static readonly string SELECT_QUERY_GREEN = @"SELECT COUNT(*)
                                                          FROM [dbo].[SmartLogistics]
                                                         WHERE Division LIKE '%GREEN%';";


        public static readonly string SELECT_QUERY_BLUE = @"SELECT COUNT(*)
                                                          FROM SmartLogistics
                                                         WHERE Division LIKE '%BLUE%';";

    }
}
