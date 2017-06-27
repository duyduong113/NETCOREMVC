using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class BaseEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        public int ID { get; set; }
    }
}
