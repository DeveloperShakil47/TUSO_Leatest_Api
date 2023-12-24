using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Utilities.Constants;

namespace TUSO.Domain.Dto
{
    public class ModuleDto
    {
        /// <summary>
        /// Name of the Module
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        /// <summary>
        /// Description of the Module.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(500)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
