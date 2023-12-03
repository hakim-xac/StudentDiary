using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StudentDiary
{
    public partial class AddingNote : Form
    {
        private String _db_file_name;

        public AddingNote()
        {
            InitializeComponent();
        }

        private void AddingNote_Load(object sender, EventArgs e)
        {
            _db_file_name = "StudentDiary.db";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(!СheckInputData())
            {
                MessageBox.Show("Вы заполнили не все поля ввода!");
                return;
            }

            if (!AddedToDataBase())
            {
                MessageBox.Show("Ошибка добавления данных в базу!");
                return;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private bool AddedToDataBase()
        {
            if (!File.Exists(_db_file_name))
                SQLiteConnection.CreateFile(_db_file_name);

            try
            {
                SQLiteConnection _db_connect = new SQLiteConnection($"Data Source={_db_file_name};Version=3;");
                _db_connect.Open();

                String start_datetime = dateTimePicker1.Value.ToString("dd.MM.yyyy HH:mm");
                String end_datetime = dateTimePicker2.Value.ToString("dd.MM.yyyy HH:mm");

                SQLiteCommand _sql_cmd = new SQLiteCommand($"insert INTO ListNotes (header, note, start_datetime, end_datetime)" +
                    $" values(\"{textBox2.Text}\", \"{textBox1.Text}\", \"{start_datetime}\", \"{end_datetime}\")");

                _sql_cmd.Connection = _db_connect;
                _sql_cmd.ExecuteNonQuery();

                _db_connect.Close();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool СheckInputData()
        {
            return textBox2.Text.Length > 0;
        }
    }
}
