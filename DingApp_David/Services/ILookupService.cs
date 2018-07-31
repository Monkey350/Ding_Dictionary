using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DingApp_David.Models;

namespace DingApp_David.Services
{
    public interface ILookupService
    {
        WordModel WordLookup(string word);

        WordModel APILookup(string word);

        WordModel DbLookup(string word);
    }
}
