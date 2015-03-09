using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;


namespace SQLiteDemo
{
    public partial class Form1 : Form
    {
        SQLiteDatabase db;
        SQLiteConnection m_dbConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(!File.Exists("bookmarks.s3db")){
            SQLiteConnection.CreateFile("bookmarks.s3db");
            CreateDBStructure();
            }           
            LoadData();
        }

        private void CreateDBStructure() {

           
            m_dbConnection =
            new SQLiteConnection("Data Source=bookmarks.s3db;Version=3;");
            m_dbConnection.Open();

            string sql = "create table bookmarks (id INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT, name varchar(200),url varchar(2000), time TIMESTAMP DEFAULT CURRENT_TIMESTAMP NULL )";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }
        private void LoadData() {

            try
            {
                db = new SQLiteDatabase();
                DataTable bookmarks;
                String query = "select id \"id\",name \"name\", url \"url\",";
                query += "time \"time\"";
                query += "from bookmarks;";
                bookmarks = db.GetDataTable(query);
                // The results can be directly applied to a DataGridView control
                dgBookMarks.DataSource = bookmarks;
                dgDeltebookmarks.DataSource = bookmarks;
                /*
                // Or looped through for some other reason
                foreach (DataRow r in recipe.Rows)
                {
                    MessageBox.Show(r["Name"].ToString());
                    MessageBox.Show(r["Description"].ToString());
                    MessageBox.Show(r["Prep Time"].ToString());
                    MessageBox.Show(r["Cooking Time"].ToString());
                }
	
                */
            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                this.Close();
            }


        }

        private void btnadd_Click(object sender, EventArgs e)
        {

            db = new SQLiteDatabase();
            Dictionary<String, String> data = new Dictionary<String, String>();
            data.Add("name", txtname.Text);
            data.Add("url", txturl.Text);
            
            try
            {
                db.Insert("bookmarks", data);
            }
            catch (Exception crap)
            {
                MessageBox.Show(crap.Message);
            }
            LoadData();
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgDeltebookmarks.SelectedRows.Count>0)
            {
            int id = Convert.ToInt32(dgDeltebookmarks.SelectedRows[0].Cells[0].Value);

                db = new SQLiteDatabase();

                db.Delete("bookmarks", String.Format("id = {0}", id));        

            LoadData();
            }
        }

        private void dgBookMarks_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgBookMarks.SelectedRows.Count > 0)
            {
                string url = (dgBookMarks.SelectedRows[0].Cells[2].Value).ToString();
                Process.Start(url);
            }
        }
    }
}
