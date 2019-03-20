using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KontakteApp
{
    public partial class FormConnect : Form
    {
        private string _sqlConnectionString = "";
        private string _configFile = "Config.xml";

        public FormConnect()
        {
            InitializeComponent();
        }

        private void FormConnect_Load(object sender, EventArgs e)
        {
            Config config = Config.GetInstance(_configFile);

            comboBoxServers.Items.Add(".");
            comboBoxServers.Items.Add(@"(localdb)\MSSQLLocalDB");
            comboBoxServers.Items.Add(@".\SQLEXPRESS");
            comboBoxServers.Items.Add(String.Format(@"{0}\SQLEXPRESS",Environment.MachineName));
            comboBoxServers.SelectedIndex = 1;
            checkBoxSecurity.Checked = true;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder();
            sqlConnectionString.InitialCatalog = textBoxDatabase.Text;
            sqlConnectionString.DataSource = comboBoxServers.Text;
            sqlConnectionString.IntegratedSecurity = checkBoxSecurity.Checked;

            try
            {
                SQLHelper helper = new SQLHelper(sqlConnectionString.ConnectionString);
                if (helper.IsConnected)
                {
                    MessageBox.Show("connection established", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _sqlConnectionString = sqlConnectionString.ConnectionString;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                SQLHelper helper = new SQLHelper(_sqlConnectionString);
                if (helper.IsConnected)
                {
                    string con;
                    AppSetting appSetting = new AppSetting();
                    con = appSetting.GetConnection("KontakteApp.Properties.Settings.KontakteDBConnectionString");
                    Debug.Print(con);

                    appSetting.SaveConnection("KontakteApp.Properties.Settings.KontakteDBConnectionString", _sqlConnectionString);
                    con = appSetting.GetConnection("KontakteApp.Properties.Settings.KontakteDBConnectionString");
                    Debug.Print(con);

                    Config config = Config.GetInstance(_configFile);
                    config.ConnectionString = con;
                    config.SaveConfig();

                    MessageBox.Show("the connection string was successfully saved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form1 form1 = new Form1();
                    form1.ShowDialog();

                }
                else
                { 
                    ArgumentException ArgEx = new ArgumentException("Emty connection string!","Exeption");
                    throw ArgEx;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxSecurity_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxSecurity_CheckedChanged(object sender, EventArgs e)
        {
            textBoxUsername.Enabled = !checkBoxSecurity.Checked;
            textBoxPassword.Enabled = !checkBoxSecurity.Checked;
        }
    }
}
