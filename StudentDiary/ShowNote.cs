using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Messaging;
using System.Reflection.Emit;

namespace StudentDiary
{
    public partial class ShowNote : Form
    {
        private String _db_file_name;
        private String _tmp_note;
        private String _tmp_start_datetime;
        private String _tmp_end_datetime;
        public String id;

        public ShowNote()
        {
            InitializeComponent();
        }

        private void ShowNote_Load(object sender, EventArgs e)
        {
            _db_file_name = "StudentDiary.db";
        }

        private bool loadFromDataBase()
        {
            button1.Enabled = false;

            if (!File.Exists(_db_file_name))
            {
                MessageBox.Show("Ошибка чтения базы данных!");
                return false;
            }
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out int _) == false)
            {
                MessageBox.Show("Ошибка входных данных!");
                return false;
            }

            try
            {
                SQLiteConnection db_connect = new SQLiteConnection($"Data Source={_db_file_name};Version=3;");
                db_connect.Open();
                SQLiteCommand _sql_cmd = new SQLiteCommand($"SELECT * FROM ListNotes where id={id}");

                _sql_cmd.Connection = db_connect;
                _sql_cmd.ExecuteNonQuery();
                db_connect.Close();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(_sql_cmd);

                DataTable data = new DataTable();
                adapter.Fill(data);

                if (data.Rows.Count != 1)
                {
                    MessageBox.Show("Ошибка чтения базы данных!");
                    return false;
                }

                var item = data.Rows[0].ItemArray;

                _tmp_note = item[2].ToString();
                _tmp_start_datetime = item[3].ToString();
                _tmp_end_datetime = item[4].ToString();

                label1.Text = item[1].ToString();
                dateTimePicker1.Value = DateTime.Parse(_tmp_start_datetime);
                dateTimePicker2.Value = DateTime.Parse(_tmp_end_datetime);
                textBox3.Text = _tmp_note;

                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }

        }

        private bool UpdateItemInBase()
        {
            if (!File.Exists(_db_file_name))
            {
                MessageBox.Show("Ошибка обновления данных!");
                return false;
            }

            try
            {
                SQLiteConnection _db_connect = new SQLiteConnection($"Data Source={_db_file_name};Version=3;");
                _db_connect.Open();

                String start_datetime = dateTimePicker1.Value.ToString("dd.MM.yyyy HH:mm");
                String end_datetime = dateTimePicker2.Value.ToString("dd.MM.yyyy HH:mm");

                SQLiteCommand _sql_cmd = new SQLiteCommand($"update ListNotes set note = \"{textBox3.Text}\", " +
                    $"start_datetime = \"{start_datetime}\", " +
                    $"end_datetime = \"{end_datetime}\"" +
                    $" where id=\"{id}\"");
                                
                _sql_cmd.Connection = _db_connect;
                int is_update_count = _sql_cmd.ExecuteNonQuery();
                _db_connect.Close();

                if(is_update_count != 1)
                {
                    MessageBox.Show("Ошибка записи в базу данных!");
                    return false;
                }

                MessageBox.Show("Данные успешно обновлены!");

                _tmp_note = textBox3.Text;
                _tmp_start_datetime = start_datetime;
                _tmp_end_datetime = end_datetime;

                button1.Enabled = false;

                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        private bool CheckUpdate()
        {            
            return textBox3.Text != _tmp_note
                || dateTimePicker2.Value.ToString("dd.MM.yyyy HH:mm") != _tmp_end_datetime
                || dateTimePicker1.Value.ToString("dd.MM.yyyy HH:mm") != _tmp_start_datetime;
        }

        private void ShowNote_Shown(object sender, EventArgs e)
        {
            loadFromDataBase();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = CheckUpdate();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker2_ValueChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = CheckUpdate();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = CheckUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateItemInBase();
        }
    }
}
