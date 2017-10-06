using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AmRestFeedback.Models.HomeViewModels
{
    public class HomeViewModel
    {
        [Required(ErrorMessage = "RequiredValidationMessage")]
        [Display(Name = "Feedback")]
        [DataType(DataType.MultilineText)]
        [StringLength(300, ErrorMessage = "MaximumLengthValidationMessage")]
        public string Description { get; set; }

        [Required(ErrorMessage = "RequiredValidationMessage")]
        [Display(Name="Name")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "BetweenLengthValidationMessage", MinimumLength = 2)]
        public string Name { get; set; }

        public bool SubmitResult { get; set; }
    }
}
