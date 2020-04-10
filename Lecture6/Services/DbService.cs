using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Lecture6.Helpers;

namespace Lecture6.Services
{
    public class DbService : IDbService
    {
        private const string connectionString = @"Server=localhost\SQLEXPRESS01;Integrated Security=true;";

        public void SaveLogData(string data)
        {
            DataLogger.WriteToLogFile(DataLogger.requestsLogFile, data);
        }

        public Student GetStudentByIndex(string index)
        {
            return null;
        }

        public IEnumerable<Student> GetStudents()
        {
            List<Student> _students = new List<Student>();
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var com = new SqlCommand("Select * From Student"))
                {
                    com.Connection = con;

                    con.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        //...
                        _students.Add(st);
                    }
                }
            }
            
            /*
            var dr = Execute("Select * From Student");
            while (dr.Read())
            {
                var st = new Student();
                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.IndexNumber = dr["IndexNumber"].ToString();
                //...
                _students.Add(st);
            }
            */

            return _students;
        }

        public Student GetStudent(string name) 
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var com = new SqlCommand($"Select * From Student Where FirstName Like @name"))
                {
                    com.Connection = con;
                    com.Parameters.AddWithValue("name", name);

                    con.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        //...
                        return st;
                    }
                }
            }

            return null;
        }

        public List<int> GetSemesterEntries(int studentId)
        {
            string sqlString = "Select e.Semester" + 
                                " From Enrollment e" +
                                " Where e.idEnrollment = (Select s.idEnrollment" +
                                " From Student s" +
                                $" Where s.IndexNumber = @studentId)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var com = new SqlCommand(sqlString))
                {
                    com.Connection = con;
                    com.CommandText = sqlString;
                    com.Parameters.AddWithValue("studentId", studentId);

                    con.Open();
                    var dr = com.ExecuteReader();
                    List<int> semesterEntries = new List<int>();
                    while (dr.Read())
                    {
                        semesterEntries.Add(int.Parse(dr["Semester"].ToString()));
                    }
                    return semesterEntries;
                }
            }

            return null;
        }

        /*
        private SqlDataReader Execute(string sqlString)
        {
            using (SqlConnection con = new SqlConnection(
                            @"Server=localhost\SQLEXPRESS01;Integrated Security=true;"))
            {
                using (var com = new SqlCommand(sqlString))
                {
                    com.Connection = con;

                    con.Open();
                    var dr = com.ExecuteReader();
                    return dr;
                    
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        //...
                        _students.Add(st);
                    }
                    

                    con.Close();
                }
            }
        }
        */
    }
}