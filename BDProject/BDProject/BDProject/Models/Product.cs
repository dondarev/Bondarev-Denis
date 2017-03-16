using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BDProject.Models
{
    public class Product
    {
        public Product()
        {
            Date = DateTime.Now;
        }
        // ID 
        public int ProductId { get; set; }
        //Название товара
        public string prod_name { get; set; }
        //Картинка товара
        public string picture { get; set; }
        //Цена товара
        public string price { get; set; }
        //Описание товара
        public string description { get; set; }
        // дата покупки
        public DateTime Date { get; set; }
    }
}