using QLTTSV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTTSV
{
    public partial class FrmFind : Form
    {
        DBContext context = new DBContext();
        public FrmFind()
        {
            InitializeComponent();
        }

        private void FrmFind_Load(object sender, EventArgs e)
        {
            //Khỏi tạo danh sách khoa
            cmbKhoa.Items.Add("Công Nghệ Thông Tin");
            cmbKhoa.Items.Add("Ngôn Ngữ Anh");
            cmbKhoa.Items.Add("Quản trị kinh doanh");
            
            btnXoa.Enabled = false;
            //Đẩy lên data

        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            List<Student> listStudent = context.Students.ToList();
        }
    }
}
