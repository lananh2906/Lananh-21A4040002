﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;// Sử dụng hàm MessageBox

namespace QuanLiAnhVienAoCuoi.Class
{
    class Functions
    {
        public static SqlConnection Conn; //Khai báo đối tượng kết nối
        public static string connString; //Khai báo biến chứa chuỗi kết nối
        public static void Connect()
        {
            //Thiết lập giá trị cho chuỗi kết nối
            connString = "Data Source = localhost\\SQLEXPRESS; Initial Catalog = QuanLiAnhVienAoCuoi; Integrated Security = True";
            Conn = new SqlConnection(); //Cấp phát đối tượng
            Conn.ConnectionString = connString; //Kết nối
            Conn.Open(); //Mở kết nối
        }
        public static void Disconnect()
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close(); //Đóng kết nối
                Conn.Dispose(); //Giải phóng tài nguyên
                Conn = null;
            }
        }

        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter Mydata;
            Mydata = new SqlDataAdapter(sql, Conn);
            DataTable table = new DataTable();

            Mydata.Fill(table);
            return table;
        }

        public static string GetFieldValues(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, Functions.Conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ma = reader.GetValue(0).ToString();
            }
            reader.Close();
            return ma;
        
        }

        public static void RunSql(string sql)
        {
            SqlCommand cmd; // Khai báo đối tượng SqlCommand
            cmd = new SqlCommand(); // Khởi tạo đối tượng
            cmd.Connection = Functions.Conn; // Gán kết nối
            cmd.CommandText = sql; // Gán câu lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); // Thực hiện câu lệnh SQL
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }

        public static string ChuyenSoSangChu(string sNumber)
        {
            int mLen, mDigit;
            string mTemp = " ";
            string[] mNumText;
            sNumber = sNumber.Replace(",", " ");
            mNumText = " không; một; hai; ba; bốn; năm; sáu; bảy; tám; chín ".Split(';'); mLen = sNumber.Length - 1; // trừ 1 vì thứ tự đi từ 0
            for (int i = 0; i <= mLen; i++)
            {
                mDigit = Convert.ToInt32(sNumber.Substring(i, 1));
                mTemp = mTemp + "" + mNumText[mDigit];
                if (mLen == i) // Chữ số cuối cùng không cần xét tiếp
                    break;
                switch ((mLen - i) % 9)
                {
                    case 0:
                        mTemp = mTemp + " tỷ ";
                        if (sNumber.Substring(i + 1, 3) == " 000 ")
                            i = i + 3;
                        if (sNumber.Substring(i + 1, 3) == " 000 ")
                            i = i + 3;
                        if (sNumber.Substring(i + 1, 3) == " 000 ")
                            i = i + 3;
                        break;
                    case 6:
                        mTemp = mTemp + " triệu ";
                        if (sNumber.Substring(i + 1, 3) == " 000 ")
                            i = i + 3;
                        if (sNumber.Substring(i + 1, 3) == " 000 ")
                            i = i + 3;
                        break;
                    case 3:
                        mTemp = mTemp + " nghìn ";
                        if (sNumber.Substring(i + 1, 3) == " 000 ")
                            i = i + 3;
                        break;
                    default:
                        switch ((mLen - i) % 3)
                        {
                            case 2:
                                mTemp = mTemp + " trăm ";
                                break;
                            case 1:
                                mTemp = mTemp + " mươi ";
                                break;
                        }
                        break;
                }
            }
            //Loại bỏ trường hợp x00
            mTemp = mTemp.Replace(" không mươi không ", " ");
            //Loại bỏ trường hợp 00x
            mTemp = mTemp.Replace(" không mươi ", " linh ");
            //Loại bỏ trường hợp x0, x&gt;=2
            mTemp = mTemp.Replace(" mươi không", " mươi ");
            //Fix trường hợp 10
            mTemp = mTemp.Replace(" một mươi", " mười ");
            //Fix trường hợp x4, x&gt;=2
            mTemp = mTemp.Replace(" mươi bốn", " mươi tư");
            //Fix trường hợp x04
            mTemp = mTemp.Replace(" linh bốn", " linh tư");
            //Fix trường hợp x5, x&gt;=2
            mTemp = mTemp.Replace(" mươi năm", "mươi lăm");
            //Fix trường hợp x1, x&gt;=2
            mTemp = mTemp.Replace(" mươi một", " mươi mốt");
            //Fix trường hợp x15
            mTemp = mTemp.Replace(" mười năm", " mười lăm");
            //Bỏ ký tự space
            mTemp = mTemp.Trim();
            //Viết hoa ký tự đầu tiên
            mTemp = mTemp.Substring(0, 1).ToUpper() + mTemp.Substring(1) + " đồng ";
            return mTemp;
        }
    }
}
