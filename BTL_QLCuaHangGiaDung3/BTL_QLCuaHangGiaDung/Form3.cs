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
    public partial class Form3 : Form
    {

        double tongtien = 0;
      

        void loaddataHoaDon()
        {
            DataTable dt = new DataTable();
            String Query = "select * from v_HDMH";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            dtg_HoaDon.DataSource = dt;
        }
        void loaddataSP()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            String Query = "select * from SanPham";
            String Query1 = "select * from v_SP";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            dt1=conn.getTable(Query1);
            dtg_SP.DataSource = dt1;

            //DisplayMember chứa cột trong bảng dữ liệu hiển thị cho mình thấy
            //valumeMember chứa cột trong bảng dữ liệu khi lựa chọn ở combobox
            tenSP.DisplayMember = "Ten_SP";
            tenSP.ValueMember = "ID_SP";
            tenSP.DataSource = dt;
        }
        void loaddataCTHD()
        {
            DataTable dt = new DataTable();
            String Query = "select * from CTHD";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
        }
        public Form3()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index;
            maHD.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            maSP.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            tenSP.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            soluongSP.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            maKH.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            dt_ngayHD.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            tongtienHD.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();

        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            loaddataHoaDon();
            loaddataSP();
            loaddataCTHD();
        }
        private void tenSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            maSP.Text = tenSP.SelectedValue.ToString();
            DataTable dt = new DataTable();
            String Query = "select * from SanPham where ID_SP = '"+tenSP.SelectedValue+"'";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            // hiện đơn giá sp 
            dgSP.Text = dt.Rows[0]["DonGiaBan"].ToString();

        }
        // nút thanh toán
        private void btn_thanhtoan_Click(object sender, EventArgs e)
        {
            try
            {
                maHD.ReadOnly = true;
                dt_ngayHD.Enabled = false;
                maKH.ReadOnly = true;
                tenKH.ReadOnly = true;
                diachiKH.ReadOnly = true;
                sdtKH.ReadOnly = true;
                gioitinhKH.Enabled = false;
                tenSP.Enabled = false;
                soluongSP.Enabled = false;
                btn_them.Enabled = false;
                btn_xoa.Enabled = false;
                btn_thanhtoan.Enabled = false;


                tongtienHD.Text = Convert.ToString(tongtien);
                connectDB conn = new connectDB();
                DataTable dt = new DataTable();
                String Query = "select * from KhachHang where ID_KH = '" + maKH.Text + "'";
                dt = conn.getTable(Query);
                // nếu không trùng mã hóa đơn thì sẽ insert , trùng thì sẽ chạy lệnh insert hóa đơn bỏ qua insert khách
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into KhachHang values('" + maKH.Text + "',N'" + tenKH.Text + "',N'" + sdtKH.Text + "',N'" + diachiKH.Text + "',N'" + gioitinhKH.Text + "')";
                    conn.ExcuteNonQuery(query);
                }

                string query1 = "Insert into HoaDon (ID_HD , ID_KH , Ngay_HD) values('" + maHD.Text + "',N'" + maKH.Text + "',N'" + dt_ngayHD.Value + "')";
                conn.ExcuteNonQuery(query1);
                //foreach (DataGridView row in dataGridView1.DataSource)
                //cập nhập vào chi tiết hóa đơn bằng datagridview
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string value1 = "Insert into CTHD(ID_HD , ID_SP , SL_CTHD) values('" + dataGridView1.Rows[i].Cells[0].Value + "','" + dataGridView1.Rows[i].Cells[1].Value + "'," + dataGridView1.Rows[i].Cells[3].Value + ")";
                    conn.ExcuteNonQuery(value1);
                    //update số lượng sản phẩm còn lại trong kho
                    //string value2 = "update SP set SL_N = SL_N - " + dataGridView1.Rows[i].Cells[3].Value + " where Ma_SP = '" + dataGridView1.Rows[i].Cells[1].Value + "'";
                    //conn.ExcuteNonQuery(value2);
                }
                // load lại cả form để refresh tất cả datagridview
                MessageBox.Show("Đã thanh toán !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi thêm hóa đơn!");
            }
            Form2_Load(sender, e);
        }

        //nút thêm
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ( maHD.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã hóa đơn!");
                }
                else if ( maKH.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã khách hàng");
                }
                else if (maSP.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã sản phẩm");
                }
                else
                {
                    string ngay = dt_ngayHD.Value.ToShortDateString();
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = maHD.Text;
                    row.Cells[1].Value = maSP.Text;
                    row.Cells[2].Value = tenSP.Text;
                    row.Cells[3].Value = soluongSP.Value;
                    row.Cells[4].Value = maKH.Text;
                    row.Cells[5].Value = ngay.ToString();
                    //Lỗi try catch khi ko nhập mã SP vì sl * DGSP
                    row.Cells[6].Value = soluongSP.Value * Convert.ToInt64(dgSP.Text);
                    dataGridView1.Rows.Add(row);



                    maHD.ReadOnly = true;
                    dt_ngayHD.Enabled = false;
                    maKH.ReadOnly = true;
                    tenKH.ReadOnly = true;
                    diachiKH.ReadOnly = true;
                    sdtKH.ReadOnly = true;
                    gioitinhKH.Enabled = false;
                    soluongSP.Value = 1;

                    tongtien += Convert.ToDouble(row.Cells[6].Value);
                }
                //hiển thị trên dtgridview
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi");
            }

        }
        //nút xóa
        private void btn_sua_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell oneCell in dataGridView1.SelectedCells)
            {
                if (oneCell.Selected)
                    dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
            }
        }
        private void ResetValues()
        {
            maHD.Text = "";
            dt_ngayHD.Text = DateTime.Now.ToShortDateString();
            maKH.Text = "";
            tenKH.Text = "";
            diachiKH.Text = "";
            sdtKH.Text = "";
            gioitinhKH.Text = "Nam";
            tenSP.Text = "";
            maSP.Text = "";
            dgSP.Text = "";
            soluongSP.Value = 1;
            tongtienHD.Text = "";
            dataGridView1.Rows.Clear();
        }
        //dtg_NCC
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }
        //Nguồn hàng
        private void button2_Click(object sender, EventArgs e)
        {
    
        }
        private void btn_suaNCC_Click(object sender, EventArgs e)
        {
         
        }
        private void dtg_NV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        // thêm SP
      

        private void button5_Click(object sender, EventArgs e)
        {
          
        }
        // xóa SP
      
        private void button6_Click(object sender, EventArgs e)
        {
        }
       
        private void btn_suaNV_Click(object sender, EventArgs e)
        {
           
        }

        
        private void thôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tenSP_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void tongtienSP_TextChanged(object sender, EventArgs e)
        {
            
        }
        


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void maSP_TextChanged(object sender, EventArgs e)
        {

        }
        // nút làm mới 
        private void button1_Click_1(object sender, EventArgs e)
        {
            maHD.ReadOnly = false;
            dt_ngayHD.Enabled = true;
            maKH.ReadOnly =false;
            tenKH.ReadOnly = false;
            diachiKH.ReadOnly = false;
            sdtKH.ReadOnly = false;
            gioitinhKH.Enabled = true;
            tenSP.Enabled = true;
            soluongSP.Enabled = true ;
            btn_them.Enabled = true;
            btn_xoa.Enabled = true;
            btn_thanhtoan.Enabled = true;
            ResetValues();
        }

        private void dtg_KhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dt_ngayHD_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tb_mancc1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TenNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
     
        }

        private void btn_xoaSP_Click(object sender, EventArgs e)
        {
           
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void quanLyCuaHangDoGiaDungDataSetBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dtg_KhachHang_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btn_tktk_Click(object sender, EventArgs e)
        {
        }

        private void btn_tkmk_Click(object sender, EventArgs e)
        {
            if (txt_tktsp.Text == "")
            {
                MessageBox.Show("Bạn phải nhập tên sản phẩm để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_tktsp.Focus();
                return;
            }
            maHD.Text = txt_tktsp.Text;
            DataTable dt = new DataTable();
            String Query = "EXEC sp_TKHD3 N'" + txt_tktsp.Text + "' ;";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            if (dt.Rows.Count > 0)
            {
                dtgtimkiem.DataSource = dt;
                txt_tktk.Text = "";
            }
            else MessageBox.Show("Không có kết quả cho tên sản phẩm bạn tìm kiếm!", "Thông báo", MessageBoxButtons.OK);
        }

        private void btn_suakh_Click(object sender, EventArgs e)
        {
        }

        private void dtg_TK_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
       
        private void bt_ttk_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
           
        }

        private void bt_xtk_Click(object sender, EventArgs e)
        {
        
        }
    }
}

