using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Web.Mvc;


namespace BDProject_Tests
{
    [TestClass]
    public class HomeControllerTest :BDProject.Controllers.HomeController
    {
        BDProject.Controllers.HomeController controller;
        ViewResult result;
       
        [TestInitialize]
        public void SetupContext()
        {
            controller = new BDProject.Controllers.HomeController();
            result = controller.Index() as ViewResult;
        }
        
        [TestMethod]
        public void IndexViewResultNotNull()
        {

            Assert.IsNotNull(result);
        }
       
        [TestMethod]
        public void IndexViewEqual_null()
        {
            
            Assert.AreEqual("", result.ViewName);
        }


        //Тест метода поиска картинок
        [TestMethod]
        public void Parsser_Image_referencstring_imagesreferencereturned()
        {
            // arenge (Настройка всего необходимого)
            string STR = "http://tt.ua/bytovaja-tehnika-dlja-doma/tehnika-dlya-stirki/kbt-stiralnie-mashini/indesit-iwsc-50852-c-eco-eu";

            WebClient wb2 = new WebClient();
            string str = wb2.DownloadString(STR);

            string expected = "http://tt.ua/image/cache/data/product/212242/indesit-iwsc-50852-c-ecu-300x280.jpg";
            //act (действие )

            HomeControllerTest c = new HomeControllerTest();

            string result =c.Parsser_Image(STR);

            //assert (првильно ли закончился код)
            Assert.AreEqual(expected, result);
        }
        //Тест метода поиска цены
        [TestMethod]
        public void Parsser_Price_referencstring_pricereturned()
        {
            // arenge (Настройка всего необходимого)
            string STR = "http://tt.ua/bytovaja-tehnika-dlja-doma/tehnika-dlya-stirki/kbt-stiralnie-mashini/indesit-iwsc-50852-c-eco-eu";

            WebClient wb2 = new WebClient();
            string str = wb2.DownloadString(STR);

            string expected = "4874";
            //act (действие )

            HomeControllerTest c = new HomeControllerTest();
            string result = c.Parsser_Price(STR);

            //assert (првильно ли закончился код)
            Assert.AreEqual(expected, result);

        }
        //Тест метода поиска описания
        [TestMethod]
        public void Parsser_description_referencstring_descriptionreturned()
        {
            // arenge (Настройка всего необходимого)
            string STR = "http://tt.ua/bytovaja-tehnika-dlja-doma/tehnika-dlya-stirki/kbt-stiralnie-mashini/indesit-iwsc-50852-c-eco-eu";

            WebClient wb2 = new WebClient();
            string str = wb2.DownloadString(STR);

            string expected = "Тип: полногабаритные &gt; 50см\nТип загрузки: фронтальная\nМаксимальная загрузка: 5кг\nКласс потребления электроэнергии: А";

            //act (действие )

            HomeControllerTest c = new HomeControllerTest();
            string result = c.Parsser_description(STR);
            
            //assert (првильно ли закончился код)
            Assert.AreEqual(expected, result);
        }
        //Тест метода поиска названия
        [TestMethod]
        public void Parsser_Name_referencstring_Namereturned()
        {
            // arenge (Настройка всего необходимого)
            string STR = "http://tt.ua/bytovaja-tehnika-dlja-doma/tehnika-dlya-stirki/kbt-stiralnie-mashini/indesit-iwsc-50852-c-eco-eu";

            WebClient wb2 = new WebClient();
            string str = wb2.DownloadString(STR);

            string expected = "Стиральная машина INDESIT IWSC 50852 C ECO EU";

            //act (действие )

            HomeControllerTest c = new HomeControllerTest();
            string result = c.Parsser_Name(STR);

            //assert (првильно ли закончился код)
            Assert.AreEqual(expected, result);
        }

    }
}
