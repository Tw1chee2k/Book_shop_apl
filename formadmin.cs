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
using System.Reflection.Emit;

namespace Osnov
{
    public partial class formadmin : Form
    {
        DataBase DataBase = new DataBase();
        public string adress;
        public string mail;
        public string telephone;

        public formadmin()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void formadmin_Load(object sender, EventArgs e)
        {
            textBox39.PasswordChar = '•';
            pictureBox33.Visible = false;
            textBox39.MaxLength = 50;

            LoadData();
            LoadDataProfil();
        }
        private void pictureBox33_Click(object sender, EventArgs e)
        {
            textBox39.UseSystemPasswordChar = false;
            pictureBox32.Visible = true;
            pictureBox33.Visible = false;
        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {
            textBox39.UseSystemPasswordChar = true;
            pictureBox32.Visible = false;
            pictureBox33.Visible = true;
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

            SqlDataAdapter dataAdapter1 = new SqlDataAdapter("Select * from Orders", DataBase.getConnection());
            DataBase.OpenConnection();

            DataSet db1 = new DataSet();
            dataAdapter1.Fill(db1);
            dataGridView2.DataSource = db1.Tables[0];

            DataBase.closeConnection();

            //-----------------------------------------

            SqlDataAdapter dataAdapter3 = new SqlDataAdapter("Select * from register", DataBase.getConnection());
            DataBase.OpenConnection();

            DataSet db3 = new DataSet();
            dataAdapter3.Fill(db3);
            dataGridView3.DataSource = db3.Tables[0];
            DataBase.closeConnection();
        }
        private void LoadDataProfil()
        {
            textBox21.Text = log_in.GetText();
            textBox40.Text = log_in.GetText();
            textBox39.Text = log_in.GetText1();
            AssignUserDataFromDatabase();

            textBox20.Text = adress;
            label4.Text = adress;
            label5.Text = mail;
            label6.Text = telephone;
        }
        private void AssignUserDataFromDatabase()
        {
            string login = log_in.GetText(); // Получаем значение логина пользователя

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

        private void button1_Click_1(object sender, EventArgs e)
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
        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                double result1 = Convert.ToDouble(textBox26.Text);
                if (textBox26.Text != "")
                {
                    DataBase.OpenConnection();

                    string querystring2 = $"DELETE FROM Books WHERE id = {textBox26.Text}";
                    SqlCommand command = new SqlCommand(querystring2, DataBase.getConnection());

                    DialogResult result = MessageBox.Show($"Подвердите удаление книги с id {textBox26.Text}!", "Сообщение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information); ;

                    if (result == DialogResult.OK)
                    {
                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show($"Книга с id {textBox26.Text} удалена!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formadmin formadmin = new formadmin();

                            formadmin.ShowDialog();
                            this.Close();
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Книга с id {textBox26.Text} не была удалена!", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    DataBase.closeConnection();
                }

                else
                {
                    MessageBox.Show($"Введите id книги для удаления", "Пусто!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Введите id", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox22.Text) && !string.IsNullOrEmpty(textBox24.Text))
                {
                    DataBase.OpenConnection();

                    string querystring3 = $"DELETE FROM Orders WHERE login = '{textBox22.Text}' and bookid = {textBox24.Text} and address = '{textBox25.Text}'";
                    SqlCommand command = new SqlCommand(querystring3, DataBase.getConnection());

                    DialogResult result = MessageBox.Show($"Подтвердите удаление заказа с login = {textBox22.Text} и bookid = {textBox22.Text}! и address = {textBox25.Text}", "Сообщение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.OK)
                    {
                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show($"Удаление заказа с login = {textBox22.Text} и bookid = {textBox22.Text} и address = {textBox25.Text} произошло успешно", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formadmin formadmin = new formadmin();

                            LoadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Отмена!", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"Введите id книги и имя пользователя для удаления заказа", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {

                DataBase.closeConnection();
            }
            LoadData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox23.Text != "")
            {
                DataBase.OpenConnection();

                string querystring2 = $"DELETE FROM register WHERE id_user = {textBox23.Text}";
                SqlCommand command = new SqlCommand(querystring2, DataBase.getConnection());

                DialogResult result = MessageBox.Show($"Подвердите удаление аккаунта с id = {textBox23.Text}", "Сообщение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information); ;

                if (result == DialogResult.OK)
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show($"Удаление аккаунта с id = {textBox23.Text}", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formadmin formadmin = new formadmin();
                        formadmin.ShowDialog();
                        this.Close();
                        this.Show();
                    }
                }
                else
                {
                    MessageBox.Show($"Заказ не удалён!", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                DataBase.closeConnection();
            }
            else
            {
                MessageBox.Show($"Введите id аккаунта для удаления", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadData();
        }

        private void pictureBox16_Click_1(object sender, EventArgs e)
        {
            textBox26.Text = "";
            textBox23.Text = "";
            textBox22.Text = "";
            textBox24.Text = "";
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Name Like '%{textBox6.Text}%' or Autor Like '%{textBox6.Text}%' or Genre Like '%{textBox6.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
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

        private void dataGridView1_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBox21.Text = log_in.GetText();
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            DataBase.OpenConnection();
            var Bookid = textBox19.Text;

            textBox21.Text = log_in.GetText();

            var Namelog = textBox21.Text;

            var DateOrder = DateTime.Now.ToString("yyyy.MM.dd");

            var Mesto = textBox20.Text;
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

        private void button2_Click(object sender, EventArgs e)
        {
            var id = textBox27.Text;
            var a = textBox32.Text;
            var b = textBox31.Text;
            var c = textBox30.Text;
            var d = textBox29.Text; 
            var cc = textBox28.Text;

            string query = $"UPDATE Books SET Name = '{a}', Autor = '{b}', Date = '{c}', Genre = '{d}', Price = '{cc}' WHERE id = '{id}'";
            SqlCommand command = new SqlCommand(query, DataBase.getConnection());
            DataBase.OpenConnection();
            int rowsAffected = command.ExecuteNonQuery();
            DataBase.closeConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Данные книги успешно обновлены.", "Успех!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Не удалось обновить данные книги.", "Ошибка!");
            }
            LoadData();
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            textBox27.Text = "";
            textBox32.Text = "";
            textBox31.Text = "";
            textBox30.Text = ""; 
            textBox29.Text = ""; 
            textBox28.Text = "";
            textBox34.Text = "";
            textBox33.Text = "";
            textBox35.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var id = textBox34.Text;
            var a = textBox33.Text;
            var b = textBox35.Text;
           

            string query = $"UPDATE register SET login = '{a}', password = '{b}' WHERE id = '{id}'";
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

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text.Length > 10)
            {
                textBox14.Text = textBox14.Text.Substring(0, 10);
                textBox14.SelectionStart = 10;
                textBox14.SelectionLength = 0;
            }
        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {

        }
    }
}