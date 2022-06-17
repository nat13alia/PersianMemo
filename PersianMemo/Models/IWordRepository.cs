using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersianMemo.Models
{
    public interface IWordRepository
    {
        Word GetWord(int Id);
        IEnumerable<Word> GetAllWords(string userId);
        Word Add(Word word);
        Word Update(Word wordChanges);
        Word Delete(int Id);
    }
}
