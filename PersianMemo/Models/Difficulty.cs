using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public enum Difficulty
    {
        [Description("Początkujący")]
        Beginner = 1,
        [Description("Podstawowy")]
        Basic = 2,
        [Description("Średnio-zaawansowany")]
        Intermediate = 3,
        [Description("Zaawansowany")]
        Advanced = 4
    }
}
