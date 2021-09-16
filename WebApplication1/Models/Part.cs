using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Part
    {
        private const string InvalidFloatNumberErrorMassage = "Invalid {0} value: Please enter a float number";
        private const string DoubleDataFormatString = "{0:0.00000}";
        private const string DoubleRangeErrorMessage = "Invalid {0} value: Please enter a value must be greater than or equal zero";
        private const string DoubleValidationRegex = @"[\+|\-]?([0-9]+(\.[0-9][0-9]*)?|(\.[0-9][0-9]*)+)";

        [Key]
        [Required]
        [RegularExpression(@"\w+(-\w+)+",ErrorMessage ="Invalid Part Number format")]
        [Display(Name = "Numbwer")]
        public string     PartNumberCommonized { get; set; } // example value "DS7A-12A692-C",

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DoubleDataFormatString)]
        [RegularExpression(DoubleValidationRegex, ErrorMessage = InvalidFloatNumberErrorMassage)]
        [Range(0.0, double.MaxValue, ErrorMessage = DoubleRangeErrorMessage)]
        [Display(Name = "Length")]
        public double     PartL                { get; set; } // example value 10.85000,

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DoubleDataFormatString)]
        [RegularExpression(DoubleValidationRegex, ErrorMessage = InvalidFloatNumberErrorMassage)]
        [Range(0.0, double.MaxValue, ErrorMessage = DoubleRangeErrorMessage)]
        [Display(Name = "Height")]
        public double     PartH                { get; set; } // example value 2.63000,

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DoubleDataFormatString)]
        [RegularExpression(DoubleValidationRegex, ErrorMessage = InvalidFloatNumberErrorMassage)]
        [Range(0.0, double.MaxValue, ErrorMessage = DoubleRangeErrorMessage)]
        [Display(Name = "Width")]
        public double     PartW                { get; set; } // example value 9.87000,

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Intro Date")]
        public DateTime   PartIntroDate        { get; set; } // example value 2013-01-23T12:56:02.133

        public void Copy(Part item)
        {
            this.PartNumberCommonized = item.PartNumberCommonized;
            this.PartH = item.PartH;
            this.PartL = item.PartL;
            this.PartW = item.PartW;
            this.PartIntroDate = item.PartIntroDate;
        }

        public Part Clone()
        {
            var part = new Part();
            part.Copy(this);
            return this;
        }
    }
}
