using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace BTL_QLCuaHangGiaDung
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public static string layTaiKhoan = "";
        private void button1_Click(object sender, EventArgs e)
        {
            string str = @"Data Source=DESKTOP-G4NLQSF;Initial Catalog=QuanLyCuaHangDoGiaDung;Integrated Security=True";
            try
            {
                SqlConnection con = new SqlConnection(str);
                con.Open();
                string query1 = "execute sp_LoginA " + tb_dangnhap.Text + ", " + tb_matkhau.Text + ";" ;
                SqlCommand cmd1 = new SqlCommand(query1, con);
                SqlDataAdapter damt1 = new SqlDataAdapter(query1, con);
                DataSet ds1 = new DataSet();
                damt1.Fill(ds1, "INFO");
                DataTable dt1 = ds1.Tables[0];
                string query2 = "execute sp_LoginB " + tb_dangnhap.Text + ", " + tb_matkhau.Text + ";";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                SqlDataAdapter damt2 = new SqlDataAdapter(query2, con);
                DataSet ds2 = new DataSet();
                damt2.Fill(ds2, "INFO");
                DataTable dt2 = ds2.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK);
                    layTaiKhoan = tb_dangnhap.Text;
                    Form2 form2 = new Form2();
                    form2.Show();

                }
                else if (dt2.Rows.Count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK);
                    Form3 form3 = new Form3();
                    form3.Show();
                }
                else
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception)
            {
                MessageBox.Show("Không có kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
