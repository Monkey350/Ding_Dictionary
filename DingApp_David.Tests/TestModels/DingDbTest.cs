using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DingApp_David.Models;


namespace DingApp_David.Tests.TestModels
{
    public class DingDbTest : IDingDb
    {
        private List<WordModel> testWords = new List<WordModel>()
        {
            new WordModel { word = "test_Word_1", definitions = "test Definition 1" },
            new WordModel { word = "test_Word_2", definitions = "test Definition 2" },
            new WordModel { word = "test_Word_3", definitions = "test Definition 3" },
            new WordModel { word = "test_Word_4", definitions = "test Definition 4" },
            new WordModel { word = "test_Word_5", definitions = "test Definition 5" },
        };
            
        
        IQueryable<T> IDingDb.Query<T>() //there's probably a better way of doing this...
        {
            return Queryable.AsQueryable<T>(testWords as IEnumerable<T>);
        }

        T IDingDb.Add<T>(T entity)
        {
            testWords.Add(entity as WordModel);
            return entity;
        }

        void IDisposable.Dispose()
        {
            
        }
    }
}
