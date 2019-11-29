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

namespace ACRRentalCar
{
    public partial class frmConsultaCliente : Form
    {
        frmCadastroCliente frmCliente;

        public frmConsultaCliente(frmCadastroCliente frmCliente)
        {
            this.frmCliente = frmCliente;
            InitializeComponent();
        }

        public void frmConsultaCliente_Load(object sender, EventArgs e)
        {
            string sqlQuery;
            SqlConnection conCliente = Conexao.GetConnection();

            sqlQuery = "SELECT id_cliente, nome, cpf, data_nasc FROM cliente ORDER BY nome";

            SqlDataAdapter dta = new SqlDataAdapter(sqlQuery, conCliente);
            DataTable dt = new DataTable();

            try
            {
                dta.Fill(dt);
                dgvCliente.DataSource = dt;

                dgvCliente.RowsDefaultCellStyle.BackColor = Color.White;
                dgvCliente.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;

                dgvCliente.Columns[0].HeaderCell.Value = "Código do Cliente";
                dgvCliente.Columns[1].HeaderCell.Value = "Nome"; 
                dgvCliente.Columns[2].HeaderCell.Value = "CPF"; 
                dgvCliente.Columns[3].HeaderCell.Value = "Dt. Nasc."; 

            }
            catch(Exception ex)
            {
                MessageBox.Show("Problema ao listar clientes " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }
        }

        public void btnSelecionar_Click(object sender, EventArgs e)
        {
            string codigoCliente;

            codigoCliente = dgvCliente.CurrentRow.Cells[0].Value.ToString();

            string sqlQuery;

            SqlConnection conClienteConsulta = Conexao.GetConnection();

            SqlDataReader dtr = null;

            sqlQuery = "SELECT id_cliente, nome, cpf, data_nasc FROM cliente WHERE id_cliente = @id_cliente";

            try
            {
                conClienteConsulta.Open();

                SqlCommand cmd = new SqlCommand(sqlQuery, conClienteConsulta);

                cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(codigoCliente)));

                dtr = cmd.ExecuteReader();

                if (dtr.Read())
                {
                    frmCliente.txtCodigo.Text = dtr["ID_CLIENTE"].ToString();
                    frmCliente.txtNome.Text = dtr["NOME"].ToString();
                    frmCliente.mskDtNasc.Text = dtr["DATA_NASC"].ToString();
                    frmCliente.mskCPF.Text = dtr["CPF"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (dtr != null)
                {
                    dtr.Close();
                }
                if (conClienteConsulta != null)
                {
                    conClienteConsulta.Close();
                }
            }

            this.Close();
        }

        public void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
