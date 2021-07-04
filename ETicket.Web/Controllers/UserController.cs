using ETicket.Domain.DomainModels;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ETicket.Web.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ImportUsers(IFormFile file)
        {

            //pravime kopija

            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";


            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {

                file.CopyTo(fileStream);
                fileStream.Flush();

            }

            //citame od kopija

            List<User> users = getAllUsersFromFile(file.FileName);


            HttpClient client = new HttpClient();

           // string URL = "https://localhost:44352/api/Admin/ImportAllUsers";
            string URL = "https://localhost:44352//Admin/ImportAllUsers";



            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<bool>().Result;


            return RedirectToAction("Index", "Order");

        }


        private List<User> getAllUsersFromFile(string fileName)
        {


            List<User> users = new List<User>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); //za enkodiranje na podatoci

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {


                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    while (reader.Read())
                    {
                        users.Add(new User
                        {

                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString()

                        });
                    }

                }
            }

            return users;

        }




    }
}
