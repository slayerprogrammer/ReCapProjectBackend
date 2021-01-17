using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FullCalendar.Models;

namespace FullCalendar.Controllers {
    public class CalendarController : Controller {
        //
        // GET: /Calendar/

        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// Veritabanındaki kayıtları takvime doldurur
        /// </summary>
        /// <param name="start">Başlangıç tarihi parametresi</param>
        /// <param name="end">Bitiş tarihi parametresi</param>
        /// <returns></returns>
        public JsonResult GetCalendarEvents(string start, string end) {
            List<CalendarEvent> eventItems = new List<CalendarEvent>();

            DBConnection connect = new DBConnection();

            try {
                connect.OpenConnection();

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@StartDate", start));
                param.Add(new SqlParameter("@EndDate", end));

                DataTable dt = connect.GetDataTable("Select * from tCalendar Where StartDate>=@StartDate and EndDate<=@EndDate", param);

                int i = 0, n = dt.Rows.Count;
                for (i = 0; i < n; i++) {
                    DataRow dr = dt.Rows[i];

                    CalendarEvent item = new CalendarEvent();
                    item.id = int.Parse(dr["IDCalendar"].ToString());
                    item.title = dr["Title"].ToString();
                    item.start = string.Format("{0:s}", dr["StartDate"]);
                    item.end = string.Format("{0:s}", dr["EndDate"]);
                    item.color = dr["Color"].ToString();
                    item.allDay = bool.Parse(dr["AllDay"].ToString());

                    eventItems.Add(item);
                }

                return Json(eventItems, JsonRequestBehavior.AllowGet);
            }
            finally {
                connect.CloseConnection();
            }
        }

        /// <summary>
        /// Veritabanına Ekleme yapar veya varolan kayıtta güncelleme yapar
        /// </summary>
        /// <param name="item">İşlem yapılması istenen event nesnesi</param>
        /// <returns></returns>
        public JsonResult AddOrEditItem(CalendarEvent item) {
            DBConnection connect = new DBConnection();
            try {
                connect.OpenConnection();
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Title", item.title));
                param.Add(new SqlParameter("@StartDate", item.start));
                param.Add(new SqlParameter("@EndDate", item.end));
                param.Add(new SqlParameter("@Color", item.color));
                param.Add(new SqlParameter("@AllDay", item.allDay));

                if (item.id == 0) {
                    string sql = "Insert into tCalendar(Title,StartDate,EndDate,Color,AllDay) ";
                    sql += "Values(@Title,@StartDate,@EndDate,@Color,@AllDay) ";

                    connect.RunSqlCommand(sql, param);
                }
                else {
                    string sql = "Update tCalendar Set Title=@Title,StartDate=@StartDate,EndDate=@EndDate,Color=@Color,AllDay=@AllDay ";
                    sql += "Where IDCalendar=@IDCalendar ";

                    param.Add(new SqlParameter("@IDCalendar", item.id));

                    connect.RunSqlCommand(sql, param);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            finally {
                connect.CloseConnection();
            }
        }

        /// <summary>
        /// Veritabanından kayıt siler
        /// </summary>
        /// <param name="id">Silinmesi istenilen kaydın id değeri</param>
        /// <returns></returns>
        public JsonResult DeleteItem(int id) {
            DBConnection connect = new DBConnection();
            try {
                connect.OpenConnection();
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@IDCalendar", id));

                string sql = "Delete tCalendar Where IDCalendar=@IDCalendar";

                connect.RunSqlCommand(sql, param);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            finally {
                connect.CloseConnection();
            }

        }

        /// <summary>
        /// Sürükle bırak işlemiyle tarih bilgilerini güncelleme
        /// </summary>
        /// <param name="id">ilgili kaydın id değeri</param>
        /// <param name="start">başlangıç tarihi değeri</param>
        /// <param name="end">bitiş tarihi değeri</param>
        /// <returns></returns>
        public JsonResult UpdateItemDate(int id, string start, string end) {
            DBConnection connect = new DBConnection();
            try {
                connect.OpenConnection();
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@IDCalendar", id));
                param.Add(new SqlParameter("@StartDate", start));
                param.Add(new SqlParameter("@EndDate", end));

                string sql = "Update tCalendar Set StartDate=@StartDate, EndDate=@EndDate Where IDCalendar=@IDCalendar";

                connect.RunSqlCommand(sql, param);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            finally {
                connect.CloseConnection();
            }

        }

    }
}
