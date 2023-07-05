using Microsoft.AspNetCore.Mvc;
using MVC_test.Models;
using System.Diagnostics;
using System.IO;

namespace MVC_test.Controllers
{
    public class CommandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ExecuteCommand(CommandModel model)
        {
            if (ModelState.IsValid)
            {
                var command = model.Command;

                // выполнить команду в шелле и получить результат
                var result = ExecuteShellCommand(command);

                // вывод результата в консоль для тестирования
                Debug.WriteLine(result);

                // Возвращаем partial view с результатом выполнения команды
                return PartialView("_CommandResult", result);
            }

            // возвращаем вью с ошибкой
            return View("Index", model);
        }

        private static string ExecuteShellCommand(string command)
        {
            var escapedCommand = command.Replace("\"", "\\\"");

            string shell;
            string args;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                shell = "cmd.exe";
                args = $"/c \"{escapedCommand}\"";
            }
            else
            {
                shell = "powershell.exe";
                args = $"-c \"{escapedCommand}\"";
            }

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = shell,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                process.Start();

                // Читаем результат выполнения команды
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    process.WaitForExit();
                    return result;
                }
            }
        }
    }
}
