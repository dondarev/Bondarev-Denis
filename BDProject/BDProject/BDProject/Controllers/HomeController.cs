using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDProject.Models;

using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
namespace BDProject.Controllers
{
    public class HomeController : Controller
    {
        //Поля контроллера 
       
        Product[] mass; // непроинициализированный масив моделей
       
        ProductContext db = new ProductContext(); // Контекст базы данных


        public ActionResult Index()
        {
            //передаем все значения из БД
            IEnumerable<Product> products = db.Products;

            List<Product> bi = products.ToList();
            List<Product> bi2 = products.ToList();

            for (int i = 0; i < bi.Count; i++)
                for (int j = i+1; j < bi.Count; j++)
                {
                    if (bi[i].picture == bi2[j].picture)
                    {
                        bi.RemoveAt(j);
                    }
                }

                    // возвращаем представление с уникальными знач.
                    ViewBag.Products = bi;
            
            //возвращаем представление Index
            return View();
        }


        //Метод для добавления данных в БД
        public string Add()
        { 
            //Извличение данных со HTML страницы
            mass = Parser("http://tt.ua/bytovaja-tehnika-dlja-doma/tehnika-dlya-stirki/kbt-stiralnie-mashini");

            //Обновление БД
            foreach (Product b in mass)
            {
                b.Date = DateTime.Now;
                db.Products.Add(b);
            }

            db.SaveChanges();
            return "Данные добавлены в базу";
        }
        
        //Метод возвращает предтавление всей БД
        public ActionResult All()
        {
            return View(db.Products);
        }

       
        //Метод для вывда представления изменения цены по определнному товару через POST запрос
       [HttpPost]
        public ActionResult Dinamic(Product p)
        {
            IEnumerable<Product> products = db.Products;
            var numb = from m in products where m.prod_name==p.prod_name select m;
            
            ViewData["Name"] = p.prod_name;
            ViewData["Picture"] = p.picture;
            ViewBag.Products = numb;
            return View();
        }

        //Парсинг HTML страницы
        
        // Основной метод парcинга страницы, находит набор URI по каждому товару, зполняет поле mass 
        private Product[] Parser(string STR)
        {
             List<string> mas = new List<string> { };


            WebClient wb = new WebClient();
            wb.Encoding = Encoding.UTF8;//кодировка для кирилицы
            string str = wb.DownloadString(STR);
            

            Regex re = new Regex("(<a class=\"image\" href=.* onclick=.*[ \n\t]*)");
            foreach (Match m in re.Matches(str))
            {
                mas.Add(m.Value);
            }

            Regex rr = new Regex("<a class=\"image\" href=\"");
            for (int i = 0; i < mas.Count; i++)
            {
                mas[i] = rr.Replace(mas[i], "");
            }

            Regex rrr = new Regex("\" onclick=.*[ \n\t]*", RegexOptions.RightToLeft);
            for (int i = 0; i < mas.Count; i++)
            {
                mas[i] = rrr.Replace(mas[i], "");
            }

            Product[] mas_prod = new Product[mas.Count];// массив для ссылок на каждый товар

            for (int i = 0; i < mas.Count; i++)
            {
                mas_prod[i] =new Product();
                mas_prod[i].prod_name = Parsser_Name(mas[i]);
                mas_prod[i].picture = Parsser_Image(mas[i]);
                mas_prod[i].price = Parsser_Price(mas[i]);
                mas_prod[i].description = Parsser_description(mas[i]);
                mas_prod[i].Date = DateTime.Now;
            }
            return mas_prod;
        }

        //Находит сылку на картинку товара
        protected string Parsser_Image(string STR)
        {
            WebClient wb2 = new WebClient();
            string str = wb2.DownloadString(STR);

            Regex par2 = new Regex("<meta property=\"og:image\" content=\"http:/{2}t{2}.*\"");
            Match m = par2.Match(str);
            str = m.Value;

            par2 = new Regex("<meta property=\"og:image\" content=\"");
            str = par2.Replace(str, "");
            par2 = new Regex("\"");
            str = par2.Replace(str, "");

            return str;
        }
        //Находит цену товара 
        protected string Parsser_Price(string STR)
        {
            WebClient wb3 = new WebClient();
            wb3.Encoding = Encoding.UTF8;
            string str2 = wb3.DownloadString(STR);

            Regex par3 = new Regex("[ \n\t]'price': '[0-9]*'");
            Match m = par3.Match(str2);
            str2 = m.Value;


            par3 = new Regex(" 'price': '");
            str2 = par3.Replace(str2, "");
            par3 = new Regex("'");
            str2 = par3.Replace(str2, "");

            if (str2 == "")
            {
                par3 = new Regex("<span class=\"price\">[0-9 ]*<small>грн</small></span>");
                m = par3.Match(wb3.DownloadString(STR));
                str2 = m.Value;
                par3 = new Regex("<span class=\"price\">");
                str2 = par3.Replace(str2, "");
                par3 = new Regex("<small>");
                str2 = par3.Replace(str2, "");
                par3 = new Regex("</small></span>");
                str2 = par3.Replace(str2, "");

            }
            return str2;
        }
        //Находит описание товара
        protected string Parsser_description(string STR)
        {
            WebClient wb4 = new WebClient();
            wb4.Encoding = Encoding.UTF8;
            string str3 = wb4.DownloadString(STR);

            Regex par4 = new Regex("<div class=\"title_group\">Общее описание:</div>[ \n\t]*<ul class=\"parameter_list\">([ \n\t]*.*){4}");
            Match m = par4.Match(str3);
            str3 = m.Value;

            par4 = new Regex("<div class=\"title_group\">Общее описание:</div>[ \n\t]*<ul class=\"parameter_list\">[ \n\t]*");
            str3 = par4.Replace(str3, "");
            par4 = new Regex("<li><span class=\"parameter\">");
            str3 = par4.Replace(str3, "");
            par4 = new Regex("</span><span class=\"value\">");
            str3 = par4.Replace(str3, "");
            par4 = new Regex("</span></li>");
            str3 = par4.Replace(str3, "");
            par4 = new Regex("                                                                                              ");
            str3 = par4.Replace(str3, "");

            return str3;
        }
        //Находит название товара
        protected string Parsser_Name(string STR)
        {
            WebClient wb5 = new WebClient();
            wb5.Encoding = Encoding.UTF8;
            string str4 = wb5.DownloadString(STR);

            Regex par5 = new Regex("[ \n\t]*<div class=\"about_product\">[ \n\t]*<h1 class=\"title\">.*</h1>");
            Match m = par5.Match(str4);
            str4 = m.Value;

            par5 = new Regex("[ \n\t]*<div class=\"about_product\">[ \n\t]*<h1 class=\"title\">");
            str4 = par5.Replace(str4, "");
            par5 = new Regex("</h1>");
            str4 = par5.Replace(str4, "");

            return str4;
        }
    }
}
