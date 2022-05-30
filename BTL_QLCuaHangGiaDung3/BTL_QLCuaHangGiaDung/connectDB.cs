using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace BTL_QLCuaHangGiaDung
{
    class connectDB
    {
        //kết nối sql
        public SqlConnection getConnect()
        {
            String strConn = @"Data Source=DESKTOP-G4NLQSF;Initial Catalog=QuanLyCuaHangDoGiaDung;Integrated Security=True";
            return new SqlConnection(strConn);
        }
        // điền dữ liệu vào bảng
        public DataTable getTable(String sql)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = getConnect();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        // thực hiện lệnh truy vấn
        public void ExcuteNonQuery(String sql)
        {
            SqlConnection conn = getConnect();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd.Clone();
            conn.Close();
        }
    }
}
