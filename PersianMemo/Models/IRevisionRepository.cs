using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public interface IRevisionRepository
    {
        Revision Add(Revision revision);
        public IEnumerable<Word> GetWords(string userID, DateTime date);
        public IEnumerable<Revision> GetAllRevisionsPerUser(string userId);
        public IEnumerable<Revision> GetAllRevisionsPerDay(DateTime date);
        public Revision GetRevisionRow(string userID, int wordId, DateTime date);
        public Revision Update(Revision revisionChanges);
    }
}
