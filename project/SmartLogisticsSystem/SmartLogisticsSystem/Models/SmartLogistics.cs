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


    }
}
