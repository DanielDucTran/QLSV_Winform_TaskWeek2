using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class Form1 : Form
    {
        string flag, flagMH; // Nhận biết button thêm hay sửa
        public DataTable dtSV, dtMH;
        int index, indexMH;
        /// <summary>
        /// List luu tru danh sach mon hoc dang ki
        /// </summary>
        List<Object> dsMHDK = new List<object>();
        HashSet<Object> uniqueValues = new HashSet<Object>();

        public Form1()
        {
            InitializeComponent();
        }
        public void LockControlSV()
        {

            //Page1
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

            //page2
            btnThem1.Enabled = true;
            btnSua1.Enabled = true;
            btnXoa1.Enabled = true;
            btnLuu1.Enabled = false;
            btnHuy1.Enabled = false;

            txtTenSV.ReadOnly = true;
            txtMaSV.ReadOnly = true;
            txtLop.ReadOnly = true;

            // page2

            txtTenMH.ReadOnly = true;
            txtMaLop.ReadOnly = true;
            txtMaMH.ReadOnly = true;
            txtSoTC.ReadOnly = true;
            txtCN.ReadOnly = true;
            txtGV.ReadOnly = true;


            btnThem.Focus();
            btnThem1.Focus();
        }

        public void UnlockControlSV()
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem1.Enabled = false;
            btnSua1.Enabled = false;
            btnXoa1.Enabled = false;
            btnLuu1.Enabled = true;
            btnHuy1.Enabled = true;

            txtTenSV.ReadOnly = false;
            txtMaSV.ReadOnly = false;
            txtLop.ReadOnly = false;

            txtTenMH.ReadOnly = false;
            txtMaLop.ReadOnly = false;
            txtMaMH.ReadOnly = false;
            txtSoTC.ReadOnly = false;
            txtCN.ReadOnly = false;
            txtGV.ReadOnly = false;


            txtMaSV.Focus();
            txtMaMH.Focus();
        }

        public DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSV");
            dt.Columns.Add("TenSV");
            dt.Columns.Add("Sex");
            dt.Columns.Add("Lop");

            return dt;
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LockControlSV();
            //dataGridDSDangKi.DataSource = FormDangKiMonHoc.dsMH;

            try
            {
                dtSV = ReadData();
                dataGridSinhVien.DataSource = dtSV;

                dtMH = ReadDataMH();
                dataGridMH.DataSource = dtMH;
            }
            catch (Exception ex)
            {

                dtSV = CreateTable();
                dtMH = CreateTable1();
            }

            LoadDanhSachMonHoc();
            ReadDataMHDK();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            UnlockControlSV();
            flag = "add";

            txtMaSV.Text = "";
            txtTenSV.Text = "";
            txtLop.Text = "";
            cboSex.Text = "";


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            UnlockControlSV();
            flag = "edit";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (flag == "add")
            {
                if (CheckData())
                {
                    dtSV.Rows.Add(txtMaSV.Text, txtTenSV.Text, cboSex.Text, txtLop.Text);
                    dataGridSinhVien.DataSource = dtSV;
                    dataGridSinhVien.RefreshEdit();
                }
                // LockControlSV();
            }
            else if (flag == "edit")
            {
                if (CheckData())
                {
                    dtSV.Rows[index][0] = txtMaSV.Text;
                    dtSV.Rows[index][1] = txtTenSV.Text;
                    dtSV.Rows[index][2] = cboSex.Text;
                    dtSV.Rows[index][3] = txtLop.Text;
                    dataGridSinhVien.DataSource = dtSV;
                    dataGridSinhVien.RefreshEdit();
                }
            }
            LockControlSV();
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa sinh viên này!", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dtSV.Rows.RemoveAt(index);
                dataGridSinhVien.DataSource = dtSV;
                dataGridSinhVien.RefreshEdit();
            }

        }
        public bool CheckData()
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Bạn chưa nhập Mã Sinh Viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaSV.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTenSV.Text))
            {
                MessageBox.Show("Bạn chưa nhập Tên Sinh Viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenSV.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtLop.Text))
            {
                MessageBox.Show("Bạn chưa nhập Lớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLop.Focus();
                return false;
            }
            if (string.IsNullOrEmpty((string)cboSex.SelectedItem))
            {
                MessageBox.Show("Bạn chưa chọn Giới Tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLop.Focus();
                return false;
            }

            return true;
        }


        private void dataGridSinhVien_SelectionChanged_1(object sender, EventArgs e)
        {
            //try
            //{
            //    if (dataGridSinhVien.CurrentRow != null && dataGridSinhVien.DataSource is DataTable dt1)
            //    {
            //        index = dataGridSinhVien.CurrentCell.RowIndex;

            //        if (dt1.Rows.Count > 0 && index < dt1.Rows.Count)
            //        {
            //            txtMaSV.Text = dt1.Rows[index]["MaSV"].ToString();
            //            txtTenSV.Text = dt1.Rows[index]["TenSV"].ToString();
            //            cboSex.Text = dt1.Rows[index]["Sex"].ToString();
            //            txtLop.Text = dt1.Rows[index]["Lop"].ToString();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi    1: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    // Ghi log chi tiết lỗi nếu cần
            //}

            index = dataGridSinhVien.CurrentCell == null ? -1 : dataGridSinhVien.CurrentCell.RowIndex;
            if (index != -1)
            {
                txtMaSV.Text = dataGridSinhVien.Rows[index].Cells[0].Value.ToString();
                txtTenSV.Text = dataGridSinhVien.Rows[index].Cells[1].Value.ToString();
                cboSex.Text = dataGridSinhVien.Rows[index].Cells[2].Value.ToString();
                txtLop.Text = dataGridSinhVien.Rows[index].Cells[3].Value.ToString();
            }
        }

        public void SaveData(DataTable dt)
        {

            FileStream fs = new FileStream("data1.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sw.Write(dt.Rows[i][j].ToString() + "\t");
                    //
                }
                sw.WriteLine();
            }
            sw.Close();
            fs.Close();

        }

        public DataTable ReadData()
        {
            DataTable dt = CreateTable();
            FileStream fs = new FileStream("data1.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadLine();
            string maSv, tenSv, sex, lop;

            while (str != null)
            {
                try
                {
                    string[] tmp = str.Split('\t');

                    maSv = tmp[0];
                    tenSv = tmp[1];
                    sex = tmp[2];
                    lop = tmp[3];
                    dt.Rows.Add(maSv, tenSv, sex, lop);

                    str = sr.ReadLine();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi data SinhVien: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData(dtSV);
            SaveDataMH(dtMH);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Design Page 2
        /// Quản lý môn học
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnThem1_Click(object sender, EventArgs e)
        {
            UnlockControlSV();
            flagMH = "add";

            txtMaMH.Text = "";
            txtMaLop.Text = "";
            txtTenMH.Text = "";
            txtCN.Text = "";
            txtSoTC.Text = "";
            txtGV.Text = "";
        }

        private void btnSua1_Click(object sender, EventArgs e)
        {
            UnlockControlSV();
            flagMH = "edit";

        }

        private void btnXoa1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa môn học này!", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dtMH.Rows.RemoveAt(indexMH);
                dataGridMH.DataSource = dtMH;
                dataGridMH.RefreshEdit();
            }
        }

        public DataTable CreateTable1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMH");
            dt.Columns.Add("MaLop");
            dt.Columns.Add("TenMH");
            dt.Columns.Add("SoTC");
            dt.Columns.Add("CN");
            dt.Columns.Add("GV");

            return dt;
        }
        private void btnLuu1_Click(object sender, EventArgs e)
        {
            if (flagMH == "add")
            {
                if (CheckData1())
                {
                    dtMH.Rows.Add(txtMaMH.Text, txtMaLop.Text, txtTenMH.Text, txtSoTC.Text, txtCN.Text, txtGV.Text);
                    dataGridMH.DataSource = dtMH;
                    dataGridMH.RefreshEdit();
                }

            }
            else if (flagMH == "edit")
            {
                if (CheckData1())
                {
                    dtMH.Rows[indexMH][0] = txtMaMH.Text;
                    dtMH.Rows[indexMH][1] = txtMaLop.Text;
                    dtMH.Rows[indexMH][2] = txtTenMH.Text;
                    dtMH.Rows[indexMH][3] = txtSoTC.Text;
                    dtMH.Rows[indexMH][4] = txtCN.Text;
                    dtMH.Rows[indexMH][5] = txtGV.Text;
                    dataGridMH.DataSource = dtMH;
                    dataGridMH.RefreshEdit();
                }
                //LockControlSV();
            }
            LockControlSV();
        }

        private void dataGridMH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public bool CheckData1()
        {
            if (string.IsNullOrEmpty(txtMaMH.Text))
            {
                MessageBox.Show("Bạn chưa nhập Mã Môn Học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaMH.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMaLop.Text))
            {
                MessageBox.Show("Bạn chưa nhập Mã Lớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaLop.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTenMH.Text))
            {
                MessageBox.Show("Bạn chưa nhập Tên Môn Học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenMH.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSoTC.Text))
            {
                MessageBox.Show("Bạn chưa nhập Số Tín Chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoTC.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCN.Text))
            {
                MessageBox.Show("Bạn chưa chọn Chuyên Ngành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCN.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtGV.Text))
            {
                MessageBox.Show("Bạn chưa nhập Giáo Vien!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGV.Focus();
                return false;
            }

            return true;
        }

        private void dataGridMH_SelectionChanged(object sender, EventArgs e)
        {
            indexMH = dataGridMH.CurrentCell == null ? -1 : dataGridMH.CurrentCell.RowIndex;

            if (indexMH != -1)
            {
                txtMaMH.Text = dataGridMH.Rows[indexMH].Cells[0].Value.ToString();
                txtMaLop.Text = dataGridMH.Rows[indexMH].Cells[1].Value.ToString();
                txtTenMH.Text = dataGridMH.Rows[indexMH].Cells[2].Value.ToString();
                txtSoTC.Text = dataGridMH.Rows[indexMH].Cells[3].Value.ToString();
                txtCN.Text = dataGridMH.Rows[indexMH].Cells[4].Value.ToString();
                txtGV.Text = dataGridMH.Rows[indexMH].Cells[5].Value.ToString();
            }
        }

        public void SaveDataMH(DataTable dt)
        {

            FileStream fs = new FileStream("dataMH.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sw.Write(dt.Rows[i][j].ToString() + "\t");
                    //
                }
                sw.WriteLine();
            }
            sw.Close();
            fs.Close();

        }

        public DataTable ReadDataMH()
        {
            DataTable dt = CreateTable1();
            FileStream fs = new FileStream("dataMH.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadLine();
            string maMH, maLop, tenMH, soTC, CN, GV;

            while (str != null)
            {
                //try
                //{
                string[] tmp = str.Split('\t');

                maMH = tmp[0];
                maLop = tmp[1];
                tenMH = tmp[2];
                soTC = tmp[3];
                CN = tmp[4];
                GV = tmp[5];
                dt.Rows.Add(maMH, maLop, tenMH, soTC, CN, GV);

                str = sr.ReadLine();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Lỗi data mon hoc PAGE 2: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary>
        /// page 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void LoadDanhSachMonHoc()
        {
            try
            {
                dtMH = ReadDataMH();
                dataGridDKMH.DataSource = dtMH;
            }
            catch (Exception)
            {
                dtMH = CreateTable1();
            }
        }
        public void ReadDataMHDK()
        {
            FileStream fs = new FileStream("dataMH.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadLine();
            string maMH, tenMH;
            while (str != null)
            {
                //try
                //{
                string[] tmp = str.Split('\t');

                maMH = tmp[0];
                tenMH = tmp[2];
                string mhdk = "[" + maMH + "] " + "[" + tenMH + "]";
                Course dsMHDK = new Course(mhdk);
                cboMHDK.Items.Add(dsMHDK);

                str = sr.ReadLine();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Lỗi data mon hoc PAGE 2: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
            sr.Close();
            fs.Close();
        }
        
        public void CheckDangKyMonHoc()
        {
            dsMHDK.Add(cboMHDK.SelectedItem);
            bool hasDuplicates = false;
            foreach (var item in dsMHDK)
            {
                if (uniqueValues.Add(item))
                {
                    hasDuplicates = true;
                    break;
                }
            }
            if (hasDuplicates)
            {
                MessageBox.Show("Đăng kí học phần thành công!", "SUCCESS!");
            }
            else
            {
                MessageBox.Show("Học phần này đã được đăng kí!", "Lỗi!!!");
            }
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn muốn đăng ký học phần: " + cboMHDK.SelectedItem + " ", "xác nhận đăng ký", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            CheckDangKyMonHoc();

        }
    
    }
}
