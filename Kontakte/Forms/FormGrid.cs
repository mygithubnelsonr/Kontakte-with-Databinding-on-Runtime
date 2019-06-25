using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KontakteApp
{
    public partial class FormGrid : Form
    {
        private string _connectionString;
        private string _caption;
        private SqlDataAdapter dataAdapter;

        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string Caption { get => _caption; set => _caption = value; }

        public FormGrid()
        {
            InitializeComponent();
        }

        private void FormGrid_Load(object sender, EventArgs e)
        {
            Config config = Config.GetInstance("Config.xml");

            ConnectionString = config.ConnectionString;
            Caption = config.Caption;

            try
            {
                SQLHelper helper = new SQLHelper(ConnectionString);
                bool connectionOK = helper.IsConnected;
            }
            catch
            {
                MessageBox.Show("Die Datenbank 'Kontakte' wurde nicht gefunden!\n" +
                    "Bitte die Datei 'KontakteApp.exe.config.deploy' anpassen.\n" +
                    "Das Programm wird jetzt beendet.");
                this.Close();
            }

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("select * from dbo.Personen", sqlConnection);
            dataAdapter = new SqlDataAdapter(sqlCommand);

            kontakteDataSet = new DataSet("Kontakte");
            kontakteDataSet.Tables.Add("Personen");
            dataAdapter.Fill(kontakteDataSet.Tables["Personen"]);

            personenBindingNavigator.BindingSource = personenBindingSource;
            personenBindingSource.DataMember = "Personen";
            BindingControls(kontakteDataSet.Tables["Personen"]);

            personenDataGridView.DataSource = personenBindingSource;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void personenBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.personenBindingSource.EndEdit();

            SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.UpdateCommand = builder.GetUpdateCommand();
            dataAdapter.Update(kontakteDataSet, "Kontakte");
        }

        private void BindingControls(DataTable dt)
        {
            personenBindingSource.DataSource = dt;
            kontakt_IDTextBox.DataBindings.Add(new Binding("Text", personenBindingSource, "Kontakt_ID", true));
            vornameTextBox.DataBindings.Add(new Binding("Text", personenBindingSource, "Vorname", true));
            nachnameTextBox.DataBindings.Add(new Binding("Text", personenBindingSource, "Nachname", true));
            firmaTextBox.DataBindings.Add(new Binding("Text", personenBindingSource, "Firma", true));
            telefonTextBox.DataBindings.Add(new Binding("Text", personenBindingSource, "Telefon", true));
            emailTextBox.DataBindings.Add(new Binding("Text", personenBindingSource, "Email", true));
            anrufDateTimePicker.DataBindings.Add(new Binding("Value", personenBindingSource, "Anruf", true));
        }

    }
}
