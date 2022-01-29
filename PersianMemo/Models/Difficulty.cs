using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public enum Difficulty
    {
        [Display(Name = "Początkujący")]
        Beginner = 1,
        [Display(Name = "Podstawowy")]
        Basic = 2,
        [Display(Name = "Średnio-zaawansowany")]
        Intermediate = 3,
        [Display(Name ="Zaawansowany")]
        Advanced = 4
    }
}
