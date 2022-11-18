using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SchedulerSetMultiple : System.Web.UI.Page
{
    SchDbHelper db = new SchDbHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {

            ddlLocation.DataTextField = "Location";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataSource = db.GetData("select * from tblLocations  where Is_Active='True' Order By Location ");
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("Please Select", "0"));
            if (Request.QueryString["location"] != null)
            {
                ddlLocation.SelectedValue = Request.QueryString["location"].ToString();
            }
        

        }
    }

        protected void btnSave_Click(object sender, EventArgs e)
    {
        int age = 0;
        ILog log = log4net.LogManager.GetLogger(typeof(SchedulerSetMultiple));
        DBHelperClass db = new DBHelperClass();
        try
        {
            string SP = "nusp_Insert_Patient_Sch";
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Sex", ddlSex.SelectedValue);
            param[1] = new SqlParameter("@FirstName", txtFirstName.Text);
            param[2] = new SqlParameter("@LastName", txtLastName.Text);

            DateTime dob = new DateTime(Int16.Parse(txtDOB.Text.Substring(6)), Int16.Parse(txtDOB.Text.Substring(0, 2)), Int16.Parse(txtDOB.Text.Substring(3, 2)));
            if (string.IsNullOrWhiteSpace(txtDOB.Text))
            {
                param[3] = new SqlParameter("@DOB", DBNull.Value);
            }
            else
            {
                age = System.DateTime.Now.Year - dob.Year;
                param[3] = new SqlParameter("@DOB", dob);
            }


            param[4] = new SqlParameter("@AGE", age);
            param[5] = new SqlParameter("@Phone", txtPhone.Text);
            param[6] = new SqlParameter("@Phone2", txtMobile.Text);
            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                hfPatientId.Value = val.ToString();
                txtPatientName.Text = ddlSex.SelectedValue + " " + txtFirstName.Text + " " + txtLastName.Text;
            }
            //    Response.Redirect("SchedulerSetNew.aspx?pid=");
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void btnSet_Click(object sender, EventArgs e)
    {
        CultureInfo culture = new CultureInfo("en-US");
        foreach ( ListItem date in lstSelectedDate.Items )
        {
             DateTime tm = DateTime.Parse(txtTime.Text);              
            string datetime = date + " " + tm.ToString("HH:mm");
            DateTime d = Convert.ToDateTime(datetime , culture);

            string AppointmentDate = d.ToString("yyyy-MM-dd HH:mm").Replace(" ", "T");
            //         if (!db.CheckDate(AppointmentDate, Convert.ToInt16(ddlLocation.SelectedValue)))
            //       {
            db.Insert(hfPatientId.Value, ddlLocation.SelectedValue, d.ToString("yyyy-MM-dd HH:mm"), txtNote.Text);
        }
        Response.Write("<script>parent.closemodal()</script>");
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        if (ddlLocation.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Location First')", true);
            return;
        }
        if (hfPatientId.Value == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Patient')", true);
            return;
        }
        if (txtDateFrom.Text == "" || txtDateTo.Text == "" || txtTime.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select From-Date To-Date and Time ')", true);
            return;

        }
        List<int> days = SelectedDays();
        if (days.Count==0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Any Day ')", true);
            return;
        }

        CultureInfo culture = new CultureInfo("en-US");
        DateTime datefrom = Convert.ToDateTime(txtDateFrom.Text, culture);
        DateTime dateto = Convert.ToDateTime(txtDateTo.Text, culture);
        var dates = new List<DateTime>();
        
        for (var dt = datefrom; dt <= dateto; dt = dt.AddDays(1))
        {
            if(days.Contains ((int) dt.DayOfWeek ) )
               dates.Add(dt);
        }

        foreach (DateTime dt in dates)
        {
            lstSelectedDate.Items.Add(dt.ToString("MM/dd/yyyy"));
        }
        setdata.Visible = false;
        viewDates.Visible = true;
    }

    protected List<int> SelectedDays()
    {
        var days = new List<int>();
        if (chkMon.Checked) days.Add(1);
        if (chkTue.Checked) days.Add(2);
        if (chkWed.Checked) days.Add(3);
        if (chkThr.Checked) days.Add(4);
        if (chkFri.Checked) days.Add(5);
        if (chkSat.Checked) days.Add(6);
        if (chkSun.Checked) days.Add(0);
        return days;
    }

}