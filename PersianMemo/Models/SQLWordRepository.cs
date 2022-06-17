using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public class SQLWordRepository : IWordRepository
    {
        private readonly AppDbContext context;
        public SQLWordRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Word Add(Word word)
        {
            context.Words.Add(word);
            context.SaveChanges();
            return word;
        }

        public Word Delete(int Id)
        {
            Word word = context.Words.Find(Id);
            if (word != null)
            {
                context.Words.Remove(word);
                context.SaveChanges();
            }
            return word;
        }

        public IEnumerable<Word> GetAllWords(string userId)
        {
            return context.Words.Where(w => w.UserId == userId);
        }

        public Word GetWord(int Id)
        {
            return context.Words.Find(Id);
        }

        public Word Update(Word wordChanges)
        {
            context.ChangeTracker.Clear();
            var word = context.Words.Attach(wordChanges);
            word.State = EntityState.Modified;
            context.SaveChanges();
            return wordChanges;
        }
    }
}
