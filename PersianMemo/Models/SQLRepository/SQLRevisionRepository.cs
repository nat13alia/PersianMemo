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

        public SQLRevisionRepository(AppDbContext context, IWordRepository wordRepository)
        {
            this.context = context;
            _wordRepository = wordRepository;
        }

        public Revision Add(Revision revision)
        {
            context.Revisions.Add(revision);
            context.SaveChanges();
            return revision;
        }

        public IEnumerable<Word> GetWords(string userID, DateTime date)
        {
            var revisions = context.Revisions;
            var words = new List<Word>();

            foreach (Revision revision in revisions)
            {
                if (revision.RevisionDate <= date && revision.UserId == userID)
                {
                    words.Add(_wordRepository.GetWord(revision.WordId));
                }
            }
            return words;
        }

        public IEnumerable<Revision> GetAllRevisionsPerDay(DateTime date)
        {
            return context.Revisions.Where(r => r.RevisionDate <= date);
        }

        public IEnumerable<Revision> GetAllRevisionsPerUser(string userId)
        {
            return context.Revisions.Where(r => r.UserId == userId);

        }



        public Revision GetRevisionRow(string userID, int wordId, DateTime date)
        {
            var revisions = this.GetAllRevisionsPerDay(date).ToList();
            return revisions.Find(r => r.UserId == userID && r.WordId == wordId);
        }

        public Revision Update(Revision revisionChanges)
        {
            context.ChangeTracker.Clear();
            var word = context.Revisions.Attach(revisionChanges);
            word.State = EntityState.Modified;
            context.SaveChanges();
            return revisionChanges;
        }

        public Revision Delete(int id)
        {
            Revision revision = context.Revisions.Find(id);
            if (revision != null)
            {
                context.Revisions.Remove(revision);
                context.SaveChanges();
            }
            return revision;
        }
    }
}
