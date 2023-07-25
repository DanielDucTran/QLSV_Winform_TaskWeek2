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
    public partial class FormDangKiMonHoc : Form
    {
        //DataTable dsMH;
        public FormDangKiMonHoc()
        {
            InitializeComponent();
        }
        private void FormDangKiMonHoc_Load(object sender, EventArgs e)
        {
            LoadDataMonHoc();

            dataGridView1.Columns["MaMH"].HeaderText = "Mã Môn Học";
            dataGridView1.Columns["MaLop"].HeaderText = "Mã Lớp";
            dataGridView1.Columns["TenMH"].HeaderText = "Tên Môn Học";
            dataGridView1.Columns["SoTC"].HeaderText = "Số Tín Chỉ";
            dataGridView1.Columns["CN"].HeaderText = "Chuyên Ngành";
            dataGridView1.Columns["GV"].HeaderText = "Giáo Viên";
        }

        private void LoadDataMonHoc() {
            Form1 f1 = new Form1();
            f1.dtMH = f1.ReadDataMH();
            dataGridView1.DataSource = f1.dtMH;
            f1.dtMH = f1.CreateTable1();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Index >= 0)
            {
                if (DialogResult.Yes == MessageBox.Show(
                                                        "Bạn muốn đăng ký môn: [ " + dataGridView1.Rows[e.RowIndex].Cells["TenMH"].Value.ToString() + "]?",
                                                        "Xác nhận đăng ký", MessageBoxButtons.YesNo, MessageBoxIcon.Question)                
                                                        )
                {

                }
            }
        }
    }
}
