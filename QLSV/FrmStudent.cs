using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QLTTSV.Model;

namespace QLTTSV.QLSV
{
    public partial class FrmStudent : Form
    {
        private int index = -1;
        DBContext context = new DBContext();     

        public FrmStudent()
        {
            InitializeComponent();
        }

        private void FrmStudent_Load(object sender, EventArgs e)
        {           
            //Khỏi tạo danh sách khoa
            cmbKhoa.Items.Add("Công Nghệ Thông Tin");
            cmbKhoa.Items.Add("Ngôn Ngữ Anh");
            cmbKhoa.Items.Add("Quản trị kinh doanh");
            cmbKhoa.Text = "Công Nghệ Thông Tin";           
            //Tắt nút xóa và sửa
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            try
            {                
                List<Faculty> listFaculty = context.Faculties.ToList();
                List<Student> listStudent = context.Students.ToList();
                FillFalcultyCombobox(listFaculty);
                BindGird(listStudent);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGird(List<Student> listStudent)
        {
            dgvDanhSach.Rows.Clear();
            foreach (Student student in listStudent)
            {
                index = dgvDanhSach.Rows.Add();
                dgvDanhSach.Rows[index].Cells[0].Value = student.StudentID;
                dgvDanhSach.Rows[index].Cells[1].Value = student.FullName;
                dgvDanhSach.Rows[index].Cells[2].Value = student.Faculty.FacultyName;
                dgvDanhSach.Rows[index].Cells[3].Value = student.AverageScore;
                
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFaculty)
        {
            this.cmbKhoa.DataSource = listFaculty;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTraThongTinNhap())
            {
                Student student = new Student();
                student.StudentID=txtMSSV.Text;
                student.FullName=txtHoTen.Text;
                student.AverageScore = float.Parse(txtDTB.Text);
                if(cmbKhoa.SelectedIndex!=1)
                {
                    student.FacultyID = 1;
                }    
                else if(cmbKhoa.SelectedIndex==1)
                {
                    student.FacultyID = 2;
                }    
                else
                {
                    student.FacultyID = 3;
                }    
                context.Students.Add(student);
                context.SaveChanges();
                index = dgvDanhSach.Rows.Add();
                CapNhatThongTin(index);
                MessageBox.Show("Thêm dữ liệu thành công!", "Thông Báo", MessageBoxButtons.OK);
                ResetForm();
            }
        }

        private bool KiemTraThongTinNhap()
        {
            //Hàm cảnh báo nếu trống
            errCanhBao.Clear();
            if (txtMSSV.Text.Length!=10)
            {
                errCanhBao.SetError(txtMSSV, "Mã số sinh viên phải có 10 chữ số");
                MessageBox.Show("Vui lòng nhập đủ 10 số", "Thông Báo", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrEmpty(txtMSSV.Text))
            {
                errCanhBao.SetError(txtMSSV, "*");
                MessageBox.Show("Vui lòng nhâp đầy đủ thông tin!", "Thông Báo", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                errCanhBao.SetError(txtHoTen, "*");
                MessageBox.Show("Vui lòng nhâp đầy đủ thông tin!", "Thông Báo", MessageBoxButtons.OK);
                return false;
            }
            float diem;
            if (float.TryParse(txtDTB.Text, out diem) == false || diem < 0 || diem > 10)
            {
                errCanhBao.SetError(txtDTB, "*");
                MessageBox.Show("Vui lòng nhâp đầy đủ thông tin!", "Thông Báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string ID = txtMSSV.Text;
            Student dbUpdate = context.Students.FirstOrDefault(p=>p.StudentID == ID);
            if (KiemTraThongTinNhap())
            {
               if(dbUpdate!= null)
                {
                    if(index!=-1)
                    {
                        //Sủa trên datagrid
                        CapNhatThongTin(index);

                        //Sửa trên SQL
                        dbUpdate.StudentID = txtMSSV.Text;
                        dbUpdate.FullName = txtHoTen.Text;
                        dbUpdate.AverageScore = float.Parse(txtDTB.Text);
                        if (cmbKhoa.SelectedIndex != -1)
                        {
                            dbUpdate.FacultyID = 1;
                        }
                        else if (cmbKhoa.SelectedIndex == -1)
                        {
                            dbUpdate.FacultyID = 2;
                        }
                        else
                        {
                            dbUpdate.FacultyID = 3;
                        }
                        context.SaveChanges();
                    }                       
                }
                ResetForm();
            }
        }

        private void CapNhatThongTin(int index)
        {
            dgvDanhSach.Rows[index].Cells[0].Value = txtMSSV.Text;
            dgvDanhSach.Rows[index].Cells[1].Value = txtHoTen.Text;
            dgvDanhSach.Rows[index].Cells[2].Value = cmbKhoa.Text;
            dgvDanhSach.Rows[index].Cells[3].Value = float.Parse(txtDTB.Text).ToString();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                index = e.RowIndex;
                txtMSSV.Text = dgvDanhSach.Rows[index].Cells[0].Value.ToString();
                txtHoTen.Text = dgvDanhSach.Rows[index].Cells[1].Value.ToString();
                cmbKhoa.Text = dgvDanhSach.Rows[index].Cells[2].Value.ToString();
                txtDTB.Text = dgvDanhSach.Rows[index].Cells[3].Value.ToString();
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private void ResetForm()
        {
            txtMSSV.Text = txtHoTen.Text = txtDTB.Text = string.Empty;
            cmbKhoa.SelectedIndex = -1;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ID = txtMSSV.Text;
            Student dbDelete = context.Students.FirstOrDefault(p => p.StudentID == ID);
            if(dbDelete != null)
            {
                dgvDanhSach.Rows.RemoveAt(index);
                context.Students.Remove(dbDelete);
                context.SaveChanges();
            }    
            ResetForm();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Ban có muốn thoát", "YES/NO", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            FrmFaculty oj = new FrmFaculty();
            oj.Show();
        }

        private void quảnLýKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFaculty oj = new FrmFaculty();
            oj.Show();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            FrmFind of = new FrmFind();
            of.Show();
        }

        private void tiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFind of = new FrmFind();
            of.Show();
        }
    }
}
