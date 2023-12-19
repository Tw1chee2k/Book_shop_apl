using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Osnov
{
    public partial class log_in : Form
    {
        DataBase DataBase = new DataBase();
        public static Func<string> GetText; 
        public static Func<string> GetText1;
        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void log_in_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '•';
            pictureBox1.Visible = false;
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;
        }

        public void button1_Click(object sender, EventArgs e)
        {

            var loginUser = textBox_login.Text;
            var passUser = textBox_password.Text;

            mainForm form2 = new mainForm();
            GetText += () => textBox_login.Text;

            mainForm form22 = new mainForm();
            GetText1 += () => textBox_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id, login, password, adress, mail, telephone from register where login = '{loginUser}' and password = '{passUser}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);


            if (table.Rows.Count == 1)
            {
                if (loginUser != "admin" && passUser != "admin")
                {
                    MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mainForm mainForm = new mainForm();

                    mainForm.ShowDialog();
                    this.Close();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Вход в админ панель!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formadmin formadmin = new formadmin();
                    formadmin.ShowDialog();
                    this.Close();
                    this.Show();
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует! / Неверный пароль!", "Аккаунта не существует! / Неверный пароль!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox_login.Text = "";
            textBox_password.Text = "";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;

            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void Ссылка_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up sign_up = new sign_up();
            sign_up.Show();

            log_in log_in = new log_in();
            log_in.Close(); 
        }

        private void log_in_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
