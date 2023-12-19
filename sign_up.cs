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
using System.Drawing.Text;
using Microsoft.Win32;

namespace Osnov
{
    public partial class sign_up : Form
    {
        DataBase DataBase = new DataBase();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void sign_up_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '•';
            pictureBox4.Visible = false;  
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;
        }
        private Boolean checkuser()
        {
            var login = textBox_login.Text;

            var sql = "SELECT COUNT(*) FROM register WHERE login = @login";

            using (var command = new SqlCommand(sql, DataBase.getConnection()))
            {
                command.Parameters.AddWithValue("@login", login);

                DataBase.OpenConnection();
                var count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован!", "Ошибка!");
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var login = textBox_login.Text;
            var password = textBox_password.Text;

            string querystring = $"insert into register(login, password) values ('{login}', '{password}')";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            DataBase.OpenConnection();

            if (checkuser())
            {
                return;
            }

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");
                log_in form_login = new log_in();
                //  this.Hide();
                form_login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }
            DataBase.closeConnection();
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = true;
            pictureBox4.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = false;
            pictureBox4.Visible = false;
            pictureBox1.Visible = true;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
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
            this.Close();
            log_in log_in = new log_in();
            
            log_in.Show();  
        }


    }
}
