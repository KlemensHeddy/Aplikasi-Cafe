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
using System.IO;

namespace CafeTongTong
{
    public partial class frmMenu : Form
    {
        public frmMenu()
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
            query = "Select * From Transaksi";
            cmd = new SqlCommand(query, koneksi);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Transaksi");
            dc_1pk[0] = ds.Tables["Transaksi"].Columns[0];
            ds.Tables["Transaksi"].PrimaryKey = dc_1pk;
        }

        private void UpdateData()
        {
            cb = new SqlCommandBuilder(da);
            da = cb.DataAdapter;
            da.Update(ds.Tables["Transaksi"]);
        }

        private void Tampil()
        {
            dgvTransaction.DataSource = ds.Tables["Transaksi"];
            dgvTransaction.Columns[0].HeaderText = "ID Transaction";
            dgvTransaction.Columns[1].HeaderText = "ID Menu";
            dgvTransaction.Columns[2].HeaderText = "Menu";
            dgvTransaction.Columns[3].HeaderText = "Price";
            dgvTransaction.Columns[4].HeaderText = "Quantity";
            dgvTransaction.Columns[5].HeaderText = "Id Custumer";
            dgvTransaction.Columns[6].HeaderText = "Full Name";
            dgvTransaction.Columns[7].HeaderText = "Order Date";
            dgvTransaction.Columns[8].HeaderText = "Note";
            dgvTransaction.Columns[9].HeaderText = "ID Meja";
            dgvTransaction.Columns[10].HeaderText = "Desk Type";
            dgvTransaction.Columns[11].HeaderText = "Status";
            dgvTransaction.AllowUserToAddRows = false;
            dgvTransaction.ReadOnly = true;
            dgvTransaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransaction.MultiSelect = false;
            dgvTransaction.RowHeadersVisible = false;
        }

        private void btnBackMain_Click(object sender, EventArgs e)
        {
            frmUtama frmu = new frmUtama();
            frmu.Show();
            this.Hide();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            Koneksi();
            LoadData();
        }

        int diskon = 0;
        private void btnRefreshMenu_Click(object sender, EventArgs e)
        {
            nudIDTrans.Value = 0;
            nudIDmenu.Value = 0;
            lblMenu.Text = "";
            lblharga.Text = "";
            nudQy.Value = 0;
        }

        private void btnCarimenu_Click(object sender, EventArgs e)
        {
            Koneksi();
            query = $"Select * From DataMenu Where IDMenu = '{nudIDmenu.Value}'";
            cmd = new SqlCommand(query, koneksi);
            if (nudIDmenu.Value != null)
            {
                baca = cmd.ExecuteReader();
                if (baca.Read())
                {
                    lblMenu.Text = (baca[1].ToString());
                    lblharga.Text = (baca[3].ToString());
                }
            }
            koneksi.Close();
        }

        private void btnCariCust_Click(object sender, EventArgs e)
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

