using System;
using System.Data;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace Gerenciadordeacesso
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private string strConn = @"DataSource=localhost;Database=
" + Application.StartupPath + @"\usuario.fdb; 
username= SYSDBA; password = masterkey";
        FbConnection conn;


        private void Form2_Load(object sender, EventArgs e)
        {
            txtusuario.Enabled = false;
            txtsenha.Enabled = false;

            conn = new FbConnection(strConn); //conecta utilizando a string    
            MessageBox.Show("Banco de Dados Conectado com Sucesso !", "Alerta",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnlistar_Click(object sender, EventArgs e)
        {
            FbCommand cmd = new FbCommand("SELECT id, usuario, '********' as senha FROM tblogin", conn); //executa o SQL
            FbDataAdapter DA = new FbDataAdapter(cmd); //cria uma variavel para coletar o resultado
            DataSet DS = new DataSet(); //cria uma variavel DATASET para distribuir o resultado
            conn.Open(); // abre conexao
            //jogar os resultados no grid.
            DA.Fill(DS, "tblogin"); //Diz para o DATASET qual a tabela 
            dataGridView1.DataSource = DS; // Linca o GRID com o DATASET
            dataGridView1.DataMember = "tblogin"; //especifica nome da base para grid
            btnlimpar_Click(btnlimpar, e);
            conn.Close(); //fecha conecxao
        }

        private void btnlimpar_Click(object sender, EventArgs e)
        {
            txtid.Clear();
            txtusuario.Clear();
            txtsenha.Clear();
            txtnovoid.Clear();
            txtnovousuario.Clear();
            txtnovasenha.Clear();
        }

        private void btninserir_Click_1(object sender, EventArgs e)
        {

            try
            {
                if (txtid.Text == "" || txtusuario.Text == "" || txtsenha.Text == "")
                {
                    MessageBox.Show("Favor preencha todos dados do cadastro", "Alerta",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sqlIncluir = "INSERT INTO tblogin(id,usuario,senha)" + "Values (" + txtnovoid.Text + ",'" + txtnovousuario.Text + "','" + txtnovasenha.Text + "')";
                    FbCommand cmd = new FbCommand(sqlIncluir, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close(); //fecha conexao
                    MessageBox.Show("Cadastrado com sucesso", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnlimpar_Click(btnlimpar, e);
                    btnlistar_Click(btnlistar, e);
                }
            }
            catch (FbException)

            {
                MessageBox.Show("Código único sendo ultilizado, tente outro número", "Alerta",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnuptade_Click(object sender, EventArgs e)
        {
            if (txtid.Text == "" || txtsenha.Text == "" || txtusuario.Text == "")
            {
                MessageBox.Show("Insira um código ,preencha os dados de troca  ", "Alerta",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Deseja realizar a alteração", "Confirme", MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string sqlalterar = "UPDATE tblogin SET usuario='" + txtusuario.Text + "',senha='" + txtsenha.Text + "' WHERE id=(" + txtid.Text + ")";
                    FbCommand cmd = new FbCommand(sqlalterar, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close(); //fecha conexao
                    MessageBox.Show("Cadastro alterado com sucesso", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnlimpar_Click(btnlimpar, e);
                    btnlistar_Click(btnlistar, e);
                }
            }


        }

        private void btnexcluir_Click(object sender, EventArgs e)
        {
            if (txtid.Text == "")
            {
                MessageBox.Show("Insira um código cadastrado para exclusão", "Alerta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Deseja Excluir", "Confirme", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sqldeletar = "DELETE FROM tblogin WHERE id=(" + txtid.Text + ")";
                    FbCommand cmd = new FbCommand(sqldeletar, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close(); //fecha conexao
                    MessageBox.Show("Cadastro excluido com sucesso", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnlimpar_Click(btnlimpar, e);
                    btnlistar_Click(btnlistar, e);
                }
            }

        }

        private void btnpesquisar_Click(object sender, EventArgs e)
        {
            try 
            {
                if (txtid.Text == "")
                {
                    MessageBox.Show("Favor digitar o código para realizar a pesquisa", "Alerta",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                  

                    FbCommand cmd = new FbCommand("SELECT * FROM tblogin WHERE id=" + txtid.Text, conn);
                    FbDataAdapter DA = new FbDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    conn.Open();
                    DA.Fill(DS, "tblogin");
                    conn.Close();
                    txtid.Text = DS.Tables["tblogin"].Rows[0][0].ToString();
                    txtusuario.Text = DS.Tables["tblogin"].Rows[0][1].ToString();
                    txtsenha.Text = DS.Tables["tblogin"].Rows[0][2].ToString();
                    txtusuario.Enabled = true;
                    txtsenha.Enabled = true;

                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("código de usuário não cadastrado", "Alerta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtusuario.Enabled = false;
                txtsenha.Enabled = false;
            }
        }

        private void btnfechar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Finalizar", "Confirme", MessageBoxButtons.YesNo,
              MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            statusStrip1.Items[0].Text = "Data: " + DateTime.Now.ToString("dd/MMMM/yyyy");
            statusStrip1.Items[1].Text = "Hora: " + DateTime.Now.ToString("HH:mm:ss");
            conn = new FbConnection(strConn);
            FbCommand cmd = new FbCommand("SELECT COUNT(id)  FROM tblogin", conn);
            conn.Open();
            int u = (int)cmd.ExecuteScalar();

            statusStrip1.Items[2].Text = "Total de usuarios: " + u.ToString() + "";
            conn.Close();
        }

        private void txtid_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8
             || e.KeyChar == 46 ? false : true;
        }

        private void txtsenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8
             || e.KeyChar == 46 ? false : true;
        }

        private void txtnovoid_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8
             || e.KeyChar == 46 ? false : true;
        }

        private void txtnovasenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8
             || e.KeyChar == 46 ? false : true;
        }
    }
}
