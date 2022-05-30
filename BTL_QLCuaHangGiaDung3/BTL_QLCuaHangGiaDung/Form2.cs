using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BTL_QLCuaHangGiaDung
{

    public partial class Form2 : Form
    {
        string quyen = Form1.layTaiKhoan;

        double tongtien = 0;
       
        void loaddataNCC()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            String Query = "select * from NCC";
            String Query1 = "select * from v_NCC";
            connectDB conn = new connectDB(); 
            dt = conn.getTable(Query);
            dt1 = conn.getTable(Query1);
            dtg_NCC.DataSource = dt1;

            TenNCC.DisplayMember = "Ten_NCC";
            TenNCC.ValueMember = "ID_NCC";
            TenNCC.DataSource = dt;
        }
        
        void loaddataKhachHang()
        {
            DataTable dt = new DataTable();
            String Query = "select * from v_KH";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            dtg_KhachHang.DataSource = dt;
        }

        void loaddataTaiKhoan()
        {
            DataTable dt = new DataTable();
            String Query = "select * from v_TK";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            dtg_TK.DataSource = dt;
        }

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
        public Form2()
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
            loaddataKhachHang();
            loaddataNCC();
            loaddataSP();
            loaddataCTHD();
            loaddataTaiKhoan();
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

            int i = dtg_NCC.CurrentRow.Index;
            tb_mancc.Text = dtg_NCC.Rows[i].Cells[0].Value.ToString();
            tb_tenncc.Text = dtg_NCC.Rows[i].Cells[1].Value.ToString();
            tb_diachincc.Text = dtg_NCC.Rows[i].Cells[2].Value.ToString();
            tb_dtncc.Text = dtg_NCC.Rows[i].Cells[3].Value.ToString();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }
        //Nguồn hàng
        void themNCC()
        {
            try
            {
                if (tb_mancc.Text != "")
                {
                    string query = "Insert into NCC (ID_NCC , Ten_NCC , DC_NCC , DT_NCC ) values('" + tb_mancc.Text + "',N'" + tb_tenncc.Text + "',N'" + tb_diachincc.Text + "','" + tb_dtncc.Text + "')";
                    connectDB conn = new connectDB();
                    conn.ExcuteNonQuery(query);
                    MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK);
                    tb_mancc.Text = "";
                    tb_tenncc.Text = "";
                    tb_diachincc.Text = "";
                    tb_dtncc.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Thông tin không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if(tb_mancc.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã nhà cung cấp");
                }
                else {
                    DialogResult dlr = MessageBox.Show("Bạn muốn thêm nhà cung cấp?",
               "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        themNCC();
                        loaddataNCC();
                        
                    }
                }
            
            }
            catch(Exception)
            {
                MessageBox.Show("Thông tin trùng lặp");
            }
        }
        void xoaNCC()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from NCC where ID_NCC = N'" + tb_mancc.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể xóa nhà cung cấp không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string query = "Delete from NCC where ID_NCC = '" + tb_mancc.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void btn_xoaNCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_mancc.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã nhà cung cấp");
                }
                else
                {
                    DialogResult dlr = MessageBox.Show("Bạn muốn ngừng giao dịch với nhà cung cấp?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        xoaNCC();
                        loaddataNCC();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn không có quyền xóa","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        void suaNCC()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from NCC where ID_NCC = N'" + tb_mancc.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể sửa nhà cung cấp không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string query = "Update NCC set Ten_NCC = N'" + tb_tenncc.Text + "',DC_NCC = N'" + tb_diachincc.Text + "',DT_NCC = '" + tb_dtncc.Text + "' where ID_NCC = '" + tb_mancc.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK);
            }

        }
        private void btn_suaNCC_Click(object sender, EventArgs e)
        {
            if (tb_mancc.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã nhà cung cấp");
            }
            else
            {
                DialogResult dlr = MessageBox.Show("Bạn muốn sửa nhà cung cấp?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                {
                    suaNCC();
                    loaddataNCC();
                    
                }
            }
        }
        private void dtg_NV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dtg_SP.CurrentRow.Index;
            tb_maSP.Text = dtg_SP.Rows[i].Cells[0].Value.ToString();
            TenNCC.Text = dtg_SP.Rows[i].Cells[1].Value.ToString();
            tb_tenSP.Text = dtg_SP.Rows[i].Cells[2].Value.ToString();
            nb_dgn.Text = dtg_SP.Rows[i].Cells[3].Value.ToString();
            nb_slSP.Text = dtg_SP.Rows[i].Cells[4].Value.ToString();
            nb_dgb.Text = dtg_SP.Rows[i].Cells[5].Value.ToString();
            tb_mota.Text = dtg_SP.Rows[i].Cells[6].Value.ToString();
        }
        // thêm SP
        void themSP()
        {
            string query = "Insert into SanPham (ID_SP,ID_NCC,Ten_SP,DGN_SP,SL_SP,DonGiaBan,MT_SP) values('" + tb_maSP.Text + "','" + tb_mancc1.Text + "', N'" + tb_tenSP.Text + "','" + nb_dgn.Value + "','" + nb_slSP.Value + "','" + nb_dgb.Value + "',N'" + tb_mota.Text + "')";
            connectDB con = new connectDB();
            con.ExcuteNonQuery(query);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_maSP.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã sản phẩm");
                }
                else if (tb_mancc1.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã nhà cung cấp");
                }
                else {
                    DialogResult dlr = MessageBox.Show("Bạn muốn thêm sản phẩm?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        themSP();
                        loaddataSP();
                        MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK);
                        tb_maSP.Text = "";
                        TenNCC.Text = "";
                        tb_tenSP.Text = "";
                        nb_dgn.Text = "";
                        nb_slSP.Text = "";
                        nb_dgb.Text = "";
                        tb_mota.Text = "";

                    }
                }    
            }
            catch (Exception)
            {
                MessageBox.Show("Thông tin không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // xóa SP
        void xoaSP()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from SanPham where ID_SP = N'" + tb_maSP.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể xóa sản phẩm không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string query = "Delete from SanPham where ID_SP = '" + tb_maSP.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK);
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
        }
        void suaSP()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from SanPham where ID_SP = N'" + tb_maSP.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể sửa sản phẩm không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string query = "Update SanPham set ID_NCC = " + tb_mancc1.Text + ",Ten_SP = N'" + tb_tenSP.Text + "', DGN_SP = " + nb_dgn.Value + ", SL_SP = " + nb_slSP.Value + " , DonGiaBan = " + nb_dgb.Value + " , MT_SP = N'" + tb_mota.Text + "' where ID_SP = '" + tb_maSP.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK);
            }
          
        }
        private void btn_suaNV_Click(object sender, EventArgs e)
        {
            if (tb_maSP.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã sản phẩm");
            }
            else if (tb_mancc1.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã nhà cung cấp");
            }
            else
            {
                DialogResult dlr = MessageBox.Show("Bạn muốn sửa sản phẩm?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                {
                    suaSP();
                    loaddataSP();
                    loaddataCTHD();
                    loaddataHoaDon();
                }
            }
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
            tb_mancc1.Text = TenNCC.SelectedValue.ToString();
            DataTable dt = new DataTable();
            String Query = "select * from NCC where ID_NCC = '" + TenNCC.SelectedValue + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
        }

        private void btn_xoaSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_maSP.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mã sản phẩm");
                }
                else
                {
                    DialogResult dlr = MessageBox.Show("Bạn muốn xóa sản phẩm?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        xoaSP();
                        loaddataSP();
                        loaddataCTHD();
                        loaddataHoaDon();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
            int i = dtg_KhachHang.CurrentRow.Index;
            txt_mk.Text = dtg_KhachHang.Rows[i].Cells[0].Value.ToString();
            txt_tk.Text = dtg_KhachHang.Rows[i].Cells[1].Value.ToString();
            txt_sdt.Text = dtg_KhachHang.Rows[i].Cells[2].Value.ToString();
            txt_dc.Text = dtg_KhachHang.Rows[i].Cells[3].Value.ToString();
            cb_gt.Text = dtg_KhachHang.Rows[i].Cells[4].Value.ToString();
        }

        private void btn_tktk_Click(object sender, EventArgs e)
        {
            if (txt_tktk.Text == "")
            {
                MessageBox.Show("Bạn phải nhập tên khách để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_tktk.Focus();
                return;
            }
            maHD.Text = txt_tktk.Text;
            DataTable dt = new DataTable();
            String Query = "EXEC sp_TKHD2 N'" + txt_tktk.Text + "' ;";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            if (dt.Rows.Count > 0)
            {
                dtgtimkiem.DataSource = dt;
                txt_tktsp.Text = "";
            }
            else MessageBox.Show("Không có kết quả cho tên khách bạn tìm kiếm!", "Thông báo", MessageBoxButtons.OK);
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

        void suaKH()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from KhachHang where ID_KH = N'" + txt_mk.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể sửa khách hàng không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string query = "Update KhachHang set Ten_KH = N'" + txt_tk.Text + "',DT_KH = '" + txt_sdt.Text + "',DC_KH = '" + txt_dc.Text + "' ,GioiTinh = N'" + cb_gt.Text + "' where ID_KH = '" + txt_mk.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void btn_suakh_Click(object sender, EventArgs e)
        {
            if (txt_mk.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập thông tin");
            }
            else
            {
                DialogResult dlr = MessageBox.Show("Bạn muốn sửa thông tin khách hàng?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                {
                    suaKH();
                    loaddataKhachHang();
                }
            }
        }

        private void dtg_TK_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dtg_TK.CurrentRow.Index;
            tb_ttk.Text = dtg_TK.Rows[i].Cells[0].Value.ToString();
            tb_mk.Text = dtg_TK.Rows[i].Cells[1].Value.ToString();
            cb_ltk.Text = dtg_TK.Rows[i].Cells[2].Value.ToString();
            tb_sttk.Text = dtg_TK.Rows[i].Cells[0].Value.ToString();
        }
        void themTK()
        {
            string query = "Insert into DangNhap (Tai_Khoan, Mat_Khau, type) values(N'" + tb_ttk.Text + "',N'" + tb_mk.Text + "', '" + cb_ltk.Text + "')";
            connectDB con = new connectDB();
            con.ExcuteNonQuery(query);
        }
        void suaTK()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from DangNhap where Tai_Khoan = N'" + tb_ttk.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể sửa tài khoản không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string query = "Update DangNhap set Tai_Khoan = N'" + tb_sttk.Text + "',Mat_Khau = N'" + tb_mk.Text + "',type = '" + cb_ltk.Text + "' where Tai_Khoan = '" + tb_ttk.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        void xoaTK()
        {
            DataTable dt = new DataTable();
            string query1 = "select * from DangNhap where Tai_Khoan = N'" + tb_ttk.Text + "'";
            connectDB conn = new connectDB();
            dt = conn.getTable(query1);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không thể xóa tài khoản không tồn tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else 
            {
                string query = "Delete from DangNhap where Tai_Khoan = N'" + tb_ttk.Text + "'";
                conn.ExcuteNonQuery(query);
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void bt_ttk_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_ttk.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập tên tài khoản");
                }
                else if (tb_mk.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mật khẩu");
                }
                else if (tb_mk.Text != tb_nlmk.Text)
                {
                    MessageBox.Show("Mật khẩu và nhập lại mật khẩu không khớp");
                }
                else
                {
                    DialogResult dlr = MessageBox.Show("Bạn muốn thêm tài khoản?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        themTK();
                        loaddataTaiKhoan();
                        MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK);
                        tb_ttk.Text = "";
                        tb_mk.Text = "";
                        tb_nlmk.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Thông tin không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            
                if (tb_ttk.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập tên tài khoản muốn sửa");
                }
                else if (tb_mk.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập mật khẩu");
                }
                else if (tb_mk.Text != tb_nlmk.Text)
                {
                    MessageBox.Show("Mật khẩu và nhập lại mật khẩu không khớp");
                }
                else if (tb_sttk.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập tên tài khoản sau khi được sửa");
                }
                else
                {
                    DialogResult dlr = MessageBox.Show("Bạn muốn sửa tài khoản?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        suaTK();
                        loaddataTaiKhoan();
                        tb_sttk.Text = "";
                    }
                
                }

        }

        private void bt_xtk_Click(object sender, EventArgs e)
        {
   
                if (tb_ttk.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập tên tài khoản muốn xóa");
                }
                else if (tb_ttk.Text == quyen)
                {
                    MessageBox.Show("Bạn không thể tự xóa tài khoản của mình được");
                }
                else
                {
                DialogResult dlr = MessageBox.Show("Bạn muốn xóa tài khoản?","Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlr == DialogResult.Yes)
                    {
                        xoaTK();
                        loaddataTaiKhoan();
                    }
                }
        }

        private void btn_tkkh_Click(object sender, EventArgs e)
        {
            if (tb_tkkh.Text == "")
            {
                MessageBox.Show("Bạn phải nhập tên khách để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tb_tkkh.Focus();
                return;
            }
            DataTable dt = new DataTable();
            String Query = "EXEC sp_TKKH N'" + tb_tkkh.Text + "' ;";
            connectDB conn = new connectDB();
            dt = conn.getTable(Query);
            if (dt.Rows.Count > 0)
            {
                dtg_timkiem1.DataSource = dt;
            }
            else MessageBox.Show("Không có kết quả cho tên khách bạn tìm kiếm!", "Thông báo", MessageBoxButtons.OK);
        }
    }
}

