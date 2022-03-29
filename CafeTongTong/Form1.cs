using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeTongTong
{
    public partial class frmUtama : Form
    {
        public frmUtama()
        {
            InitializeComponent();
        }

        private void frmUtama_Load(object sender, EventArgs e)
        {
            timer.Text = DateTime.Now.ToString();
            tim.Interval = 1000;
            tim.Enabled = true;
        }

        private void About()
        {
            frmAbout frma = new frmAbout();
            frma.Show();
            this.Hide();
            frma.lblAbout.Text = "About\n";
            frma.lblAbout.Text += $"\n";
            frma.lblAbout.Text += "Tong-Tong Team\n";
            frma.lblAbout.Text += $"Beta Version\n";
            frma.lblAbout.Text += $"Cafe Tong-Tong Application\n";
            frma.lblAbout.Text += "2020";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            About();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            About();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            About();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            About();
        }

        private void tim_Tick(object sender, EventArgs e)
        {
            timer.Text = DateTime.Now.ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            frmMenu frmu = new frmMenu();
            frmu.Show();
            this.Hide();
        }

        private void timer_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmRegister frmr = new frmRegister();
            frmr.Show();
            this.Close();
        }

        private void picUserr_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda Ingin Log Out ?", "Aplikasi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            frmReservasi frmreser = new frmReservasi();
            frmreser.Show();
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            frmEmployee frme = new frmEmployee();
            frme.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda Ingin Keluar ?", "Aplikasi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

    }
}
