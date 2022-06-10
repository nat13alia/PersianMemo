using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public enum WordStatus
    {
        NotStarted = 1,
        InProgress = 2,
        Learned = 3
    }
}
