using Lecture6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lecture6.Services
{
    public interface IDbService
    {
        IEnumerable<Student> GetStudents();

        Student GetStudentByIndex(string index);

        void SaveLogData(string data);
    }
}
