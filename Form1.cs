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
        string flag; // Nhận biết button thêm hay sửa
        DataTable dtSV, dtMH;
        int index, indexMH;
        public Form1()
        {
            InitializeComponent();
        }
        public void LockControlSV() {

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
            txtMaMH.ReadOnly = true;
            txtSoTC.ReadOnly = true;
            txtCN.ReadOnly = true;


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
            txtMaMH.ReadOnly = false;
            txtSoTC.ReadOnly = false;
            txtCN.ReadOnly = false;


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
            if (flag == "add") {
                if (CheckData())
                {
                    dtSV.Rows.Add(txtMaSV.Text, txtTenSV.Text, cboSex.SelectedItem, txtLop.Text);
                    dataGridSinhVien.DataSource = dtSV;
                    dataGridSinhVien.RefreshEdit();
                }
                LockControlSV();
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
            try
            {
                if (dataGridSinhVien.CurrentRow != null && dataGridSinhVien.DataSource is DataTable dt1)
                {
                    index = dataGridSinhVien.CurrentCell.RowIndex;

                    if (dt1.Rows.Count > 0 && index < dt1.Rows.Count)
                    {
                        txtMaSV.Text = dt1.Rows[index]["MaSV"].ToString();
                        txtTenSV.Text = dt1.Rows[index]["TenSV"].ToString();
                        cboSex.Text = dt1.Rows[index]["Sex"].ToString();
                        txtLop.Text = dt1.Rows[index]["Lop"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi    1: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Ghi log chi tiết lỗi nếu cần
            }
        }

        public void SaveData(DataTable dt) {

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
            flag = "add";

            txtMaMH.Text = "";
            txtTenMH.Text = "";
            txtCN.Text = "";
            txtSoTC.Text = "";
        }

        private void btnSua1_Click(object sender, EventArgs e)
        {
            UnlockControlSV();
            flag = "edit";

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

        public DataTable CreateTable1() {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMH");
            dt.Columns.Add("TenMH");
            dt.Columns.Add("SoTC");
            dt.Columns.Add("CN");

            return dt;
        }
        private void btnLuu1_Click(object sender, EventArgs e)
        {
            if (flag == "add")
            {
                if (CheckData1())
                {
                    dtMH.Rows.Add(txtMaMH.Text, txtTenMH.Text, txtSoTC.Text, txtCN.Text);
                    dataGridMH.DataSource = dtMH;
                    dataGridMH.RefreshEdit();
                }
                LockControlSV();
            }
            else if (flag == "edit")
            {
                if (CheckData1())
                {
                    dtMH.Rows[index][0] = txtMaMH.Text;
                    dtMH.Rows[index][1] = txtTenMH.Text;
                    dtMH.Rows[index][2] = txtSoTC.Text;
                    dtMH.Rows[index][3] = txtCN.Text;
                    dataGridMH.DataSource = dtMH;
                    dataGridMH.RefreshEdit();
                }
                LockControlSV();
            }
        }

        private void dataGridMH_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridMH.CurrentRow != null && dataGridMH.DataSource is DataTable dt1)
                {
                    indexMH = dataGridMH.CurrentCell.RowIndex;

                    if (dt1.Rows.Count > 0 && indexMH < dt1.Rows.Count)
                    {
                        txtMaMH.Text = dt1.Rows[index]["MaMH"].ToString();
                        txtTenMH.Text = dt1.Rows[index]["TenMH"].ToString();
                        txtSoTC.Text = dt1.Rows[index]["SoTC"].ToString();
                        txtCN.Text = dt1.Rows[index]["CN"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bản ghi: " + ex.Message, "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Ghi log chi tiết lỗi nếu cần
            }
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

            return true;
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
            string maMH, tenMH, soTC, CN;

            while (str != null)
            {
                try
                {
                    string[] tmp = str.Split('\t');

                    maMH = tmp[0];
                    tenMH = tmp[1];
                    soTC = tmp[2];
                    CN = tmp[3];
                    dt.Rows.Add(maMH, tenMH, soTC, CN);

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
    }
        
}
