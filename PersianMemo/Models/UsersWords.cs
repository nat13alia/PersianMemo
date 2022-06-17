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
    public class UsersWords
    {
        public UsersWords()
        {
            Status = WordStatus.NotStarted;
            EF = 2.5;
            RevisionsCount = 0;
            RevisionInterval = 1;
        }
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        public int WordId { get; set; }
        [ForeignKey("WordId")]
        public Word Word { get; set; }
        public WordStatus Status { get; set; }
        public double EF { get; set; }
        public int RevisionsCount { get; set; }
        public DateTime NextRevision { get; set; }
        public int RevisionInterval { get; set; }
    }
}
