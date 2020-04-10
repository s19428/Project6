using Lecture6.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lecture6.Services
{
    public class SqlServerDbService : IDbService
    {
        public Student GetStudentByIndex(string index)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand com = new SqlCommand();

            //var dr=com.ExecuteReaderAsync(); //...2 sek
         
            
            /*
             *   com.ExecuteReader(new WhenDataWillBeBackCallback(){
             *      //....
             *   });
             * 
             * 
             * 
             * 
             */

            // Concurrency (two or more things happening at the same time)
            // Multithreading - new Thread().Start()... - legacy code
            /* ThreadPool - C# (CLR), Java (JVM)
             * Db - ConnectionPool
             * 
             * 
             * 
             * 
             */
            // Parallel processing
            // Async code

            if (index == "s1234")
            {
                return new Student { IdStudent = 1, FirstName = "Andrzej", LastName = "Kowalski" };
            }

            return null;
        }

        public IEnumerable<Student> GetStudents()
        {
            //...
            throw new NotImplementedException();
        }

        public void SaveLogData(string data)
        {
           
        }
    }
}
