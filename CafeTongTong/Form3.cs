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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        SqlConnection koneksi;
        string query, penghubung;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataRow dr;
        DataSet ds;
        DataColumn[] dc_1pk = new DataColumn[1];

        private void Konek()
        {
            penghubung = "Data source = localhost\\SQLExpress; Initial Catalog = CafeTong_Tong; Integrated Security = True";
            koneksi = new SqlConnection(penghubung);
            koneksi.Open();
        }

        private void loaddata()
        {
            ds = new DataSet();
            query = "Select * from Masuk ";
            cmd = new SqlCommand(query, koneksi);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Masuk");
            dc_1pk[0] = ds.Tables["Masuk"].Columns[0];
            ds.Tables["Masuk"].PrimaryKey = dc_1pk;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Konek();
            loaddata();
        }

        private void Masuk(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            loaddata();
            dr = ds.Tables["Masuk"].Rows.Find(txtIDKaryawan.Text);
            if (dr != null && dr[0].ToString() == txtIDKaryawan.Text)
            {
                if (dr[1].ToString() == txtPass.Text)
                {
                    frmUtama frmu = new frmUtama();
                    frmu.Show();
                    this.Hide();
                }
                else
                {
                    lblPassword.Text = "Password Incorrect";
                    txtPass.Clear();
                    txtPass.Focus();
                }
            }
            else
            {
                if(MessageBox.Show($"Employee ID {txtIDKaryawan.Text} And Passwordd {txtPass.Text} You Entered Are Incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    txtIDKaryawan.Clear();
                    txtPass.Clear();
                    txtIDKaryawan.Focus();
                }
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda Ingin Keluar ?", "Aplikasi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void chkShowPasss_CheckedChanged_1(object sender, EventArgs e)
        {
            if(chkShowPasss.Checked  == true)
            {
                txtPass.PasswordChar = '\0';
            }
            else
            {
                txtPass.PasswordChar = '*';
            }
        }
    }
}
