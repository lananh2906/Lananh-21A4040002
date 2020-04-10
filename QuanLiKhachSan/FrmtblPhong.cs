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

namespace QuanLiKhachSan
{
    public partial class FrmtblPhong : Form
    {
        SqlConnection con = new SqlConnection();
        public FrmtblPhong()
        {
            InitializeComponent();
        }

        private void FrmtblPhong_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";
            con.ConnectionString = connectionString;
            con.Open();

            loadDataToGridview();

        }
        private void loadDataToGridview()
        {
            string sql = "Select * From tblPhong";
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);

            DataTable tabletblPhong = new DataTable();
            adp.Fill(tabletblPhong);

            DataGridView.DataSource = tabletblPhong;
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaPhong.Text = DataGridView.CurrentRow.Cells["MaPhong"].Value.ToString();
            txtTenPhong.Text = DataGridView.CurrentRow.Cells["TenPhong"].Value.ToString();
            txtDonGia.Text = DataGridView.CurrentRow.Cells["DonGia"].Value.ToString();
            txtMaPhong.Enabled = false;

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";
            txtMaPhong.Enabled = true;

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql = "Delete From tblPhong Where MaPhong = '" + txtMaPhong.Text + "'";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            loadDataToGridview();

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(txtMaPhong.Text == "")
            {
                MessageBox.Show("bạn cần nhập mã phòng");
                txtMaPhong.Focus();
                return;
            }
            if (txtTenPhong.Text == "")
            {
                MessageBox.Show("Bạn cần nhập tên phòng");
                txtTenPhong.Focus();
            }
            else
            {
                
                string sql = "insert into tblPhong values ('" + txtMaPhong.Text + "', '" + txtTenPhong.Text + "'";
                if (txtDonGia.Text != "")
                    sql = sql + ", " + txtDonGia.Text.Trim();
                sql = sql + ")";
                ///MessageBox.Show(sql);
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();

                    loadDataToGridview();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return;


            }


        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";
            btnThem.Enabled = true;
            btnThoat.Enabled = true;
            btnLuu.Enabled = true;
            btnHuy.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            con.Close();
            this.Close();

        }
    }
}
