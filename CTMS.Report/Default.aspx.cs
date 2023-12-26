using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Shared;
public partial class _Default : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string data = Request.QueryString["data"];
        string signId = Request.QueryString["signId"];
         if (data != null)
         {
        System.Data.SqlClient.SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString);

        connection.Open();
        string Data = "SMS";
        //string detail = "select * from RequestForms";
        ReportDocument rpt = new ReportDocument();
        SqlConnection con = new SqlConnection("Data Source=HQITPROGLAP165;User ID=sa; Password=sola@9220; Initial Catalog=CTMS");
        SqlCommand cmd = new SqlCommand("select * from RequestForms where ApplicationName=@Data", con);
        cmd.Parameters.AddWithValue("@Data", Data); //Adds the ID we got before to the SQL command
        DataTable dt = new DataTable();
        con.Open();
        SqlDataReader sdr = cmd.ExecuteReader();
        dt.Load(sdr);
        //
        rpt.Load(Server.MapPath("CrystalReport1.rpt"));
        rpt.SetDataSource(dt);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.Visible = true;
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.DataBind();
            rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "AAMS_Report");

        } 
        if (signId != null)
         {
        System.Data.SqlClient.SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString);

        connection.Open();
     
        //string detail = "select * from RequestForms";
        ReportDocument rpt = new ReportDocument();
        SqlConnection con = new SqlConnection("Data Source=HQITPROGLAP165;User ID=sa; Password=sola@9220; Initial Catalog=CTMS");
        SqlCommand cmd = new SqlCommand("select * from RequestForms where Id=@id", con);
        cmd.Parameters.AddWithValue("@id", signId); //Adds the ID we got before to the SQL command
        DataTable dt = new DataTable();
        con.Open();
        SqlDataReader sdr = cmd.ExecuteReader();
        dt.Load(sdr);
        //
        rpt.Load(Server.MapPath("CrystalReport2.rpt"));
        rpt.SetDataSource(dt);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.Visible = true;
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.DataBind();
        //rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "AAMS_Report");
        }else
        {
            System.Data.SqlClient.SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString);
            connection.Open();
            //string detail = "select * from RequestForms";
            ReportDocument rpt = new ReportDocument();
            SqlConnection con = new SqlConnection("Data Source=HQITPROGLAP165;User ID=sa; Password=sola@9220; Initial Catalog=CTMS");
            SqlCommand cmd = new SqlCommand("select * from RequestForms", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            //
            rpt.Load(Server.MapPath("CrystalReport1.rpt"));
            rpt.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.Visible = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.DataBind();
           // rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "AAMS_Report");
        }

    }
}