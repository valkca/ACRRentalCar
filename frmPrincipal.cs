using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ACRRentalCar
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        public void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmCadastroCliente();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
