using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SmartFramework4v2.Web.WebExecutor;
using SmartFramework4v2.Data.SqlServer;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Text;
using System.Collections;

/// <summary>
/// Model 的摘要说明
/// </summary>
[CSClass("Model")]
public class Model
{
    [CSMethod("GetUserAndCompanyList")]
    public object GetUserAndCompanyList(int CurrentPage, int PageSize, string UserName, string Company)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;
                string where = "";

                if (!string.IsNullOrEmpty(UserName))
                    where += " and UserName like @UserName";

                if (!string.IsNullOrEmpty(Company))
                    where += " and Company like @Company";

                string sql = "select * from UserAndCompany where 1=1 " + where;
                SqlCommand cmd = db.CreateCommand(sql);
                if (!string.IsNullOrEmpty(UserName))
                    cmd.Parameters.AddWithValue("@UserName", "%" + UserName + "%");
                if (!string.IsNullOrEmpty(Company))
                    cmd.Parameters.AddWithValue("@Company", "%" + Company + "%");

                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                return new { dt = dt, cp = cp, ac = ac };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("AddUserAndCompany")]
    public bool AddUserAndCompany(string UserName, string Company)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();
                string sql = "SELECT * FROM UserAndCompany where UserName = @UserName or Company = @Company";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@UserName", UserName);
                cmd.Parameters.Add("@Company", Company);
                DataTable dt_user = db.ExecuteDataTable(cmd);
                if (dt_user.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    DataTable dt = db.GetEmptyDataTable("UserAndCompany");
                    DataRow dr = dt.NewRow();
                    dr["UserName"] = UserName;
                    dr["Company"] = Company;
                    dt.Rows.Add(dr);
                    db.InsertTable(dt);
                    db.CommitTransaction();
                    return true;
                }
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("DeleteUserAndCompany")]
    public bool DeleteUserAndCompany(string UserName)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "delete from UserAndCompany where UserName = @UserName";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@UserName", UserName);
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetModelList")]
    public object GetModelList(int CurrentPage, int PageSize, string Name)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;
                string where = "";

                if (!string.IsNullOrEmpty(Name))
                    where += " and Name like @Name";

                string sql = "select * from CompanyModel where 1=1 " + where + " order by Addtime";
                SqlCommand cmd = db.CreateCommand(sql);
                if (!string.IsNullOrEmpty(Name))
                    cmd.Parameters.AddWithValue("@Name", "%" + Name + "%");

                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                return new { dt = dt, cp = cp, ac = ac };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetModelListByCHB")]
    public object GetModelListByCHB(int CurrentPage, int PageSize, string Name)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;
                string where = "";

                if (!string.IsNullOrEmpty(Name))
                    where += " and Name like @Name";

                string sql = "select * from CompanyCHBModel where 1=1 " + where + " order by Addtime";
                SqlCommand cmd = db.CreateCommand(sql);
                if (!string.IsNullOrEmpty(Name))
                    cmd.Parameters.AddWithValue("@Name", "%" + Name + "%");

                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                return new { dt = dt, cp = cp, ac = ac };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("AddModel")]
    public string AddModel(string Name, string ShowName)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();
                string sql = "select * from CompanyModel where Name = @Name";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@Name", Name);
                DataTable dt_list = db.ExecuteDataTable(cmd);
                if (dt_list.Rows.Count > 0)
                    throw new Exception("已存在公司名为：" + Name + "的模板！");

                string guid = Guid.NewGuid().ToString();
                DataTable dt = db.GetEmptyDataTable("CompanyModel");
                DataRow dr = dt.NewRow();
                dr["ID"] = guid;
                dr["Name"] = Name;
                dr["ShowName"] = ShowName;
                dr["Addtime"] = DateTime.Now;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                db.CommitTransaction();
                return guid;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("AddModelByCHB")]
    public string AddModelByCHB(string Name, string ShowName)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();
                string sql = "select * from CompanyCHBModel where Name = @Name";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@Name", Name);
                DataTable dt_list = db.ExecuteDataTable(cmd);
                if (dt_list.Rows.Count > 0)
                    throw new Exception("已存在公司名为：" + Name + "的模板！");

                string guid = Guid.NewGuid().ToString();
                DataTable dt = db.GetEmptyDataTable("CompanyCHBModel");
                DataRow dr = dt.NewRow();
                dr["ID"] = guid;
                dr["Name"] = Name;
                dr["ShowName"] = ShowName;
                dr["Addtime"] = DateTime.Now;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                db.CommitTransaction();
                return guid;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("DeleteModel")]
    public bool DeleteModel(string ID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "delete from CompanyModel where ID=@ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@ID", ID);
                db.ExecuteNonQuery(cmd);

                sql = "delete from CompanyModelChild where MID=@ID";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@ID", ID);
                db.ExecuteNonQuery(cmd);

                db.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("SelectWL")]
    public DataTable SelectWL(string MID, string Company)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                string sql = "select count(*) num from CompanyModelChild where MID = @MID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@MID", MID);
                DataTable dt = db.ExecuteDataTable(cmd);
                string conn = "";
                if (Convert.ToInt32(dt.Rows[0]["num"].ToString()) > 0)
                    conn += " and UserName not in (select UserName from CompanyModelChild where MID = @MID)";
                if (!string.IsNullOrEmpty(Company))
                    conn += " and Company like @Company";
                sql = "select * from UserAndCompany where 1=1 " + conn;
                SqlCommand cmd1 = db.CreateCommand(sql);
                if (Convert.ToInt32(dt.Rows[0]["num"].ToString()) > 0)
                    cmd1.Parameters.Add("@MID", MID);
                if (!string.IsNullOrEmpty(Company))
                    cmd1.Parameters.Add("@Company", "%" + Company + "%");
                DataTable dt_list = db.ExecuteDataTable(cmd1);
                return dt_list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("SelectWLByCHB")]
    public DataTable SelectWLByCHB(string MID, string Company)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                string sql = "select count(*) num from CompanyCHBModelChild where MID = @MID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@MID", MID);
                DataTable dt = db.ExecuteDataTable(cmd);
                string conn = "";
                if (Convert.ToInt32(dt.Rows[0]["num"].ToString()) > 0)
                    conn += " and ID not in (select CompanyModelChildId from CompanyCHBModelChild where MID = @MID)";
                if (!string.IsNullOrEmpty(Company))
                    conn += " and Name like @Company";
                sql = "select * from CompanyModel where 1=1 " + conn;
                SqlCommand cmd1 = db.CreateCommand(sql);
                if (Convert.ToInt32(dt.Rows[0]["num"].ToString()) > 0)
                    cmd1.Parameters.Add("@MID", MID);
                if (!string.IsNullOrEmpty(Company))
                    cmd1.Parameters.Add("@Company", "%" + Company + "%");
                DataTable dt_list = db.ExecuteDataTable(cmd1);
                return dt_list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("WLBind")]
    public DataTable WLBind(string MID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select * from CompanyModelChild where MID = @MID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@MID", MID);
                DataTable dt = db.ExecuteDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("WLBindByCHB")]
    public DataTable WLBindByCHB(string MID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select * from CompanyCHBModelChild where MID = @MID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@MID", MID);
                DataTable dt = db.ExecuteDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("DeleteYGLWL")]
    public bool DeleteYGLWL(string MX_ID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "delete from CompanyModelChild where MX_ID=@MX_ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@MX_ID", MX_ID);
                db.ExecuteNonQuery(cmd);

                db.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("DeleteYGLWLByCHB")]
    public bool DeleteYGLWLByCHB(string MX_ID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "delete from CompanyCHBModelChild where MX_ID=@MX_ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@MX_ID", MX_ID);
                db.ExecuteNonQuery(cmd);

                db.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("ShowYGLWLByCHB")]
    public string ShowYGLWLByCHB(string MX_ID,string mid)
    {
        using (var dbc = new DBConnection())
        {
            string sql = "select ShowCompany from CompanyCHBModelChild where MX_ID = @MX_ID and MID = @MID";
            SqlCommand cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@MX_ID", MX_ID);
            cmd.Parameters.Add("@MID", mid);
            DataTable dt = dbc.ExecuteDataTable(cmd);
            return dt.Rows[0]["ShowCompany"].ToString();
        }
    }

    [CSMethod("XGShowNameByCHB")]
    public bool XGShowNameByCHB(string MID, string MX_ID, string ShowCompany)
    {
        using (var dbc = new DBConnection())
        {
            string sql = "update CompanyCHBModelChild set ShowCompany = @ShowCompany where MX_ID = @MX_ID";
            SqlCommand cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@MX_ID", MX_ID);
            cmd.Parameters.Add("@ShowCompany", ShowCompany);
            dbc.ExecuteNonQuery(cmd);
            return true;
        }
    }

    [CSMethod("GetModelDetail")]
    public DataTable GetModelDetail(string ID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select * from CompanyModel where ID = @ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@ID", ID);
                DataTable dt = db.ExecuteDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetModelDetailByCHB")]
    public DataTable GetModelDetailByCHB(string ID)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select * from CompanyCHBModel where ID = @ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@ID", ID);
                DataTable dt = db.ExecuteDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("XGModel")]
    public bool XGModel(string ID, string Name, string ShowName)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "update CompanyModel set Name = @Name,ShowName = @ShowName where ID = @ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@ID", ID);
                cmd.Parameters.Add("@Name", Name);
                cmd.Parameters.Add("@ShowName", ShowName);
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("XGModelByCHB")]
    public bool XGModelByCHB(string ID, string Name, string ShowName)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "update CompanyCHBModel set Name = @Name,ShowName = @ShowName where ID = @ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@ID", ID);
                cmd.Parameters.Add("@Name", Name);
                cmd.Parameters.Add("@ShowName", ShowName);
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("CheckWL")]
    public bool CheckWL(string mid, string Company, string UserName)
    {
        using (var db = new DBConnection())
        {
            try
            {
                DataTable dt = db.GetEmptyDataTable("CompanyModelChild");
                DataRow dr = dt.NewRow();
                dr["MX_ID"] = Guid.NewGuid();
                dr["MID"] = mid;
                dr["UserName"] = UserName;
                dr["Company"] = Company;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("CheckWLByCHB")]
    public bool CheckWLByCHB(string mid, string Company, string modelid)
    {
        using (var db = new DBConnection())
        {
            try
            {
                DataTable dt = db.GetEmptyDataTable("CompanyCHBModelChild");
                DataRow dr = dt.NewRow();
                dr["MX_ID"] = Guid.NewGuid();
                dr["MID"] = mid;
                dr["Company"] = Company;
                dr["CompanyModelChildId"] = modelid;
                dr["ShowCompany"] = Company;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetDetailsByView")]
    public object GetDetailsByView(string ID, string UserName,string UserDenno,string year_qs,string month_qs,string day_qs,string year_dd,string month_dd,string day_dd)
    {
        using (var db = new DBConnection())
        {
            if (month_qs == "1" || month_qs == "3" || month_qs == "5" || month_qs == "7" || month_qs == "8" || month_qs == "10" || month_qs == "12")
            {

            }
            else
            {
                if (day_qs == "31" || day_qs == "30" || day_qs == "29")
                {
                    if (month_qs != "2")
                    {
                        if (day_qs == "31")
                            day_qs = "30";
                    }
                    else
                    {
                        if (Int32.Parse(year_qs) % 4 == 0)
                        {
                            day_qs = "29";
                        }
                        else
                        {
                            day_qs = "28";
                        }
                    }
                }
            }
            if (month_dd == "1" || month_dd == "3" || month_dd == "5" || month_dd == "7" || month_dd == "8" || month_dd == "10" || month_dd == "12")
            {

            }
            else
            {
                if (day_dd == "31" || day_dd == "30" || day_dd == "29")
                {
                    if (month_dd != "2")
                    {
                        if (day_dd == "31")
                            day_dd = "30";
                    }
                    else
                    {
                        if (Int32.Parse(year_dd) % 4 == 0)
                        {
                            day_dd = "29";
                        }
                        else
                        {
                            day_dd = "28";
                        }
                    }
                }
            }
            DateTime qs_date = new DateTime(Int32.Parse(year_qs), Int32.Parse(month_qs), Int32.Parse(day_qs), 0, 0, 0);
            DateTime dd_date = new DateTime(Int32.Parse(year_dd), Int32.Parse(month_dd), Int32.Parse(day_dd), 23, 59, 59);

            string sql = "select * from CompanyModel where ID = @ID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@ID", ID);
            DataTable dt_model = db.ExecuteDataTable(cmd);

            sql = "select * from CompanyModelChild where MID = @ID";
            cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@ID", ID);
            DataTable dt_child = db.ExecuteDataTable(cmd);

            string conn = " and SuoShuGongSi = '" + dt_model.Rows[0]["Name"].ToString() + "'";

            if (!string.IsNullOrEmpty(UserName))
                conn += " and a.UserID in (select UserID from [dbo].[User] where UserName = '" + UserName + "')";
            if (!string.IsNullOrEmpty(UserDenno))
                conn += " and UserDenno like '%" + UserDenno + "%'";
            conn += " and BangDingTime >= '" + qs_date + "' and BangDingTime < '" + dd_date + "'";
            sql = "select a.* from YunDan a where IsBangding = 1" + conn;
            DataTable dt = db.ExecuteDataTable(sql);
            dt.Columns.Add("jingweidu");
            dt.Columns.Add("markinfo");
            dt.Columns.Add("ZT");

            sql = "select YunDanDenno from YunDanIsArrive";
            DataTable dt_dan = db.ExecuteDataTable(sql);

            sql = "SELECT * FROM YunDanDistance WHERE YunDanDenno IN (select a.YunDanDenno from YunDan a where IsBangding = 1" + conn + ")";
            DataTable dt_yundandistance = db.ExecuteDataTable(sql);

            sql = @"select a.* from YunDan a 
                    inner join (
	                    select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                    inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                    where a.Expect_Hour is not null and a.IsBangding = 1 " + conn + @"
                    ) b on a.YunDanDenno = b.YunDanDenno
                    where a.IsBangding = 1 and TimeCZ < 0" + conn + @" order by BangDingTime desc";
            DataTable dt_yj = db.ExecuteDataTable(sql);

            int qb = 0;
            int zt = 0;
            int dd = 0;
            int fc = 0;
            int yj = dt_yj.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["Gps_lastlng"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["Gps_lastlat"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["QiShiZhan_lat"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["QiShiZhan_lng"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["DaoDaZhan_lat"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["DaoDaZhan_lng"].ToString()))
                {
                    string DaoDaZhan = dt.Rows[i]["DaoDaZhan"].ToString().Replace(" ", "");
                    string[] LastZhanArray = dt.Rows[i]["Gps_lastinfo"].ToString().Split(' ');
                    string LastZhan = "";
                    string SalePerson = "";
                    string Purchaser = "";
                    string PurchaserPerson = "";
                    string PurchaserTel = "";
                    string CarrierCompany = "";
                    string CarrierPerson = "";
                    string CarrierTel = "";
                    string DaoDaAddress = "";
                    string QiShiAddress = "";
                    string QiShiZhan_QX = string.IsNullOrEmpty(dt.Rows[i]["QiShiZhan_QX"].ToString()) ? "" : dt.Rows[i]["QiShiZhan_QX"].ToString();
                    string DaoDaZhan_QX = string.IsNullOrEmpty(dt.Rows[i]["DaoDaZhan_QX"].ToString()) ? "" : dt.Rows[i]["DaoDaZhan_QX"].ToString();

                    if (!string.IsNullOrEmpty(dt.Rows[i]["SalePerson"].ToString()))
                        SalePerson = "<p style='margin:0;font-size:13px'>销售员：" + dt.Rows[i]["SalePerson"] + "</p>";

                    //if (!string.IsNullOrEmpty(dt.Rows[i]["Purchaser"].ToString()))
                    //    Purchaser = "<p style='margin:0;font-size:13px'>收货单位：" + dt.Rows[i]["Purchaser"] + "</p>";

                    //if (!string.IsNullOrEmpty(dt.Rows[i]["PurchaserPerson"].ToString()))
                    //    PurchaserPerson = "<p style='margin:0;font-size:13px'>收货人：" + dt.Rows[i]["PurchaserPerson"] + "</p>";

                    //if (!string.IsNullOrEmpty(dt.Rows[i]["PurchaserTel"].ToString()))
                    //    PurchaserTel = "<p style='margin:0;font-size:13px'>联系方式：" + dt.Rows[i]["PurchaserTel"] + "</p>";

                    if (!string.IsNullOrEmpty(dt.Rows[i]["CarrierCompany"].ToString()))
                        CarrierCompany = "<p style='margin:0;font-size:13px'>承运公司：" + dt.Rows[i]["CarrierCompany"] + "</p>";

                    if (!string.IsNullOrEmpty(dt.Rows[i]["CarrierPerson"].ToString()))
                        CarrierPerson = "<p style='margin:0;font-size:13px'>负责人：" + dt.Rows[i]["CarrierPerson"] + "</p>";

                    if (!string.IsNullOrEmpty(dt.Rows[i]["CarrierTel"].ToString()))
                        CarrierTel = "<p style='margin:0;font-size:13px'>联系方式：" + dt.Rows[i]["CarrierTel"] + "</p>";

                    if (!string.IsNullOrEmpty(dt.Rows[i]["DaoDaAddress"].ToString()))
                        DaoDaAddress = "<p style='margin:0;font-size:13px'>目的地详细地址：" + dt.Rows[i]["DaoDaAddress"] + "</p>";

                    if (!string.IsNullOrEmpty(dt.Rows[i]["QiShiAddress"].ToString()))
                        QiShiAddress = "<p style='margin:0;font-size:13px'>出发地详细地址：" + dt.Rows[i]["QiShiAddress"] + "</p>";

                    if (LastZhanArray.Length >= 2)
                    {
                        LastZhan = LastZhanArray[0] + LastZhanArray[1];
                    }
                    if (DaoDaZhan == LastZhan)
                    {
                        dt.Rows[i]["jingweidu"] = dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString();
                        string url = "http://chb.yk56.net/Map?YunDanDenno=" + dt.Rows[i]["YunDanDenno"];
                        dt.Rows[i]["markinfo"] = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p><img class='closeX' src'' />" +
                                                 "<HR style='border:1 solid #2828FF' width='100%'>"
                                                 + "<p style='margin:0;font-size:13px'>行驶路线：" + dt.Rows[i]["QiShiZhan"] + " " + QiShiZhan_QX + ">>>" + dt.Rows[i]["DaoDaZhan"] + " " + DaoDaZhan_QX + "</p>"
                                                + "<p style='margin:0;font-size:13px'>建单公司：" + dt.Rows[i]["SuoShuGongSi"] + "</p>"
                                                + "<p style='margin:0;font-size:13px'>单号：" + dt.Rows[i]["UserDenno"] + "</p>"
                                                + SalePerson
                                                + Purchaser
                                                + PurchaserPerson
                                                + PurchaserTel
                                                + CarrierCompany
                                                + CarrierPerson
                                                + CarrierTel
                                                + DaoDaAddress
                                                + QiShiAddress
                                                + "<p style='margin:0;font-size:13px'>所在位置：" + dt.Rows[i]["Gps_lastinfo"] + "</p>"
                                                + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + dt.Rows[i]["Gps_lasttime"] + "</p>"
                                                + "<a style='margin:0;font-size:14px' href='" + url + "' target='_blank'>查看轨迹 </a>"
                                                + "</div>"
                                                + "<HR style='border:1 solid #2828FF' width='100%'>"
                                                + "<div style='margin:0 auto;background-color: #f44336;color: white; padding: 5px 10px; font-size: 16px; text-align: center; font-size:18px; font-weight:bold; cursor:pointer;' onclick='closeInfoWindow();'>关闭</div>";

                        dt.Rows[i]["ZT"] = "1";//到达
                        dd++;
                    }
                    else
                    {
                        DataRow[] drs = dt_dan.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            dt.Rows[i]["jingweidu"] = dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString();
                            string url = "http://chb.yk56.net/Map?YunDanDenno=" + dt.Rows[i]["YunDanDenno"];
                            dt.Rows[i]["markinfo"] = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p><img class='closeX' src'' />" +
                                                     "<HR style='border:1 solid #2828FF' width='100%'>"
                                                     + "<p style='margin:0;font-size:13px'>行驶路线：" + dt.Rows[i]["QiShiZhan"] + " " + QiShiZhan_QX + ">>>" + dt.Rows[i]["DaoDaZhan"] + " " + DaoDaZhan_QX + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>建单公司：" + dt.Rows[i]["SuoShuGongSi"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>单号：" + dt.Rows[i]["UserDenno"] + "</p>"
                                                    + SalePerson
                                                    + Purchaser
                                                    + PurchaserPerson
                                                    + PurchaserTel
                                                    + CarrierCompany
                                                    + CarrierPerson
                                                    + CarrierTel
                                                    + DaoDaAddress
                                                    + QiShiAddress
                                                    + "<p style='margin:0;font-size:13px'>所在位置：" + dt.Rows[i]["Gps_lastinfo"] + "</p>"
                                                    + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + dt.Rows[i]["Gps_lasttime"] + "</p>"
                                                    + "<a style='margin:0;font-size:14px' href='" + url + "' target='_blank'>查看轨迹 </a>"
                                                    + "</div>"
                                                    + "<HR style='border:1 solid #2828FF' width='100%'>"
                                                    + "<div style='margin:0 auto;background-color: #f44336;color: white; padding: 5px 10px; font-size: 16px; text-align: center; font-size:18px; font-weight:bold; cursor:pointer;' onclick='closeInfoWindow();'>关闭</div>";

                            dt.Rows[i]["ZT"] = "2";//回途
                            fc++;
                        }
                        else
                        {
                            //Hashtable ht = Route.getMapRoute(dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString(), dt.Rows[i]["DaoDaZhan_lng"].ToString() + "," + dt.Rows[i]["DaoDaZhan_lat"].ToString());
                            //double distance = GetDistance(Convert.ToDouble(dt.Rows[i]["DaoDaZhan_lat"].ToString()), Convert.ToDouble(dt.Rows[i]["Gps_lastlat"].ToString()), Convert.ToDouble(dt.Rows[i]["DaoDaZhan_lng"].ToString()), Convert.ToDouble(dt.Rows[i]["Gps_lastlng"].ToString()));

                            double distance = 0;
                            double duration = 0;
                            string distance_str = "";
                            string duration_str = "";
                            DataRow[] drs_distance = dt_yundandistance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs_distance.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(drs_distance[0]["Gps_distance"].ToString()))
                                    distance = Convert.ToDouble(drs_distance[0]["Gps_distance"]);
                                if (!string.IsNullOrEmpty(drs_distance[0]["Gps_duration"].ToString()))
                                    duration = Convert.ToDouble(drs_distance[0]["Gps_duration"]);
                            }

                            if(distance==0)
                                distance_str = "<p style='margin:0;font-size:13px'>剩余里程：暂无</p>";
                            else
                                distance_str = "<p style='margin:0;font-size:13px'>剩余里程：" + distance + "公里</p>";

                            if (duration == 0)
                                duration_str = "<p style='margin:0;font-size:13px'>剩余时间：暂无</p>";
                            else
                            {
                                int hour = 0;
                                int minute = 0;
                                if (Convert.ToInt32(duration / 60) == 0)
                                    duration_str = "<p style='margin:0;font-size:13px'>剩余时间：" + duration.ToString("F0") + "分钟</p>";
                                else
                                {
                                    hour = Convert.ToInt32(duration / 60);
                                    minute = Convert.ToInt32(duration % 60);
                                    duration_str = "<p style='margin:0;font-size:13px'>剩余时间：" + hour + "小时 " + minute + "分钟</p>";
                                }
                            }
                            dt.Rows[i]["jingweidu"] = dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString();
                            string url = "http://chb.yk56.net/Map?YunDanDenno=" + dt.Rows[i]["YunDanDenno"];
                            dt.Rows[i]["markinfo"] = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p><img class='closeX' src'' />" +
                                                     "<HR style='border:1 solid #2828FF' width='100%'>"
                                                    + "<p style='margin:0;font-size:13px'>行驶路线：" + dt.Rows[i]["QiShiZhan"] + " " + QiShiZhan_QX + ">>>" + dt.Rows[i]["DaoDaZhan"] + " " + DaoDaZhan_QX + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>建单公司：" + dt.Rows[i]["SuoShuGongSi"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>单号：" + dt.Rows[i]["UserDenno"] + "</p>"
                                                    + SalePerson
                                                    + Purchaser
                                                    + PurchaserPerson
                                                    + PurchaserTel
                                                    + CarrierCompany
                                                    + CarrierPerson
                                                    + CarrierTel
                                                    + DaoDaAddress
                                                    + QiShiAddress
                                                    + "<p style='margin:0;font-size:13px'>所在位置：" + dt.Rows[i]["Gps_lastinfo"] + "</p>"
                                                    + distance_str
                                                    + duration_str
                                                    + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + dt.Rows[i]["Gps_lasttime"] + "</p>"
                                                    + "<a style='margin:0;font-size:14px' href='" + url + "' target='_blank'>查看轨迹 </a>"
                                                    + "</div>"
                                                    + "<HR style='border:1 solid #2828FF' width='100%'>"
                                                    + "<div style='margin:0 auto;background-color: #f44336;color: white; padding: 5px 10px; font-size: 16px; text-align: center; font-size:18px; font-weight:bold; cursor:pointer;' onclick='closeInfoWindow();'>关闭</div>";

                            DataRow[] drs_yj = dt_yj.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"] + "'");
                            if (drs_yj.Length > 0)
                            {
                                dt.Rows[i]["ZT"] = "4";//预警
                                yj++;
                            }
                            else
                            {
                                dt.Rows[i]["ZT"] = "0";//在途
                            }
                            zt++;
                        }
                    }
                }
            }

            qb = zt + dd + fc;

            return new { dt_model = dt_model, dt_child = dt_child, dt = dt, zt = zt, dd = dd, fc = fc, qb = qb, yj = yj };
        }
    }


    [CSMethod("GetDetailsByViewByCHB")]
    public object GetDetailsByViewByCHB(string ID, string CompanyModelChildId,string SuoShuGongSi, string UserDenno, string year_qs, string month_qs, string day_qs, string year_dd, string month_dd, string day_dd)
    {
        using (var db = new DBConnection())
        {
            if (month_qs == "1" || month_qs == "3" || month_qs == "5" || month_qs == "7" || month_qs == "8" || month_qs == "10" || month_qs == "12")
            {

            }
            else
            {
                if (day_qs == "31" || day_qs == "30" || day_qs == "29")
                {
                    if (month_qs != "2")
                    {
                        if (day_qs == "31")
                            day_qs = "30";
                    }
                    else
                    {
                        if (Int32.Parse(year_qs) % 4 == 0)
                        {
                            day_qs = "29";
                        }
                        else
                        {
                            day_qs = "28";
                        }
                    }
                }
            }
            if (month_dd == "1" || month_dd == "3" || month_dd == "5" || month_dd == "7" || month_dd == "8" || month_dd == "10" || month_dd == "12")
            {

            }
            else
            {
                if (day_dd == "31" || day_dd == "30" || day_dd == "29")
                {
                    if (month_dd != "2")
                    {
                        if (day_dd == "31")
                            day_dd = "30";
                    }
                    else
                    {
                        if (Int32.Parse(year_dd) % 4 == 0)
                        {
                            day_dd = "29";
                        }
                        else
                        {
                            day_dd = "28";
                        }
                    }
                }
            }
            DateTime qs_date = new DateTime(Int32.Parse(year_qs), Int32.Parse(month_qs), Int32.Parse(day_qs), 0, 0, 0);
            DateTime dd_date = new DateTime(Int32.Parse(year_dd), Int32.Parse(month_dd), Int32.Parse(day_dd), 23, 59, 59);

            string sql = "select * from CompanyCHBModel where ID = @ID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@ID", ID);
            DataTable dt_model = db.ExecuteDataTable(cmd);

            sql = "select * from CompanyCHBModelChild where MID = @ID";
            cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@ID", ID);
            DataTable dt_child = db.ExecuteDataTable(cmd);

            string conn = "";
            if (!string.IsNullOrEmpty(CompanyModelChildId))
                conn += " and a.UserID in (select UserID from [dbo].[User] where UserName in (select UserName from CompanyModelChild where MID = '" + CompanyModelChildId + "'))";
            if (!string.IsNullOrEmpty(UserDenno))
                conn += " and UserDenno like '%" + UserDenno + "%'";
            if (!string.IsNullOrEmpty(SuoShuGongSi))
                conn += " and SuoShuGongSi = '" + SuoShuGongSi + "'";
            conn += " and BangDingTime >= '" + qs_date + "' and BangDingTime < '" + dd_date + "'";
            sql = "select a.* from YunDan a where IsBangding = 1 " + conn;
            DataTable dt = db.ExecuteDataTable(sql);
            dt.Columns.Add("jingweidu");
            dt.Columns.Add("markinfo");
            dt.Columns.Add("ZT");

            sql = "select YunDanDenno from YunDanIsArrive";
            DataTable dt_dan = db.ExecuteDataTable(sql);

            sql = "SELECT * FROM YunDanDistance WHERE YunDanDenno IN (select a.YunDanDenno from YunDan a where IsBangding = 1 " + conn + ")";
            DataTable dt_yundandistance = db.ExecuteDataTable(sql);

            sql = @"select a.* from YunDan a 
                    inner join (
	                    select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                    inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                    where a.Expect_Hour is not null and a.IsBangding = 1 " + conn + @"
                    ) b on a.YunDanDenno = b.YunDanDenno
                    where a.IsBangding = 1 and TimeCZ < 0" + conn + @" order by BangDingTime desc";
            DataTable dt_yj = db.ExecuteDataTable(sql);

            int qb = 0;
            int zt = 0;
            int dd = 0;
            int fc = 0;
            int yj = dt_yj.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["Gps_lastlng"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["Gps_lastlat"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["QiShiZhan_lat"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["QiShiZhan_lng"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["DaoDaZhan_lat"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["DaoDaZhan_lng"].ToString()))
                {
                    string DaoDaZhan = dt.Rows[i]["DaoDaZhan"].ToString().Replace(" ", "");
                    string[] LastZhanArray = dt.Rows[i]["Gps_lastinfo"].ToString().Split(' ');
                    string LastZhan = "";
                    if (LastZhanArray.Length >= 2)
                    {
                        LastZhan = LastZhanArray[0] + LastZhanArray[1];
                    }
                    if (DaoDaZhan == LastZhan)
                    {
                        dt.Rows[i]["jingweidu"] = dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString();
                        string url = "http://chb.yk56.net/Map?YunDanDenno=" + dt.Rows[i]["YunDanDenno"];
                        dt.Rows[i]["markinfo"] = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p><img class='closeX' src'' />" +
                                                 "<HR style='border:1 solid #2828FF' width='100%'>"
                                                 + "<p style='margin:0;font-size:13px'>行驶路线：" + dt.Rows[i]["QiShiZhan"] + ">>>" + dt.Rows[i]["DaoDaZhan"] + "</p>"
                                                + "<p style='margin:0;font-size:13px'>建单公司：" + dt.Rows[i]["SuoShuGongSi"] + "</p>"
                                                + "<p style='margin:0;font-size:13px'>单号：" + dt.Rows[i]["UserDenno"] + "</p>"
                                                + "<p style='margin:0;font-size:13px'>所在位置：" + dt.Rows[i]["Gps_lastinfo"] + "</p>"
                                                + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + dt.Rows[i]["Gps_lasttime"] + "</p>"
                                                + "<a style='margin:0;font-size:14px' href='" + url + "' target='_blank'>查看轨迹 </a>"
                                                + "</div>"
                                                + "<HR style='border:1 solid #2828FF' width='100%'>"
                                                + "<div style='margin:0 auto;background-color: #f44336;color: white; padding: 5px 10px; font-size: 16px; text-align: center; font-size:18px; font-weight:bold; cursor:pointer;' onclick='closeInfoWindow();'>关闭</div>";

                        dt.Rows[i]["ZT"] = "1";//到达
                        dd++;
                    }
                    else
                    {
                        DataRow[] drs = dt_dan.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            dt.Rows[i]["jingweidu"] = dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString();
                            string url = "http://chb.yk56.net/Map?YunDanDenno=" + dt.Rows[i]["YunDanDenno"];
                            dt.Rows[i]["markinfo"] = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p><img class='closeX' src'' />" +
                                                     "<HR style='border:1 solid #2828FF' width='100%'>"
                                                     + "<p style='margin:0;font-size:13px'>行驶路线：" + dt.Rows[i]["QiShiZhan"] + ">>>" + dt.Rows[i]["DaoDaZhan"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>建单公司：" + dt.Rows[i]["SuoShuGongSi"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>单号：" + dt.Rows[i]["UserDenno"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>所在位置：" + dt.Rows[i]["Gps_lastinfo"] + "</p>"
                                                    + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + dt.Rows[i]["Gps_lasttime"] + "</p>"
                                                    + "<a style='margin:0;font-size:14px' href='" + url + "' target='_blank'>查看轨迹 </a>"
                                                    + "</div>"
                                                    + "<HR style='border:1 solid #2828FF' width='100%'>"
                                                    + "<div style='margin:0 auto;background-color: #f44336;color: white; padding: 5px 10px; font-size: 16px; text-align: center; font-size:18px; font-weight:bold; cursor:pointer;' onclick='closeInfoWindow();'>关闭</div>";

                            dt.Rows[i]["ZT"] = "2";//回途
                            fc++;
                        }
                        else
                        {
                            //Hashtable ht = Route.getMapRoute(dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString(), dt.Rows[i]["DaoDaZhan_lng"].ToString() + "," + dt.Rows[i]["DaoDaZhan_lat"].ToString());
                            //double distance = GetDistance(Convert.ToDouble(dt.Rows[i]["DaoDaZhan_lat"].ToString()), Convert.ToDouble(dt.Rows[i]["Gps_lastlat"].ToString()), Convert.ToDouble(dt.Rows[i]["DaoDaZhan_lng"].ToString()), Convert.ToDouble(dt.Rows[i]["Gps_lastlng"].ToString()));

                            double distance = 0;
                            double duration = 0;
                            string distance_str = "";
                            string duration_str = "";
                            DataRow[] drs_distance = dt_yundandistance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs_distance.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(drs_distance[0]["Gps_distance"].ToString()))
                                    distance = Convert.ToDouble(drs_distance[0]["Gps_distance"]);
                                if (!string.IsNullOrEmpty(drs_distance[0]["Gps_duration"].ToString()))
                                    duration = Convert.ToDouble(drs_distance[0]["Gps_duration"]);
                            }

                            if (distance == 0)
                                distance_str = "<p style='margin:0;font-size:13px'>剩余里程：暂无</p>";
                            else
                                distance_str = "<p style='margin:0;font-size:13px'>剩余里程：" + distance + "公里</p>";

                            if (duration == 0)
                                duration_str = "<p style='margin:0;font-size:13px'>剩余时间：暂无</p>";
                            else
                            {
                                int hour = 0;
                                int minute = 0;
                                if (Convert.ToInt32(duration / 60) == 0)
                                    duration_str = "<p style='margin:0;font-size:13px'>剩余时间：" + duration.ToString("F0") + "分钟</p>";
                                else
                                {
                                    hour = Convert.ToInt32(duration / 60);
                                    minute = Convert.ToInt32(duration % 60);
                                    duration_str = "<p style='margin:0;font-size:13px'>剩余时间：" + hour + "小时 " + minute + "分钟</p>";
                                }
                            }
                            dt.Rows[i]["jingweidu"] = dt.Rows[i]["Gps_lastlng"].ToString() + "," + dt.Rows[i]["Gps_lastlat"].ToString();
                            string url = "http://chb.yk56.net/Map?YunDanDenno=" + dt.Rows[i]["YunDanDenno"];
                            dt.Rows[i]["markinfo"] = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p><img class='closeX' src'' />" +
                                                     "<HR style='border:1 solid #2828FF' width='100%'>"
                                                     + "<p style='margin:0;font-size:13px'>行驶路线：" + dt.Rows[i]["QiShiZhan"] + ">>>" + dt.Rows[i]["DaoDaZhan"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>建单公司：" + dt.Rows[i]["SuoShuGongSi"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>单号：" + dt.Rows[i]["UserDenno"] + "</p>"
                                                    + "<p style='margin:0;font-size:13px'>所在位置：" + dt.Rows[i]["Gps_lastinfo"] + "</p>"
                                                    + distance_str
                                                    + duration_str
                                                    + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + dt.Rows[i]["Gps_lasttime"] + "</p>"
                                                    + "<a style='margin:0;font-size:14px' href='" + url + "' target='_blank'>查看轨迹 </a>"
                                                    + "</div>"
                                                    + "<HR style='border:1 solid #2828FF' width='100%'>"
                                                    + "<div style='margin:0 auto;background-color: #f44336;color: white; padding: 5px 10px; font-size: 16px; text-align: center; font-size:18px; font-weight:bold; cursor:pointer;' onclick='closeInfoWindow();'>关闭</div>";

                            DataRow[] drs_yj = dt_yj.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"] + "'");
                            if (drs_yj.Length > 0)
                            {
                                dt.Rows[i]["ZT"] = "4";//预警
                                yj++;
                            }
                            else
                            {
                                dt.Rows[i]["ZT"] = "0";//在途
                            }
                            zt++;
                        }
                    }
                }
            }

            qb = zt + dd + fc;

            return new { dt_model = dt_model, dt_child = dt_child, dt = dt, zt = zt, dd = dd, fc = fc, qb = qb, yj = yj };
        }
    }

    //地球半径，单位米
    private const double EARTH_RADIUS = 6378137;

    /// <summary>
    /// 计算两点位置的距离，返回两点的距离，单位：米
    /// 该公式为GOOGLE提供，误差小于0.2米
    /// </summary>
    /// <param name="lng1">第一点经度</param>
    /// <param name="lat1">第一点纬度</param>        
    /// <param name="lng2">第二点经度</param>
    /// <param name="lat2">第二点纬度</param>
    /// <returns></returns>
    public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
    {
        double radLat1 = Rad(lat1);
        double radLng1 = Rad(lng1);
        double radLat2 = Rad(lat2);
        double radLng2 = Rad(lng2);
        double a = radLat1 - radLat2;
        double b = radLng1 - radLng2;
        double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
        return result;
    }

    /// <summary>
    /// 经纬度转化成弧度
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    private static double Rad(double d)
    {
        return (double)d * Math.PI / 180d;
    }
}