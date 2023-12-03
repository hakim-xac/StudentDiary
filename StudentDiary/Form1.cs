using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentDiary
{
    public partial class Form1 : Form
    {
        private String _db_file_name;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _db_file_name = "StudentDiary.db";

            if (loadFromDataBase())
                ActiveControl = dataGridView1;
            
            
        }

        private bool loadFromDataBase()
        {
            if (!File.Exists(_db_file_name))
            {
                SQLiteConnection.CreateFile(_db_file_name);

                SQLiteConnection _db_connect = new SQLiteConnection($"Data Source={_db_file_name};Version=3;");
                _db_connect.Open();
                SQLiteCommand _sql_cmd = new SQLiteCommand("" +
                    "CREATE TABLE \"ListNotes\" (\r\n\t\"id\"" +
                    "\tINTEGER NOT NULL UNIQUE,\r\n" +
                    "\t\"header\"\tTEXT NOT NULL,\r\n" +
                    "\t\"note\"\tTEXT,\r\n" +
                    "\t\"start_datetime\"\tTEXT NOT NULL,\r\n" +
                    "\t\"end_datetime\"\tTEXT NOT NULL,\r\n" +
                    "\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n" +
                    ")");
                
                _sql_cmd.Connection = _db_connect;
                _sql_cmd.ExecuteNonQuery();
                _db_connect.Close();
            }

            try
            {

                SQLiteConnection _db_connect = new SQLiteConnection($"Data Source={_db_file_name};Version=3;");
                _db_connect.Open();
                SQLiteCommand _sql_cmd = new SQLiteCommand("SELECT id, header, start_datetime, end_datetime FROM ListNotes");
                
                _sql_cmd.Connection = _db_connect;
                _sql_cmd.ExecuteNonQuery();

                _db_connect.Close();

                DataTable data = new DataTable();  
                
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(_sql_cmd);

                adapter.Fill(data);
                
                if (data.Rows.Count == 0)
                {
                    label1.Visible = true;
                    dataGridView1.Visible = false;
                    button4.Enabled = false;
                    return false;
                }

                label1.Visible = false;
                dataGridView1.Visible = true;

                dataGridView1.DataSource = data;
                button4.Enabled = true;

                if(dataGridView1.Columns.Count == 4)
                {
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[2].Width = 140;
                    dataGridView1.Columns[3].Width = 140;
                }

                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddingNote addingNote = new AddingNote();
            if (addingNote != null && addingNote.ShowDialog() == DialogResult.OK)
            {
                if (loadFromDataBase())
                {
                    dataGridView1.Update();
                    ActiveControl = dataGridView1;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool is_access_delete = true;


            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String id = row.Cells[0].Value.ToString();
                if (DeleteFromDatabase(id) == false)
                    is_access_delete = false;                
            }

            if(is_access_delete == false)
                    MessageBox.Show("Ошибка удаления данных из базы!");
            dataGridView1.Update();

            if (loadFromDataBase())
                ActiveControl = dataGridView1;
        }

        private bool DeleteFromDatabase(String id)
        {
            try
            {
                SQLiteConnection _db_connect = new SQLiteConnection($"Data Source={_db_file_name};Version=3;");
                _db_connect.Open();
                SQLiteCommand _sql_cmd = new SQLiteCommand($"DELETE FROM ListNotes where id = {id}");

                _sql_cmd.Connection = _db_connect;
                int access_count = _sql_cmd.ExecuteNonQuery();

                return access_count > 0;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            ShowNote show_note = new ShowNote();


            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбран элемент для удаления!");
                return;
            }

            show_note.id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            if (show_note != null && show_note.ShowDialog() == DialogResult.OK)
            {
            }
        }
    }
}
