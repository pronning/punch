using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Punch.Models
{
    public class CategoryModel : BaseModel
    {
        
        [DisplayName("Kategori")]
        [Required(ErrorMessage = "Få kategorinavn!")]
        public string Value { get; set; }

        public bool DefaultCommon { get; set; }

        public string Filter { get; set; }
    }

   
}