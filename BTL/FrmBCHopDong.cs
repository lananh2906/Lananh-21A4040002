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
using COMExcel = Microsoft.Office.Interop.Excel;
using QuanLiAnhVienAoCuoi.Class;

namespace QuanLiAnhVienAoCuoi
{
    public partial class FrmBCHopDong : Form
    {
        DataTable tblBC;
        public FrmBCHopDong()
        {
            InitializeComponent();
        }

        private void FrmBCHopDong_Load(object sender, EventArgs e)
        {
            Functions.Connect();
            ResetValues();
            DataGridView.DataSource = null;
        }

        private void ResetValues()
        {
            txtThang.Text = "";
            txtNam.Text = "";
            txtTongtien.Text = "0";
        }

        private void Load_DataGridView()
        {
            DataGridView.Columns[0].HeaderText = "Mã HD";
            DataGridView.Columns[1].HeaderText = "Mã Khách";
            DataGridView.Columns[2].HeaderText = "Mã NV";
            DataGridView.Columns[3].HeaderText = "Ngày HD";
            DataGridView.Columns[4].HeaderText = "Ngày Thanh Toán";
            DataGridView.Columns[5].HeaderText = "Khuyến mại";
            DataGridView.Columns[6].HeaderText = "Tạm ứng";
            DataGridView.Columns[7].HeaderText = "Tổng tiền";

            DataGridView.Columns[0].Width = 150;
            DataGridView.Columns[1].Width = 100;
            DataGridView.Columns[2].Width = 80;
            DataGridView.Columns[3].Width = 80;
            DataGridView.Columns[4].Width = 80;
            DataGridView.Columns[5].Width = 80;
            DataGridView.Columns[6].Width = 80;
            DataGridView.Columns[6].Width = 80;

            DataGridView.AllowUserToAddRows = false;
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;

        }

        private void btnBaocao_Click(object sender, EventArgs e)
        {
            string sql;
            Double tong;
            sql = "SELECT a.MaHD, a.MaKhach, a.MaNV, a.NgayHD, b.NgayThanhToan,a.Khuyenmai,a.TamUng, b.Tongtien FROM tblHopdong AS a, tblThanhtoanHD AS b WHERE 1=1 AND a.MaHD=b.MaHD ";
            if (txtThang.Text != "")
                sql = sql + " AND MONTH(NgayHD) =" + txtThang.Text;
            if(txtNam.Text != "")
                sql = sql + " AND YEAR(NgayHD) =" + txtNam.Text;

            tblBC = Functions.GetDataToTable(sql);
            if (tblBC.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetValues();
            }
            else
            {
                MessageBox.Show("Có " + tblBC.Rows.Count + " bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tong = Convert.ToDouble(Functions.GetFieldValues("SELECT sum(Tongtien) FROM tblHopdong AS a, tblThanhtoanHD AS b WHERE a.MaHD=b.MaHD AND MONTH(NgayHD)=N'" + txtThang.Text + "' AND YEAR(NgayHD)=N'" + txtNam.Text + "'"));
                Functions.RunSql(sql);
                txtTongtien.Text = tong.ToString();
            }
            DataGridView.DataSource = tblBC;
            Load_DataGridView();

        }

        private void btnLamlai_Click(object sender, EventArgs e)
        {
            ResetValues();
            Load_DataGridView();
        }

        private void btnInbaocao_Click(object sender, EventArgs e)
        {
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            string sql;
            int hang = 0, cot = 0;
            DataTable tblThongtinHD;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Name = "Times new roman";
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Ảnh viện áo cưới 13";

            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "Cầu Giấy - Hà Nội";

            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: (04) 44456789";


            exRange.Range["C2:F2"].Font.Size = 16;
            exRange.Range["C2:F2"].Font.Name = "Times new roman";
            exRange.Range["C2:F2"].Font.Bold = true;
            exRange.Range["C2:F2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:F2"].MergeCells = true;
            exRange.Range["C2:F2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:F2"].Value = "BÁO CÁO DANH SÁCH HỢP ĐỒNG";

            exRange.Range["B4:B4"].MergeCells = true;
            exRange.Range["B4:B4"].Font.Bold = true;
            exRange.Range["B4:B4"].Font.Italic = true;
            exRange.Range["B4:B4"].Value = "Kì báo cáo: ";

            exRange.Range["C4:C4"].MergeCells = true;
            exRange.Range["C4:C4"].Value = txtThang.Text + "/" + txtNam.Text;


            sql = "SELECT a.MaHD, a.MaKhach, a.MaNV, a.NgayHD, b.NgayThanhToan, a.Khuyenmai, a.TamUng, b.Tongtien FROM tblHopdong AS a, tblThanhtoanHD AS b WHERE 1=1 AND a.MaHD=b.MaHD";
            tblThongtinHD = Functions.GetDataToTable(sql);
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:F5"].Font.Bold = true;
            exRange.Range["A5:F5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:F5"].ColumnWidth = 12;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã HD";
            exRange.Range["C5:C5"].Value = "Mã khách";
            exRange.Range["D5:D5"].Value = "Mã NV";
            exRange.Range["E5:E5"].Value = "Ngày hợp đồng";
            exRange.Range["F5:F5"].Value = "Ngày thanh toán";
            exRange.Range["G5:G5"].Value = "Khuyến mại";
            exRange.Range["H5:H5"].Value = "Tạm ứng";
            exRange.Range["I5:I5"].Value = "Tổng tiền";

            for (hang = 0; hang <= tblThongtinHD.Rows.Count - 1; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 6] = hang + 1;
                for (cot = 0; cot <= tblThongtinHD.Columns.Count - 1; cot++)
                    //Điền thông tin hàng từ cột thứ 2, dòng 12
                    exSheet.Cells[cot + 2][hang + 6] = tblThongtinHD.Rows[hang][cot].ToString();
            }
            exRange = exSheet.Cells[cot][hang + 8];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange.Value2 = "Tổng tiền: " + txtTongtien.Text.ToString();
            exRange = exSheet.Cells[1][hang + 9];
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            exRange.Range["A1:F1"].Value = "    Bằng chữ: " + Functions.ChuyenSoSangChu(txtTongtien.Text.ToString());

            exRange = exSheet.Cells[4][hang + 10];
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            exRange.Range["A1:C1"].Value = "Hà Nội, ngày " + day + " tháng " + month + " năm " + year;

            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = "Nhân viên lập báo cáo";

            exRange.Range["A3:C3"].MergeCells = true;
            exRange.Range["A3:C3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:C3"].Value = "(Kí, Ghi rõ họ tên)";

            exSheet.Name = "BC Hợp Đồng";
            exApp.Visible = true;

        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
