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

namespace KontakteApp
{
    public partial class Form1 : Form
    {
        private string _connectionString = "";
        private string _caption = "";
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Config config = Config.GetInstance("Config.xml");

            _connectionString = config.ConnectionString;
            _caption = config.Caption;

            FormConnect formConnect = new FormConnect();
            formConnect.Close();


            SqlConnection sqlConnection = null;

            DataTable table = new DataTable("Kontakte");



            try
            {

                sqlConnection = new SqlConnection(_connectionString);
                sqlConnection.Open();

            }
            catch
            {
                MessageBox.Show("Die Datenbank 'Kontakte' wurde nicht gefunden!\n" +
                    "Bitte die Datei 'KontakteApp.exe.config.deploy' anpassen.\n" +
                    "Das Programm wird jetzt beendet.");
                this.Close();
            }


            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select * from dbo.Personen", sqlConnection);

            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);

            dataSet = new DataSet();
            dataSet.Tables.Add(table);


            this.personenTableAdapter.Fill(this.kontakteDBDataSet.Personen);


        }

        private void personenBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.personenBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.kontakteDBDataSet);
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            personenBindingSource.MovePrevious();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            personenBindingSource.MoveNext();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showGridFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form2 = new Form2();
            form2.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.Owner != null)
                this.Owner.Dispose();
        }
    }
}
