using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FullCalendar.Models {
    public class DBConnection {

        private SqlConnection connection = null;
        private string connectionString = @"Data Source={SQLSUNUCUNUZ};Initial Catalog=FullCalendar;User ID={SQLKULLANICINIZ};Password={SQLPAROLANIZ};";

        /// <summary>
        /// Sql Bağlantısını açar
        /// </summary>
        public void OpenConnection() {
            connection = new SqlConnection(connectionString);

            if (connection.State != ConnectionState.Open) {
                connection.Open();
                //RunSqlCommand("set dateformat dmy");
            }
        }
        /// <summary>
        /// Sql Bağlantısını kapatır
        /// </summary>
        public void CloseConnection() {
            if (connection != null && connection.State != ConnectionState.Closed) {
                connection.Close();
                connection = null;
            }
        }

        /// <summary>
        /// Verilen sql cümlesiyle DataTable döndürür
        /// </summary>
        /// <param name="sql">Sql Cümlesi</param>
        /// <param name="param">Parametreler</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, List<SqlParameter> param = null) {
            try {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                if (param != null)
                    cmd.Parameters.AddRange(param.ToArray());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cmd.Dispose();
                adapter.Dispose();
                return dt;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Verilen sql cümlesini execute eder 
        /// </summary>
        /// <param name="sql">Sql Cümlesi</param>
        /// <param name="param">parametreler</param>
        public void RunSqlCommand(string sql, List<SqlParameter> param = null) {
            try {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                if (param != null)
                    cmd.Parameters.AddRange(param.ToArray());
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}