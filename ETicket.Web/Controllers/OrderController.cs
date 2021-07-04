﻿using ETicket.Domain.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace ETicket.Web.Controllers
{
    public class OrderController : Controller
    {

        public OrderController()
        {

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            //string URL = "https://localhost:44352/api/Admin/GetAllActiveOrders";
                  string URL = "https://localhost:44352/Admin/GetAllActiveOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result; //the http message

            var data = response.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
                       
        }

        public IActionResult Details(Guid orderId)
        {


            HttpClient client = new HttpClient();

            //string URL = "https://localhost:44352/api/Admin/GetDetailsForOrder";

            string URL = "https://localhost:44352/Admin/GetDetailsForOrder";
            var model = new
            {

                Id = orderId
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }



        public FileContentResult CreateInvoice(Guid orderId)
        {
            HttpClient client = new HttpClient();

            //string URL = "https://localhost:44352/api/Admin/GetDetailsForOrder";
            string URL = "https://localhost:44352/Admin/GetDetailsForOrder";

            var model = new
            {

                Id = orderId
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", data.Id.ToString());
            document.Content.Replace("{{UserName}}", data.User.UserName);

            StringBuilder sb = new StringBuilder();


            var totalPrice = 0.0;

            foreach (var item in data.Movies)
            {

                totalPrice += item.Quantity * item.SelectedMovie.MoviePrice;
                sb.AppendLine(item.SelectedMovie.MovieName + " amount: " + item.Quantity + " and price: " + item.SelectedMovie.MoviePrice + "$");
            }

            document.Content.Replace("{{MovieList}}", sb.ToString());

            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");



            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportedInvoice.pdf");
        }

        [HttpGet]
        public IActionResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "applicaton/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {

                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Customer Email";

                HttpClient client = new HttpClient();

                //string URL = "https://localhost:44352/api/Admin/GetAllActiveOrders";
                string URL = "https://localhost:44352/Admin/GetAllActiveOrders";

                HttpResponseMessage response = client.GetAsync(URL).Result; //the http message

                var data = response.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 1; i < data.Count(); i++)
                {
                    var item = data[i - 1];
                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;


                    for (int p = 0; p < item.Movies.Count(); p++)
                    {

                        worksheet.Cell(1, p + 3).Value = "Movie- " + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.Movies.ElementAt(p).SelectedMovie.MovieName;

                    }

                }


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);

                }


            }


            return null;

        }




    }
}