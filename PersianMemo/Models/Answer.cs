using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public enum Answer
    {
        NotAnsweredYet = 0,
        AnsweredIncorrectly = 1,
        AnsweredCorrectly = 2
    }
}
