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
using QLTTSV.Model;

namespace QLTTSV.QLSV
{
    public partial class FrmFaculty : Form
    {
        private int index = -1;
        DBContext context = new DBContext();
        public FrmFaculty()
        {
            InitializeComponent();
        }

        private void FrmFaculty_Load(object sender, EventArgs e)
        {
            List<Faculty> listFaculty = context.Faculties.ToList();
            BindGrid(listFaculty);
            btnThemSua.Text = "Thêm";
        }

        private void BindGrid(List<Faculty> listFaculty)
        {
            dgvDanhSach.Rows.Clear();
            foreach (var item in listFaculty)
            {
                int index = dgvDanhSach.Rows.Add();
                dgvDanhSach.Rows[index].Cells[0].Value = item.FacultyID;
                dgvDanhSach.Rows[index].Cells[1].Value = item.FacultyName;
                dgvDanhSach.Rows[index].Cells[2].Value = item.TotalProfessor;
            }
        }

        private void btnThemSua_Click(object sender, EventArgs e)
        {
            if (index == -1)
            {
                int dongmoi = dgvDanhSach.Rows.Add();
                CapNhatThongTin(dongmoi);
                Faculty dbInsert = new Faculty();
                dbInsert.FacultyID = int.Parse(txtMaKhoa.Text);
                dbInsert.FacultyName = txtTenKhoa.Text;
                dbInsert.TotalProfessor = int.Parse(txtTongGS.Text);
                context.Faculties.Add(dbInsert);
                context.SaveChanges();
                Reset();
            }
            else
            {
                int ID = int.Parse(txtMaKhoa.Text);
                Faculty dbUpdate = context.Faculties.FirstOrDefault(p => p.FacultyID == ID);
                if (dbUpdate != null)
                {
                    //dgv
                    CapNhatThongTin(index);
                    //DB
                    dbUpdate.FacultyID = int.Parse(txtMaKhoa.Text);
                    dbUpdate.FacultyName = txtTenKhoa.Text;
                    dbUpdate.TotalProfessor = int.Parse(txtTongGS.Text);
                    context.SaveChanges();
                    Reset();
                }
            }
        }

        private void CapNhatThongTin(int dong)
        {
            dgvDanhSach.Rows[dong].Cells[0].Value = txtMaKhoa.Text;
            dgvDanhSach.Rows[dong].Cells[1].Value = txtTenKhoa.Text;
            dgvDanhSach.Rows[dong].Cells[2].Value = txtTongGS.Text;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(txtMaKhoa.Text);
            Faculty dbDelete = context.Faculties.FirstOrDefault(p => p.FacultyID == ID);
            if (dbDelete != null)
            {
                dgvDanhSach.Rows.RemoveAt(index);
                context.Faculties.Remove(dbDelete);
                context.SaveChanges();
                Reset();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                index = e.RowIndex;
                txtMaKhoa.Text = dgvDanhSach.Rows[index].Cells[0].Value.ToString();
                txtTenKhoa.Text = dgvDanhSach.Rows[index].Cells[1].Value.ToString();
                if (dgvDanhSach.Rows[index].Cells[2].Value == null)
                {
                    txtTongGS.Text = "0";
                }
                else
                {
                    txtTongGS.Text = dgvDanhSach.Rows[index].Cells[2].Value.ToString();
                }
                btnThemSua.Text = "Sửa";
                btnXoa.Enabled = true;
            }
        }

        private void Reset()
        {
            txtMaKhoa.Text = txtTenKhoa.Text = txtTongGS.Text = string.Empty;
            btnThemSua.Text = "Thêm";
        }
    }  
}
