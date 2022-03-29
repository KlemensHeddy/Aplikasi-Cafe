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
    public partial class frmReservasi : Form
    {
        public frmReservasi()
        {
            InitializeComponent();
        }

        SqlConnection koneksi;
        string query, penghubung;
        SqlCommand cmd;
        SqlDataReader baca;
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
            query = "Select * From Reservasi";
            cmd = new SqlCommand(query, koneksi);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Reservasi");
            dc_1pk[0] = ds.Tables["Reservasi"].Columns[0];
            ds.Tables["Reservasi"].PrimaryKey = dc_1pk;
        }

        private void UpdateData()
        {
            cb = new SqlCommandBuilder(da);
            da = cb.DataAdapter;
            da.Update(ds.Tables["Reservasi"]);
        }

        private void Tampil()
        {
            dgvMember.DataSource = ds.Tables["Reservasi"];
            dgvMember.Columns[0].HeaderText = "Booking Code";
            dgvMember.Columns[1].HeaderText = "ID Meja";
            dgvMember.Columns[2].HeaderText = "Desk Type";
            dgvMember.Columns[3].HeaderText = "Price";
            dgvMember.Columns[4].HeaderText = "Status";
            dgvMember.Columns[5].HeaderText = "ID Custumer";
            dgvMember.Columns[6].HeaderText = "Full Name";
            dgvMember.Columns[7].HeaderText = "Booking Date";
            dgvMember.Columns[8].HeaderText = "Note";
            dgvMember.AllowUserToAddRows = false;
            dgvMember.ReadOnly = true;
            dgvMember.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMember.MultiSelect = false;
            dgvMember.RowHeadersVisible = false;
        }
        private void btnRefreshCust_Click(object sender, EventArgs e)
        {
            int diskon = 0;
            nudIDCust.Value = 0;
            txtFullName.Clear();
            dtpBD.Value = DateTime.Now;
            txtNote.Clear();
        }

        private void btnBackMain_Click(object sender, EventArgs e)
        {
            frmUtama frmu = new frmUtama();
            frmu.Show();
            this.Hide();
        }

        private void kosong()
        {
            int diskon = 0;
            nudBC.Value = 0;
            nudIDTable.Value = 0;
            lblDT.Text = "";
            lblPrice.Text = "";
            lblStatus.Text = "";
            nudIDCust.Value = 0;
            txtFullName.Clear();
            dtpBD.Value = DateTime.Now;
            txtNote.Clear();
        }

        private void btnrefreshDT_Click(object sender, EventArgs e)
        {
            nudBC.Value = 0;
            nudIDTable.Value = 0;
            lblDT.Text = "";
            lblPrice.Text = "";
            lblStatus.Text = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblDT.Text) && !string.IsNullOrWhiteSpace(lblPrice.Text) && !string.IsNullOrWhiteSpace(txtFullName.Text) && dtpBD.Value != null)
            {
                LoadData();
                dr = ds.Tables["Reservasi"].Rows.Find(nudBC.Value);
                if (dr != null)
                {
                    dr[0] = nudBC.Value;
                    dr[1] = nudIDTable.Value = 0;
                    dr[2] = lblDT.Text;
                    dr[3] = lblPrice.Text;
                    dr[4] = lblStatus.Text;
                    dr[5] = nudIDCust.Value;
                    dr[6] = txtFullName.Text;
                    dr[7] = dtpBD.Value;
                    dr[8] = txtNote.Text;
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudBC.Value} was successfully saved");
                    kosong();
                    Tampil();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudBC.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblDT.Text) && !string.IsNullOrWhiteSpace(lblPrice.Text) && !string.IsNullOrWhiteSpace(txtFullName.Text) && dtpBD.Value != null)
            {
                LoadData();
                dr = ds.Tables["Reservasi"].Rows.Find(nudBC.Value);
                if (dr != null)
                {
                    dr.Delete();
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudBC.Value} has been successfully deleted");
                    kosong();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudBC.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMember_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = dgvMember.CurrentCell.RowIndex;

            nudBC.Value = decimal.Parse(dgvMember.Rows[baris].Cells[0].Value.ToString());
            nudIDTable.Value = decimal.Parse(dgvMember.Rows[baris].Cells[1].Value.ToString());
            lblDT.Text = dgvMember.Rows[baris].Cells[2].Value.ToString();
            lblPrice.Text = dgvMember.Rows[baris].Cells[3].Value.ToString();
            lblStatus.Text = dgvMember.Rows[baris].Cells[4].Value.ToString();
            nudIDCust.Value = decimal.Parse(dgvMember.Rows[baris].Cells[5].Value.ToString());
            txtFullName.Text = dgvMember.Rows[baris].Cells[6].Value.ToString();
            dtpBD.Value = DateTime.Parse(dgvMember.Rows[baris].Cells[7].Value.ToString());
            txtNote.Text = dgvMember.Rows[baris].Cells[8].Value.ToString();
        }

        private void btnCariTable_Click(object sender, EventArgs e)
        {
            Koneksi();
            query = $"Select * From DataMeja Where IDMeja = '{nudIDTable.Value}'";
            cmd = new SqlCommand(query, koneksi);
            if (nudIDTable.Value != null)
            {
                baca = cmd.ExecuteReader();
                if (baca.Read())
                {
                    lblDT.Text = (baca[1].ToString());
                    lblPrice.Text = (baca[2].ToString());
                    lblStatus.Text = "Full";
                }
            }
            koneksi.Close();
        }

        private void txtKodeBooking_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtIDCust_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        int diskon = 0;
        private void btnCriCust_Click(object sender, EventArgs e)
        {
            Koneksi();
            query = $"Select * From DaftarMember Where IDCustumer = '{nudIDCust.Value}'";
            cmd = new SqlCommand(query, koneksi);
            if (nudIDCust.Value != null)
            {
                baca = cmd.ExecuteReader();
                if (baca.Read())
                {
                    txtFullName.Text = (baca[1].ToString());
                    txtFullName.Enabled = false;
                }
            }
            koneksi.Close();
        }

        private void txtNote_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar)  && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCari_Click(object sender, EventArgs e)
        {

            if (rdoBookingCode.Checked == true && !string.IsNullOrWhiteSpace(txtFullNameandID.Text))
            {
                ds = new DataSet();
                query = $"Select * From Reservasi where KodeBooking = '{txtFullNameandID.Text}'";
                cmd = new SqlCommand(query, koneksi);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Reservasi");
                dc_1pk[0] = ds.Tables["Reservasi"].Columns[0];
                ds.Tables["Reservasi"].PrimaryKey = dc_1pk;
                Tampil();
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            rdoBookingCode.Checked = false;
            txtFullNameandID.Clear();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if ( !string.IsNullOrWhiteSpace(lblDT.Text) && !string.IsNullOrWhiteSpace(lblPrice.Text) && !string.IsNullOrWhiteSpace(txtFullName.Text) && dtpBD.Value != null)
            {
                LoadData();
                dr = ds.Tables["Reservasi"].Rows.Find(nudBC.Value);
                if (dr == null)
                {
                    dr = ds.Tables["Reservasi"].NewRow();
                    dr[0] = nudBC.Value;
                    dr[1] = nudIDTable.Value;
                    dr[2] = lblDT.Text;
                    dr[3] = lblPrice.Text;
                    dr[4] = lblStatus.Text;
                    dr[5] = nudIDCust.Value;
                    dr[6] = txtFullName.Text;
                    dr[7] = dtpBD.Value;
                    dr[8] = txtNote.Text;
                    ds.Tables["Reservasi"].Rows.Add(dr);
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudBC.Value} was successfully saved");
                    kosong();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudBC.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
