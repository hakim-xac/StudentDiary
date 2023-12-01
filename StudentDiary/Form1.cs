using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentDiary
{
    public partial class Form1 : Form
    {
        private String _db_file_name;
        private SQLiteConnection _db_connect;
        private SQLiteCommand _sql_cmd;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _db_file_name = "StudentDiary.db";
            _db_connect = new SQLiteConnection();
            _sql_cmd = new SQLiteCommand();

            label1.Text = "Disconnected";
            label1.Visible = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                _db_connect = new SQLiteConnection("Data Source=" + _db_file_name + ";Version=3;");
                _db_connect.Open();
                _sql_cmd.Connection = _db_connect;

                _sql_cmd.CommandText = "SELECT * FROM ListNotes";
                _sql_cmd.ExecuteNonQuery();
                label1.Text = "Connected";

                DataTable data = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(_sql_cmd);
                adapter.Fill(data);
                if(data.Rows.Count > 0)
                {
                    label1.Visible = false;
                }
                listBox2.Items.Clear();
                listBox2.Items.Add($"Прочитано {data.Rows.Count} записей из таблицы БД");
                
                foreach (DataRow row in data.Rows)
                {
                    listBox2.Items.Add($"id = {row.Field<long>("id")} header = {row.Field<string>("Header")} Note = {row.Field<string>("Note")}");
                }

            }
            catch (SQLiteException ex)
            {
                label1.Text = "Disconnected";
                MessageBox.Show("Error: " + ex.Message);
            }



            AddingNote addingNote = new AddingNote();
            if (addingNote != null && addingNote.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("fghgfjfgjfg");
            }
        }
    }
}
