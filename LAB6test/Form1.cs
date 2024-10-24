using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB6test
{
    public partial class Form1 : Form
    {
        private QLSachEntities db = new QLSachEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<LoaiSach> loaiSaches = db.LoaiSaches.ToList();
            List<SACH> book = db.SACHes.ToList();
            FillDataCBB(loaiSaches);
            FillDataGrid(book);
        }

        private void FillDataGrid(List<SACH> book)
        {
            dgvCRUD.Rows.Clear();
            foreach (var item in book)
            {
                int index = dgvCRUD.Rows.Add();
                dgvCRUD.Rows[index].Cells[0].Value = item.MaSach;
                dgvCRUD.Rows[index].Cells[1].Value = item.TenSach;
                dgvCRUD.Rows[index].Cells[2].Value = item.NamXB;
                dgvCRUD.Rows[index].Cells[3].Value = item.LoaiSach.MaLoai;
            }
        }

        private void FillDataCBB(List<LoaiSach> loaiSaches)
        {
            cmbTheloai.DataSource = loaiSaches;
            cmbTheloai.DisplayMember = "TenLoai";
            cmbTheloai.ValueMember = "MaLoai";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtYear.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách!");
                return;
            }
            if (txtID.Text.Length != 6)
            {
                MessageBox.Show("Mã sách phải có 6 ký tự!");
                return;
            }
            var book = new SACH()
            {
                MaSach = txtID.Text,
                TenSach = txtName.Text,
                NamXB = int.Parse(txtYear.Text),
                MaLoai = ((LoaiSach)cmbTheloai.SelectedItem).MaLoai
            };
            db.SACHes.Add(book);
            db.SaveChanges();
            MessageBox.Show("Thêm sách thành công");
            FillDataGrid(db.SACHes.ToList());
            ResetControl();
        }

        private void ResetControl()
        {
            txtID.Clear();
            txtName.Clear();
            txtYear.Clear();
            cmbTheloai.SelectedIndex = -1;
        }
        private bool CheckData()
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Mã sách không được để trống");
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Tên sách không được để trống");
                return false;
            }

            if (string.IsNullOrEmpty(txtYear.Text))
            {
                MessageBox.Show("Năm xuất bản không được để trống");
                return false;
            }
            if (txtID.Text.Length != 6)
            {
                MessageBox.Show("Mã sách phải có 6 ký tự!");
                return false;
            }
            return true;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckData())
                    return;
                var mss = txtID.Text;
                var book = db.SACHes.Find(mss);
                if (book != null)
                {
                    book.TenSach = txtName.Text;
                    book.NamXB = int.Parse(txtYear.Text);
                    book.MaLoai = ((LoaiSach)cmbTheloai.SelectedItem).MaLoai;

                    db.SaveChanges();
                    MessageBox.Show("Cập nhật sách thành công");
                    FillDataGrid(db.SACHes.ToList());
                    ResetControl();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var mss = txtID.Text;
                var book = db.SACHes.Find(mss);
                if (book != null)
                {
                    db.SACHes.Remove(book);
                    db.SaveChanges();
                    MessageBox.Show("Xóa sách thành công");
                    FillDataGrid(db.SACHes.ToList());
                    ResetControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void dgvCRUD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                txtID.Text = dgvCRUD.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = dgvCRUD.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtYear.Text = dgvCRUD.Rows[e.RowIndex].Cells[2].Value.ToString();
                cmbTheloai.SelectedValue = dgvCRUD.Rows[e.RowIndex].Cells[3].Value;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim().ToLower();
            var book = db.SACHes.Where(b => b.MaSach.ToLower().Contains(search) || b.TenSach.ToLower().Contains(search)|| b.NamXB.ToString().Contains(search)).ToList();
            FillDataGrid(book);
        }

        private void thốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void thốngKêTheoNămToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.ShowDialog();
            this.Hide();
        }
    }
}
