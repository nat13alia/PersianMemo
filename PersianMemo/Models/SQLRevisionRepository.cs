using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public class SQLRevisionRepository : IRevisionRepository
    {
        private readonly AppDbContext context;
        private readonly IWordRepository _wordRepository;

        public SQLRevisionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Revision Add(Revision revision)
        {
            context.Revisions.Add(revision);
            context.SaveChanges();
            return revision;
        }
    }
}
