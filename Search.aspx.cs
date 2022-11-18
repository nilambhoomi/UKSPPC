using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Search : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetPatients(string prefix, bool IsPatientPage = false)
    {
        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_SearchByName";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Parameters.AddWithValue("@IsPatient", IsPatientPage);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString() + "_" + sdr["Patient_ID"].ToString());
                    }
                }
                conn.Close();
            }
            return _patients.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetPatientMaster(string prefix)
    {
        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_searchPatientMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString() + "_" + sdr["Patient_ID"].ToString());
                    }
                }
                conn.Close();
            }
            return _patients.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetLocations(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _locations = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select Location_Id,Location from tbllocations where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Location like '%" + prefix + "%' or NameOfPractice like '%"+ prefix +"%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _locations.Add(ds.Tables[0].Rows[i]["Location_Id"].ToString() + "_" + ds.Tables[0].Rows[i]["Location"].ToString());
                    }
                }
            }
            return _locations.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetUsers(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _users = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select User_Id,FirstName,LastName from tblUserMaster where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and FirstName like '%" + prefix + "%' or LastName like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _users.Add(ds.Tables[0].Rows[i]["User_Id"].ToString() + "_" + (ds.Tables[0].Rows[i]["LastName"].ToString()+" "+ ds.Tables[0].Rows[i]["FirstName"].ToString()));
                    }
                }
            }
            return _users.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetAttorney(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select Attorney_Id,Attorney from tblAttorneys where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Attorney like '%" + prefix + "%' or Address1 like '%" + prefix + "%' or Address2 like '%"+prefix+"%' or city like '"+ prefix +"'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["Attorney_Id"].ToString() + "_" + ds.Tables[0].Rows[i]["Attorney"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetDaignoCode(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select DiagCode_ID,DiagCode,Description from tblDiagCodes where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and DiagCode like '%" + prefix + "%' or BodyPart like '%" + prefix + "%' or Description like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["DiagCode_ID"].ToString() + "_" + ds.Tables[0].Rows[i]["Description"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetIncos(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select InsCo_ID,InsCo from tblInsCos where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and InsCo like '%" + prefix + "%' or Address1 like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["InsCo_ID"].ToString() + "_" + ds.Tables[0].Rows[i]["InsCo"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetMedicien(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select Medicine_Id,Medicine from tblMedicines where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Medicine like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["Medicine_Id"].ToString() + "_" + ds.Tables[0].Rows[i]["Medicine"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetPharmacy(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select Pharmacy_ID,Pharmacy from tblPharmacy where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Pharmacy like '%" + prefix + "%' or Address1 like '%"+ prefix +"%' or City like '%"+ prefix +"%' or contactperson like '%"+prefix+"%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["Pharmacy_ID"].ToString() + "_" + ds.Tables[0].Rows[i]["Pharmacy"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetProvider(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select Provider,Provider_ID from tblProviders where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Provider like '%" + prefix + "%' or Address like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["Provider_ID"].ToString() + "_" + ds.Tables[0].Rows[i]["Provider"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetDesignation(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select id,designation from tbl_designation where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and designation like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["designation"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetGroup(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _data = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select id,Name from tblGroups where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Name like '%" + prefix + "%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _data.Add(ds.Tables[0].Rows[i]["id"].ToString() + "_" + ds.Tables[0].Rows[i]["name"].ToString());
                    }
                }
            }
            return _data.ToArray();
        }
    }
}

