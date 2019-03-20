using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontakteApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void personen2BindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.personen2BindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.kontakteDBDataSet);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.personenTableAdapter.Fill(this.kontakteDBDataSet.Personen);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
