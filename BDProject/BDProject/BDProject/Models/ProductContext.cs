using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace BDProject.Models
{
    public class ProductContext: DbContext
    { 
        public DbSet<Product> Products { get; set; }//контекст базы данных
    }
}