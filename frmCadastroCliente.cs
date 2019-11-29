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
    public partial class frmCadastroCliente : Form
    {
        public frmCadastroCliente()
        {
            InitializeComponent();
        }

        public void habilitar()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = true;
            mskCPF.Enabled = true;
            mskDtNasc.Enabled = true;
        }


        public void desabilitar()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            mskCPF.Enabled = false;
            mskDtNasc.Enabled = false;
        }

        public void limparControles()
        {
            txtCodigo.Enabled = false;
            txtNome.Clear();
            txtCodigo.Clear();
            mskCPF.Clear();
            mskDtNasc.Clear();
            mskCPF.Focus();
        }

        public bool validaDados()
        {
            if (string.IsNullOrEmpty(mskCPF.Text))
            {
                MessageBox.Show("Preenchimento obrigatório", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                mskCPF.Clear();
                mskCPF.Focus();

                return false;
            }

            DateTime auxDate;
            if (!(DateTime.TryParse(mskDtNasc.Text, out auxDate)))
            {
                MessageBox.Show("Preenchimento obrigatório", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                mskDtNasc.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Preenchimento obrigatório", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNome.Clear();
                txtNome.Focus();

                return false;
            }

            return true;
        }

        public void frmCadastroCliente_Load(object sender, EventArgs e)
        {
            habilitar();
        }

        public void btnIncluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (MessageBox.Show("Você está editando um registro existente. Deseja incluir um registro novo?", "ACR Rental Car", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    limparControles();

                return;
            }

            if (validaDados() == false)
            {
                return;
            }

            string sqlQuery;
            SqlConnection conCliente = Conexao.GetConnection();

            sqlQuery = "INSERT INTO cliente(nome, data_nasc, cpf) VALUES(@nome, @data_nasc, @cpf)";

            try
            {
                conCliente.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);

                cmd.Parameters.Add(new SqlParameter("@nome", txtNome.Text));
                cmd.Parameters.Add(new SqlParameter("@data_nasc", Convert.ToDateTime(mskDtNasc.Text)));
                cmd.Parameters.Add(new SqlParameter("@cpf", mskCPF.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente incluído com sucesso!", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                limparControles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao incluir cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }
        }

        public void btnAlterar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Consulte o cliente que deseja alterar clicando no botão consultar", "ACR Rentar Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (validaDados() == false)
            {
                return;
            }

            string sqlQuery;
            SqlConnection conCliente = Conexao.GetConnection();

            sqlQuery = "UPDATE cliente set nome = @nome, data_nasc = @data_nasc, cpf = @cpf WHERE id_cliente = @id_cliente";

            try
            {
                conCliente.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);

                cmd.Parameters.Add(new SqlParameter("@nome", txtNome.Text));
                cmd.Parameters.Add(new SqlParameter("@data_nasc", Convert.ToDateTime(mskDtNasc.Text)));
                cmd.Parameters.Add(new SqlParameter("@cpf", mskCPF.Text));
                cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(txtCodigo.Text)));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente alterado com sucesso!", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                limparControles();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Problema ao alterar cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }
        }

        public void btnExcluir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Consulte o cliente que deseja excluir clicando no botão consultar", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja excluir permanentemente o registro?", "ACR Rental", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sqlQuery;
                SqlConnection conCliente = Conexao.GetConnection();

                sqlQuery = "DELETE FROM cliente WHERE id_cliente = @id_cliente";

                try
                {
                    conCliente.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);

                    cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(txtCodigo.Text)));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cliente excluído com sucesso!", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    limparControles();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Problema ao excluir cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    if (conCliente != null)
                    {
                        conCliente.Close();
                    }
                }
            }
        }

        public void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void btnConsultar_Click(object sender, EventArgs e)
        {
            Form frm = new frmConsultaCliente(this);
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
    }
}
