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
    public class HighScoreController : ControllerBase
    {
        [HttpGet("Hello")]
        public string Hello()
        {
            return "Hi there!";
        }
        [HttpGet("GetScores")]
        public string GetScores(int id = 1, int asc = 1)
        {
            SqlConnection conn = new SqlConnection("Server=sql5109.site4now.net;DataBase=db_a6fc55_highscore;User Id=db_a6fc55_highscore_admin;Password=h1ghsc0r3");
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetHighScores", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaderBoardId", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@asc", SqlDbType.Int).Value = asc;
            SqlDataReader rdr = cmd.ExecuteReader();
            string strReturn = "[";
            while (rdr.Read())
            {
                strReturn += "{";
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    switch (i) {
                        case 0:
                            strReturn += "\"LeaderBoardId\":" + rdr.GetValue(i) + ",";
                            break;
                        case 1:
                            strReturn += "\"Name\":\"" + rdr.GetValue(i) + "\",";
                            break;
                        case 2:
                            strReturn += "\"Score\":" + rdr.GetValue(i);
                            break;

                    }
                }
                strReturn += "},";
            }
            strReturn = strReturn.Substring(0, strReturn.Length - 1);
            strReturn += "]";
            return strReturn;
        }
        
        [HttpPost("AddScore")]
        /*
        public string AddScore([FromBody]int LeaderboardId, [FromBody] string apiKey, [FromBody] string username, [FromBody] decimal score)
        {
        */
        [Consumes("application/x-www-form-urlencoded")]
        public string AddScore([FromForm] HighScore hs)
        {
            int LeaderboardId = hs.LeaderboardId;
            string apiKey = hs.apiKey;
            string username = hs.Name;
            decimal score = hs.Score;
            if (LeaderboardId == null || apiKey == null || username == null || score == null) {
                return "Error: Parameters are invalid.";
            }


            SqlConnection conn = new SqlConnection("Server=sql5109.site4now.net;DataBase=db_a6fc55_highscore;User Id=db_a6fc55_highscore_admin;Password=h1ghsc0r3");
            conn.Open();
            SqlCommand cmd = new SqlCommand("CheckLeaderBoardKey", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaderBoardId", SqlDbType.Int).Value = LeaderboardId;
            cmd.Parameters.Add("@apikey", SqlDbType.VarChar,50).Value = apiKey;

            object returnId = cmd.ExecuteScalar();
            if(returnId==null)
            {
                return "Error: Leaderboard not found";
            }
            SqlCommand cmd2 = new SqlCommand("InsertHighScore", conn);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.Add("@LeaderBoardId", SqlDbType.Int).Value = LeaderboardId;
            cmd2.Parameters.Add("@Name", SqlDbType.VarChar, 20).Value = username.Replace("?", "");
            cmd2.Parameters.Add("@Score", SqlDbType.Decimal).Value = score;

            int returnId2 = cmd2.ExecuteNonQuery();

            if (returnId2 == -1)
            {
                return "Success!";
            }
            else
            {
                return "Error: Could not insert score";
            }
        }

    }
    public class HighScore
    {

        public string Name { get; set; }
        public Decimal Score { get; set; }
        public int LeaderboardId { get; set; }
        public string apiKey { get; set; }
    }

}
