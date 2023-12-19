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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
using System.Net;
using System.Diagnostics;
using System.Net.Mail;

namespace Osnov
{
    public partial class mainForm : Form
    {
        DataBase DataBase = new DataBase();
        public string adress;
        public string mail; 
        public string telephone;
        public mainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            textBox23.PasswordChar = '•';
            pictureBox20.Visible = false;
            textBox23.MaxLength = 50;

            LoadData();
            LoadDataProfil();
        }
        private void pictureBox20_Click(object sender, EventArgs e)
        {
            textBox23.UseSystemPasswordChar =  false;
            pictureBox19.Visible = true;
            pictureBox20.Visible = false;
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            textBox23.UseSystemPasswordChar = true;
            pictureBox19.Visible = false;
            pictureBox20.Visible = true;
        }
        private void LoadData() 
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * from Books", DataBase.getConnection());
            DataBase.OpenConnection();

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridView1.DataSource = db.Tables[0];

            DataBase.closeConnection();

            //-----------------------------------------

            SqlDataAdapter dataAdapter2 = new SqlDataAdapter($"SELECT * FROM Orders WHERE login = '{log_in.GetText()}'", DataBase.getConnection()); ;
            DataBase.OpenConnection();

            DataSet db2 = new DataSet();
            dataAdapter2.Fill(db2);
            dataGridView2.DataSource = db2.Tables[0];

            DataBase.closeConnection();
        }
        private void LoadDataProfil()
        {
            textBox22.Text = log_in.GetText();
            textBox21.Text = log_in.GetText();
            textBox23.Text = log_in.GetText1();
            AssignUserDataFromDatabase();

            textBox20.Text = adress;
            label4.Text = adress;
            label5.Text = mail;
            label6.Text = telephone;
        }
        private void AssignUserDataFromDatabase()
        {
            string login = log_in.GetText();

            string query = $"SELECT mail, adress, telephone FROM register WHERE login = '{login}'";

            SqlCommand command = new SqlCommand(query, DataBase.getConnection());
            DataBase.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                mail = reader["mail"].ToString();
                adress = reader["adress"].ToString();
                telephone = reader["telephone"].ToString();
            }

            reader.Close();
            DataBase.closeConnection();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }
        private Boolean checkbook()
        {
            var NameBook = textBox1.Text;
            var AutorBook = textBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select Name, Autor, Date, Genre, Price from Books where Name = '{NameBook}' and Autor = '{AutorBook}'";

            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            adapter.SelectCommand = command;

            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Книга уже существует!");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var NameBook = textBox1.Text;
            var AutorBook = textBox2.Text;
            var DateBook = textBox3.Text;
            var GenreBook = textBox4.Text;
            var PriceBook = textBox5.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"INSERT INTO Books (Name, Autor, Date, Genre, Price) values ('{NameBook}', '{AutorBook}', '{DateBook}', '{GenreBook}', '{PriceBook}')";

            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            DataBase.OpenConnection();

            if (checkbook())
            {
                return;
            }

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Книжка доблена!", "Успех!");
                log_in form_login = new log_in();
                // this.Hide();
            }
            else
            {
                MessageBox.Show("Книжка не добавлена!");
            }
            LoadData();
            DataBase.closeConnection();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Name Like '%{textBox6.Text}%' or Autor Like '%{textBox6.Text}%' or Genre Like '%{textBox6.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Price <= 10";
                    break;

                case 1:
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Price >= 10 and Price <= 50";
                    break;

                case 2:
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Price >= 100";
                    break;

                case 3:
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;    
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox19.Text = row.Cells[0].Value.ToString();
                textBox7.Text = row.Cells[1].Value.ToString();
                textBox8.Text = row.Cells[2].Value.ToString();
                textBox9.Text = row.Cells[3].Value.ToString();
                textBox10.Text = row.Cells[4].Value.ToString();
                textBox11.Text = row.Cells[5].Value.ToString();
                textBox16.Text = row.Cells[1].Value.ToString();
                textBox15.Text = row.Cells[2].Value.ToString();
                textBox14.Text = row.Cells[3].Value.ToString();
                textBox13.Text = row.Cells[4].Value.ToString();
                textBox12.Text = row.Cells[5].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataBase.OpenConnection();

            var Bookid = textBox19.Text;
            var Namelog = textBox21.Text;
            var DateOrder = DateTime.Now.ToString("yyyy.MM.dd");
            var Mesto = adress;
            var Price = textBox18.Text;
            var Kol = textBox17.Text;

            string querystring1 = $"INSERT INTO Orders (login, date, address, bookid, quantity, price) values ('{Namelog}', '{DateOrder}', '{Mesto}', '{Bookid}', '{Kol}', '{Price}')";

            SqlCommand command = new SqlCommand(querystring1, DataBase.getConnection());

            DialogResult result = MessageBox.Show("Подвердите заказ!", "Сообщение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                if (command.ExecuteNonQuery() == 1)
                { 
                    MessageBox.Show("Заказ успешно создан!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mainForm mainForm = new mainForm();
                    mainForm.ShowDialog();
                    this.Close();
                    this.Show();
                }      
            }
            else
            {
                MessageBox.Show("Ну, подумайте ещё..", "Заказ не создан", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadData();
            DataBase.closeConnection();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            int a, b;
            if (int.TryParse(textBox17.Text, out a) && int.TryParse(textBox12.Text, out b))
            {
                textBox18.Text = (a * b).ToString();
            }
            else
            {
                textBox18.Text = "Ошибка";
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text.Length > 10)
            {
                textBox14.Text = textBox14.Text.Substring(0, 10);
                textBox14.SelectionStart = 10;
                textBox14.SelectionLength = 0;
            }
        }


        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var a = log_in.GetText();
            var b = log_in.GetText1();
            adress = textBox24.Text;
            mail = textBox25.Text;
            telephone = textBox26.Text;

            string query = $"UPDATE register SET adress = '{adress}', mail = '{mail}', telephone = '{telephone}' WHERE login = '{a}'";
            SqlCommand command = new SqlCommand(query, DataBase.getConnection());
            DataBase.OpenConnection();
            int rowsAffected = command.ExecuteNonQuery();
            DataBase.closeConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Данные аккаунта успешно обновлены.", "Успех!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Не удалось обновить данные аккаунта.", "Ошибка!");
            }

            LoadData();
        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }  
}
