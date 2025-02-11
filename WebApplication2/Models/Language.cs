using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Language
    {
        [Key]
        public required String LanguageCode{ get; set; }
        public String? LanguageName{ get; set; }
    }
}
