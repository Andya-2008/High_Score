using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Cors;

namespace HighScore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaisbeckController : ControllerBase
    {
        [HttpGet("Hello")]
        public string Hello()
        {
            return "Hi there!";
        }

        // https://localhost:44382/api/Raisbeck/GetGradYears
        [HttpGet("GetGradYears")]
        public string GetGradYears()
        {
            SqlConnection conn = new SqlConnection("Server=sql5109.site4now.net;DataBase=db_a6fc55_highscore;User Id=db_a6fc55_highscore_admin;Password=h1ghsc0r3");
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetGradYears", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = cmd.ExecuteReader();
            string strReturn = "[";
            while (rdr.Read())
            {
                strReturn += "{";
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    switch (i)
                    {
                        case 0:
                            strReturn += "\"GradYear\":" + rdr.GetValue(i);
                            break;

                    }
                }
                strReturn += "},";
            }
            if (strReturn != "[")
            {
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }
            strReturn += "]";
            return strReturn;
        }

        // https://localhost:44382/api/Raisbeck/GetStudents
        [HttpGet("GetStudents")]
        public string GetStudents(int year=0)
        {
            SqlConnection conn = new SqlConnection("Server=sql5109.site4now.net;DataBase=db_a6fc55_highscore;User Id=db_a6fc55_highscore_admin;Password=h1ghsc0r3");
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetStudents", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = cmd.ExecuteReader();
            string strReturn = "[";
            while (rdr.Read())
            {
                if ((int)rdr.GetValue(2) == year || year == 0)
                {
                    strReturn += "{";
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                strReturn += "\"StudentId\":" + rdr.GetValue(i) + ",";
                                break;
                            case 1:
                                strReturn += "\"Name\":\"" + rdr.GetValue(i) + "\",";
                                break;
                            case 2:
                                strReturn += "\"GradYear\":" + rdr.GetValue(i);
                                break;

                        }
                    }
                    strReturn += "},";
                }
            }
            if (strReturn != "[")
            {
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }
            strReturn += "]";
            return strReturn;
        }


    }

}
