using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace Museum
{
    /// <summary>
    /// Summary description for Museum
    /// </summary>
    [WebService(Namespace = "http://localhost:4567/Museum/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Museum : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetBeaconAssocaitedObject(string BeaconId)
        {

            //Connection to database.
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            //Get type of object hat associated with beacon from table beacon.
            cmd.CommandText = "SELECT Type FROM Beacon WHERE BeaconId='" + BeaconId + "'";
            SqlDataReader reader = cmd.ExecuteReader();
            string Type = "";
            while (reader.Read())
            {
                Type = reader[0].ToString();
            }
            con.Close();

            //Make list to fill it with statue info.
            //List<StutueInfo> ListOfStutue = new List<StutueInfo>();
            StutueInfo info = new StutueInfo();

            //After this condition i got the data with associated object that is one of(Museum, Part, Stutue).
            if (Type == "m")
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM Museum WHERE BeaconId='" + BeaconId + "'";
                SqlDataReader readerM = cmd.ExecuteReader();
                con.Close();
            }
            else if (Type == "p")
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM Part WHERE BeaconId='" + BeaconId + "'";
                SqlDataReader readerP = cmd.ExecuteReader();
                con.Close();
            }

            else 
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM StutueInfo WHERE StutueId = (SELECT StutueId FROM Stutue WHERE  BeaconId = '" + BeaconId + "')";
                SqlDataReader readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    //StutueInfo info = new StutueInfo();
                    //عدل بدل متسجل ال الاى دى بتاع الانفو سجل الاى دى بتاع التمثال نفسه
                    ////////////////////////////////////////////////////////////////////////
                    info.Id = readerS[0].ToString();
                    info.Name = readerS[2].ToString();
                    info.Description = readerS[3].ToString();
                    info.Audio = readerS[4].ToString();
                    info.Image = readerS[5].ToString();
                    
                    //ListOfStutue.Add(info);
                }
                con.Close();
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            HttpContext.Current.Response.Write(ser.Serialize(info));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetAllParts()
        {
            List<PartInfo> ListOfPart = new List<PartInfo>();

            //Connection to database.
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            // Get all parts.
            cmd.CommandText = "SELECT * FROM Part";
            SqlDataReader readerPart = cmd.ExecuteReader();
            while (readerPart.Read())
            {
                PartInfo info = new PartInfo();
                info.Name = readerPart[1].ToString();
                info.Description = readerPart[2].ToString();
                info.Image = readerPart[3].ToString();

                ListOfPart.Add(info);
            }
            con.Close();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            HttpContext.Current.Response.Write(ser.Serialize(ListOfPart));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetAllStutueInSpeceficPart( int PartId )
        {
            List<StutueInfo> ListOfStutue = new List<StutueInfo>();

            //Connection to database.
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT Stutue.StutueId, StutueInfo.Name, StutueInfo.Description FROM Part INNER JOIN Stutue ON Part.PartId = Stutue.PartId INNER JOIN StutueInfo ON Stutue.StutueId = StutueInfo.StutueId WHERE Part.PartId = '" + PartId + "'";
            SqlDataReader readerPS = cmd.ExecuteReader();
            while (readerPS.Read())
            {
                StutueInfo info = new StutueInfo();
                info.Name = readerPS[1].ToString();
                info.Description = readerPS[2].ToString();
                ListOfStutue.Add(info);
            }

            con.Close();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            HttpContext.Current.Response.Write(ser.Serialize(ListOfStutue));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetAllStutueTitle()
        {
            List<StutueInfo> ListOfStutue = new List<StutueInfo>();

            //Connection to database.
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            // Get specific stutue info.
            cmd.CommandText = "SELECT Stutue.StutueId, StutueInfo.Name, StutueInfo.Description FROM Stutue, StutueInfo WHERE StutueInfo.StutueId = Stutue.StutueId";
            SqlDataReader readerPS = cmd.ExecuteReader();
            while (readerPS.Read())
            {
                StutueInfo info = new StutueInfo();
                info.Id = readerPS[0].ToString();
                info.Name = readerPS[1].ToString();
                info.Description = readerPS[2].ToString();

                ListOfStutue.Add(info);
            }

            con.Close();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            HttpContext.Current.Response.Write(ser.Serialize(ListOfStutue));
        }
        
    }
}