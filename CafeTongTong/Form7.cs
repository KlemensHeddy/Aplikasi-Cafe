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
    public partial class frmEmployee : Form
    {
        public frmEmployee()
        {
            InitializeComponent();
        }

        private void frmEmployee_Load_1(object sender, EventArgs e)
        {
            rdoLL.Enabled = false;
            rdoP.Enabled = false;
        }

        private void btnBackMain_Click(object sender, EventArgs e)
        {
            frmUtama frmu = new frmUtama();
            frmu.Show();
            this.Close();
        }

        string query, jk;
        private void btnCari_Click(object sender, EventArgs e)
        {
            picPP.SizeMode = PictureBoxSizeMode.Zoom;
            if (rdoIDEmployee.Checked == true || rdoEmployee.Checked == true && !string.IsNullOrWhiteSpace(txtFullNameandIDEmployee.Text))
            {
                String source = "Data Source = localhost\\SQLExpress; Initial Catalog = CafeTong_Tong; Integrated Security = True";
                SqlConnection koneksi = new SqlConnection(source);
                koneksi.Open();

                if (rdoIDEmployee.Checked)
                {
                    query = $"Select * From Karyawan where EmployeeID = '{txtFullNameandIDEmployee.Text}' ";
                }
                else if (rdoEmployee.Checked)
                {
                    query = $"Select * From Karyawan where NamaKaryawan = '{txtFullNameandIDEmployee.Text}'";
                }
                SqlCommand cmd = new SqlCommand(query, koneksi);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblIDEmployee.Text = (dr[0].ToString());
                    lblEmployeeName.Text = (dr[1].ToString());
                    byte[] img = (byte[])dr[2];

                    MemoryStream ms = new MemoryStream(img);
                    picPP.Image = Image.FromStream(ms);

                    if (lblIDEmployee.Text == "11223344")
                    {
                        rdoLL.Checked = true;
                    }
                    else if (lblIDEmployee.Text == "44332211")
                    {
                        rdoP.Checked = true;
                    }
                    else if (lblIDEmployee.Text == "44223311")
                    {
                        rdoP.Checked = true;
                    }
                    lblAddresEmployee.Text = (dr[4].ToString());
                    lblPN.Text = (dr[5].ToString());

                    koneksi.Close();
                }
                else
                {
                    MessageBox.Show("Data Is Reqired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            rdoEmployee.Checked = false;
            rdoIDEmployee.Checked = false;
            txtFullNameandIDEmployee.Clear();
            lblIDEmployee.Text = "";
            rdoLL.Checked = false;
            rdoP.Checked = false;
            lblEmployeeName.Text = "";
            lblAddresEmployee.Text = "";
            lblPN.Text = "";
            picPP.Image = null;
        }
    }
}
