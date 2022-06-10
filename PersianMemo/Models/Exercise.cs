using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace PersianMemo.Models
{
    public class Exercise
    {
        public Exercise()
        {
            TimeStamp = DateTime.Now;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public  IdentityUser User { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
