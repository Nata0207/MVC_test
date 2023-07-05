using System.ComponentModel.DataAnnotations;

namespace MVC_test.Models
{
    public class CommandModel
    {
        [Required(ErrorMessage = "Введите команду")]
        public string Command { get; set; }
        public string Output { get; set; }
    }
}