        private void Kosong()
        {
            nudIDTrans.Value = 0;
            nudIDmenu.Value = 0;
            lblMenu.Text = "";
            lblharga.Text = "";
            nudQy.Value = 0;
            nudIDCust.Value = 0;
            txtFullName.Clear();
            dtpBD.Value = DateTime.Now;
            txtNote.Clear();
            nudIDTable.Value = 0;
            lblDT.Text = "";
            lblStatus.Text = "";
        }
        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblMenu.Text) && !string.IsNullOrWhiteSpace(lblharga.Text) && !string.IsNullOrWhiteSpace(txtFullName.Text) && dtpBD.Value != null)
            {
                LoadData();
                dr = ds.Tables["Transaksi"].Rows.Find(nudIDTrans.Value);
                if (dr == null)
                {
                    dr = ds.Tables["Transaksi"].NewRow();
                    dr[0] = nudIDTrans.Value;
                    dr[1] = nudIDmenu.Value;
                    dr[2] = lblMenu.Text;
                    dr[3] = lblharga.Text;
                    dr[4] = nudQy.Value;
                    dr[5] = nudIDCust.Value;
                    dr[6] = txtFullName.Text;
                    dr[7] = dtpBD.Value;
                    dr[8] = txtNote.Text;
                    dr[9] = nudIDmenu.Value;
                    dr[10] = lblDT.Text;
                    dr[11] = lblStatus.Text;
                    ds.Tables["Transaksi"].Rows.Add(dr);
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudIDTrans.Value} was successfully saved");
                    Kosong();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudIDTrans.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblMenu.Text) && !string.IsNullOrWhiteSpace(lblharga.Text) && !string.IsNullOrWhiteSpace(txtFullName.Text) && dtpBD.Value != null)
            {
                LoadData();
                dr = ds.Tables["Transaksi"].Rows.Find(nudIDTrans.Value);
                if (dr != null)
                {
                    dr[0] = nudIDTrans.Value;
                    dr[1] = nudIDmenu.Value;
                    dr[2] = lblMenu.Text;
                    dr[3] = lblharga.Text;
                    dr[4] = nudQy.Value;
                    dr[5] = nudIDCust.Value;
                    dr[6] = txtFullName.Text;
                    dr[7] = dtpBD.Value;
                    dr[8] = txtNote.Text;
                    dr[9] = nudIDmenu.Value;
                    dr[10] = lblDT.Text;
                    dr[11] = lblStatus.Text;
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudIDTrans.Value} was successfully saved");
                    Kosong();
                    Tampil();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudIDTrans.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblMenu.Text) && !string.IsNullOrWhiteSpace(lblharga.Text) && !string.IsNullOrWhiteSpace(txtFullName.Text) && dtpBD.Value != null)
            {
                LoadData();
                dr = ds.Tables["Transaksi"].Rows.Find(nudIDTrans.Value);
                if (dr != null)
                {
                    dr.Delete();
                    UpdateData();
                    MessageBox.Show($"The Data That You Input With the {nudIDTrans.Value} was successfully saved");
                    Kosong();
                }
                else
                {
                    MessageBox.Show($"The Data That You Input With the {nudIDTrans.Value} already exists");
                }
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTransaction_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = dgvTransaction.CurrentCell.RowIndex;

            nudIDTrans.Value = decimal.Parse(dgvTransaction.Rows[baris].Cells[0].Value.ToString());
            nudIDmenu.Value = decimal.Parse(dgvTransaction.Rows[baris].Cells[1].Value.ToString());
            lblMenu.Text = dgvTransaction.Rows[baris].Cells[2].Value.ToString();
            lblharga.Text = dgvTransaction.Rows[baris].Cells[3].Value.ToString();
            nudQy.Value = decimal.Parse(dgvTransaction.Rows[baris].Cells[4].Value.ToString());
            nudIDCust.Value = decimal.Parse(dgvTransaction.Rows[baris].Cells[5].Value.ToString());
            txtFullName.Text = dgvTransaction.Rows[baris].Cells[6].Value.ToString();
            dtpBD.Value = DateTime.Parse(dgvTransaction.Rows[baris].Cells[7].Value.ToString());
            txtNote.Text = dgvTransaction.Rows[baris].Cells[8].Value.ToString();
            nudIDTable.Value = decimal.Parse(dgvTransaction.Rows[baris].Cells[9].Value.ToString());
            lblDT.Text = dgvTransaction.Rows[baris].Cells[10].Value.ToString();
        }

        private void btnrefreshDT_Click(object sender, EventArgs e)
        {
            nudIDTable.Value = 0;
            lblDT.Text = "";
            lblStatus.Text = "";
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
                    lblStatus.Text = "Full";
                }
            }
            koneksi.Close();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            rdoBookingCode.Checked = false;
            txtFullNameandID.Clear();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (rdoBookingCode.Checked == true && !string.IsNullOrWhiteSpace(txtFullNameandID.Text))
            {
                ds = new DataSet();
                query = $"Select * From Transaksi where IDTransaksi = '{txtFullNameandID.Text}'";
                cmd = new SqlCommand(query, koneksi);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Transaksi");
                dc_1pk[0] = ds.Tables["Transaksi"].Columns[0];
                ds.Tables["Transaksi"].PrimaryKey = dc_1pk;
                Tampil();
            }
            else
            {
                MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshCust_Click(object sender, EventArgs e)
        {
            nudIDCust.Value = 0;
            txtFullName.Clear();
            dtpBD.Value = DateTime.Now;
            txtNote.Clear();
        }
    }
}
