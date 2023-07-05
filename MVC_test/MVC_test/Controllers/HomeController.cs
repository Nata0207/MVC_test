using Microsoft.AspNetCore.Mvc;
using MVC_test.Models;
using System.Diagnostics;

namespace MVC_test.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new CommandModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteCommand(CommandModel model)
        {


            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "/c " + model.Command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
            process.Start();
            model.Output = process.StandardOutput.ReadToEnd();

            return View("Index", model);
        }
    }
}