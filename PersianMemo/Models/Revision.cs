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
    public class Revision
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public  IdentityUser User { get; set; }
        public int WordId { get; set; }
        [ForeignKey("WordId")]
        public Word Word { get; set; }
        public Answer WriteAnswer { get; set; }
        public DateTime RevisionDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
