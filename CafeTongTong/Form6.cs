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

namespace CafeTongTong
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        SqlConnection koneksi;
        string query, penghubung;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        DataColumn[] dc_1pk = new DataColumn[1];
        DataRow dr;
        SqlCommandBuilder cb;

        private void Koneksi()
        {
            penghubung = "Data Source = localhost\\SQLExpress; Initial Catalog = CafeTong_Tong; Integrated Security = True";
            koneksi = new SqlConnection(penghubung);
            koneksi.Open();
        }

        private void LoadData()
        {
            ds = new DataSet();
            query = "Select * From DaftarMember";
            cmd = new SqlCommand(query, koneksi);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Member");
            dc_1pk[0] = ds.Tables["Member"].Columns[0];
            ds.Tables["Member"].PrimaryKey = dc_1pk;
        }

        private void UpdateData()
        {
            cb = new SqlCommandBuilder(da);
            da = cb.DataAdapter;
            da.Update(ds.Tables["Member"]);
        }

        private void Tampil()
        {
            dgvMember.DataSource = ds.Tables["Member"];
            dgvMember.Columns[0].HeaderText = "ID Member";
            dgvMember.Columns[1].HeaderText = "Name Member";
            dgvMember.Columns[2].HeaderText = "Gender";
            dgvMember.Columns[3].HeaderText = "Register Date";
            dgvMember.Columns[4].HeaderText = "Address";
            dgvMember.AllowUserToAddRows = false;
            dgvMember.ReadOnly = true;
            dgvMember.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMember.MultiSelect = false;
            dgvMember.RowHeadersVisible = false;
        }

        private void btnBackMain_Click(object sender, EventArgs e)
        {
            frmUtama frmu = new frmUtama();
            frmu.Show();
            this.Close();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            Koneksi();
            LoadData();
            nudMember.Maximum = 1000;
        }

        private void kosong()
        {
            nudMember.Value = 0;
            txtNamalengkap.Clear();
            rdoLL.Checked = false;
            rdoP.Checked = false;
            dtpTD.Value = DateTime.Now;
            txtAlamat.Clear();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (nudMember.Value != 0 && !string.IsNullOrWhiteSpace(txtNamalengkap.Text) && (rdoLL.Checked == true || rdoP.Checked == true) && dtpTD.Value != null && !string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                LoadData();
                dr = ds.Tables["Member"].Rows.Find(nudMember.Value);
                if (dr == null)
                {
                    dr = ds.Tables["Member"].NewRow();
                    dr[0] = nudMember.Value;
                    dr[1] = txtNamalengkap.Text;
                    if (rdoLL.Checked == true)
                    {
                        dr[2] = rdoLL.Text;
                    }
                    else
                    {
                        dr[2] = rdoP.Text;
                    }
                    dr[3] = dtpTD.Value;
                    dr[4] = txtAlamat.Text;
                    ds.Tables["Member"].Rows.Add(dr);
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudMember.Value} was successfully saved");
                    kosong();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudMember.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(nudMember.Value != 0 && !string.IsNullOrWhiteSpace(txtNamalengkap.Text) && (rdoLL.Checked == true || rdoP.Checked == true) && dtpTD.Value != null && !string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                LoadData();
                dr = ds.Tables["Member"].Rows.Find(nudMember.Value);
                if (dr != null)
                {
                    dr.Delete();
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudMember.Value} has been successfully deleted");
                    txtNamalengkap.Clear();
                    kosong();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudMember.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if(rdoCust.Checked == true || rdoIDMember.Checked == true && !string.IsNullOrWhiteSpace(txtFullNameandID.Text))
            {
                ds = new DataSet();
                if (rdoIDMember.Checked)
                {
                    query = $"Select * From DaftarMember where IDCustumer = '{txtFullNameandID.Text}'";
                }
                else if (rdoCust.Checked)
                {
                    query = $"Select * From DaftarMember where NamaCustumer = '{txtFullNameandID.Text}'";
                }
                cmd = new SqlCommand(query, koneksi);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Member");
                dc_1pk[0] = ds.Tables["Member"].Columns[0];
                ds.Tables["Member"].PrimaryKey = dc_1pk;
                Tampil();
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMember_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = dgvMember.CurrentCell.RowIndex;

            nudMember.Value = decimal.Parse(dgvMember.Rows[baris].Cells[0].Value.ToString());
            txtNamalengkap.Text = dgvMember.Rows[baris].Cells[1].Value.ToString();
            if (dgvMember.Rows[baris].Cells[2].Value.ToString() == "Female")
            {
                rdoP.Checked = true;
            }
            else
            {
                rdoLL.Checked = true;
            }
            dtpTD.Value = DateTime.Parse(dgvMember.Rows[baris].Cells[3].Value.ToString());
            txtAlamat.Text = dgvMember.Rows[baris].Cells[4].Value.ToString();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            Tampil();
            txtFullNameandID.Clear();
            rdoIDMember.Checked = false;
            rdoCust.Checked = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(nudMember.Value != 0 && !string.IsNullOrWhiteSpace(txtNamalengkap.Text) && (rdoLL.Checked == true || rdoP.Checked == true) && dtpTD.Value != null && !string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                LoadData();
                dr = ds.Tables["Member"].Rows.Find(nudMember.Value);
                if (dr != null)
                {
                    dr[1] = txtNamalengkap.Text;
                    if (rdoLL.Checked == true)
                    {
                        dr[2] = rdoLL.Text;
                    }
                    else
                    {
                        dr[2] = rdoP.Text;
                    }
                    dr[3] = dtpTD.Value;
                    dr[4] = txtAlamat.Text;
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudMember.Value} was successfully changed");
                    kosong();
                    Tampil();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudMember.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
