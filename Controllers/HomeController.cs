using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RandomPasscodeGenerator.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace RandomPasscodeGenerator.Controllers
{
    public class HomeController : Controller
    {

        // GET -> Home Page
        [HttpGet("")]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("PasscodeNumber") == null)
            {
                HttpContext.Session.SetInt32("PasscodeNumber", 1);
            }

            if (HttpContext.Session.GetString("GeneratedPasscode") == null)
            {
                Random rand = new Random();
                StringBuilder builder = new StringBuilder();
                char ch;
                for (int i = 0; i < 14; i++)
                {
                    // 50% chance of adding a digit or a char
                    int option = rand.Next(0, 2);
                    if (option == 0)
                    {
                        // add a char
                        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                        builder.Append(ch);
                    }
                    else
                    {
                        // add a digit
                        int RandomDigit = rand.Next(1, 10);
                        builder.Append(RandomDigit);
                    }
                }
                string GeneratedPasscode = builder.ToString();
                HttpContext.Session.SetString("GeneratedPasscode", GeneratedPasscode);
            }

            ViewBag.GeneratedPasscode = HttpContext.Session.GetString("GeneratedPasscode");
            ViewBag.PasscodeNumber = HttpContext.Session.GetInt32("PasscodeNumber");
            return View();
        }

        // POST -> Home Page
        [HttpPost("")]
        public IActionResult GeneratePasscode()
        {
            // Increment session Pascode counter
            int? Passcode = HttpContext.Session.GetInt32("PasscodeNumber"); // Retrieve Passcode
            int NewPasscode = Passcode ?? default(int); // convert int? -> int
            HttpContext.Session.SetInt32("PasscodeNumber", NewPasscode + 1); // incremenet Passcode

            // Generate a new passcode and store in session
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 14; i++)
            {
                int option = rand.Next(0, 2);
                if (option == 0)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                    builder.Append(ch);
                }
                else
                {
                    int RandomDigit = rand.Next(1, 10);
                    builder.Append(RandomDigit);
                }
            }
            string GeneratedPasscode = builder.ToString();
            HttpContext.Session.SetString("GeneratedPasscode", GeneratedPasscode);
            
            // redirect to Index
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
