using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Punch.Models
{
    [Field(CollectionName = "expenses")]
    public class ExpenseModel : BaseModel
    {
        [DisplayName("dato")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [Required(ErrorMessage = "Få dato!")]
        public DateTime Date { get; set; }

        [DisplayName("beskrivelse")]
        [Required(ErrorMessage = "Få beskrivelse!")]
        public String Description { get; set; }

        [Required]
        public String Owner { get; set; }
        
        [Required(ErrorMessage = "Få betalingsmåte!")]
        public String Means { get; set; }
        
        [Required(ErrorMessage = "Få beløp!")]
        [Range(1,Double.MaxValue,ErrorMessage = "Må være minst én krone")]
        public double Amount { get; set; }

        //[Field(FieldAction = FieldAction.Skip)]
        public virtual CategoryModel Category { get; set; }
        
        public bool IsCommon { get; set; }
        public bool IsPossibleDuplicate { get; set; }
    }
}