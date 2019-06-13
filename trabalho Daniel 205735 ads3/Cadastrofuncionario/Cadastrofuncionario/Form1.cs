using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace Cadastrofuncionario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string strConn = @"DataSource = localhost; 
        Database =" + Application.StartupPath + @"\bdcadastro.fdb; username = SYSDBA; password = masterkey";
        FbConnection conn;
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new FbConnection(strConn); //conecta utilizando a string    
            MessageBox.Show("Banco de Dados Conectado com Sucesso !", "Alerta",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnlistar_Click(object sender, EventArgs e)
        {
            FbCommand cmd = new FbCommand("SELECT * FROM funcionarios", conn); //executa o SQL
            FbDataAdapter DA = new FbDataAdapter(cmd); //cria uma variavel para coletar o resultado
            DataSet DS = new DataSet(); //cria uma variavel DATASET para distribuir o resultado
            conn.Open(); // abre conexao
            //jogar os resultados no grid.
            DA.Fill(DS, "funcionarios"); //Diz para o DATASET qual a tabela 
            dataGridView1.DataSource = DS; // Linca o GRID com o DATASET
            dataGridView1.DataMember = "funcionarios"; //especifica nome da base para grid
            btnlimpar_Click(btnlimpar, e);
            conn.Close();//fecha conecxao

            //observação codigo para fazer total de quantidade de usuarios cadastrados
            conn = new FbConnection(strConn);
            FbCommand cmd2 = new FbCommand("SELECT COUNT(codigo) FROM funcionarios", conn);
            conn.Open();
            int u = (int)cmd2.ExecuteScalar();
            statusStrip1.Items[2].Text = "Total de usuarios: " + u.ToString() + "";
            conn.Close();
        }
        private void btninserir_Click(object sender, EventArgs e)
        {
            if (txtid.Text == "" || txtnome.Text == "" || mskfone.Text == "" || mskadmissao.Text == "" || cbosalario.Text == "selecione..." || cbodep.Text == "selecione..." || cbocargo.Text == "selecione..." || mskcpf.Text == "" || txtend.Text == "" || txtsobrenome.Text == "" || txtcidade.Text == "" || txtemail.Text == "")
            {
                MessageBox.Show("Favor preencha os dados do cadastro", "Alerta",
                  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    string sqlIncluir = "INSERT INTO funcionarios(codigo,nome,sobrenome,nome_social,cpf,fone,endereco,cep,cidade,cargo,salario,departamento,data_admissao,email)" + "Values (" + txtid.Text + ",'" + txtnome.Text + "','" + txtsobrenome.Text + "','" + txtsocial.Text + "','" + mskcpf.Text + "','" + mskfone.Text + "','" + txtend.Text + "','" + mskcep.Text + "','" + txtcidade.Text + "','" + cbocargo.Text + "','" + cbosalario.Text + "','" + cbodep.Text + "','" + mskadmissao.Text + "','" + txtemail.Text + "')";
                    FbCommand cmd = new FbCommand(sqlIncluir, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    //fecha conexao
                    MessageBox.Show("Cadastro gravado com sucesso", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (FbException)
                {    // MessageBox.Show( ex.Message, "Alerta",
                    MessageBox.Show("Código único sendo ultilizado, tente outro número", "Alerta",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (InvalidOperationException)
                {
                    // MessageBox.Show(ex.Message, "Alerta");
                    MessageBox.Show("Perda de conexão, por favor renicie a aplicação");
                }
                conn.Close();
                btnlimpar_Click(btnlimpar, e);
                btnlistar_Click(btnlistar, e);
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
            {// if para garantir uma última vereficação antes de excluir o cadastro
                if (MessageBox.Show("Deseja Excluir", "Confirme", MessageBoxButtons.YesNo,
                      MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    try
                    {
                        string sqldeletar = "DELETE FROM funcionarios WHERE codigo=(" + txtid.Text + ")";
                        FbCommand cmd = new FbCommand(sqldeletar, conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Cadastro excluido com sucesso", "Alerta",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (FbException)
                    {
                        MessageBox.Show("Preenchimento inválido, verifique e tente novamente", "Alerta",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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
                    FbCommand cmd = new FbCommand("SELECT * FROM funcionarios WHERE codigo=" + txtid.Text, conn);
                    FbDataAdapter DA = new FbDataAdapter(cmd);
                    DataSet DS = new DataSet();
                    conn.Open();
                    DA.Fill(DS, "funcionarios");
                    conn.Close();
                    txtnome.Text = DS.Tables["funcionarios"].Rows[0][1].ToString();
                    txtsobrenome.Text = DS.Tables["funcionarios"].Rows[0][2].ToString();
                    txtsocial.Text = DS.Tables["funcionarios"].Rows[0][3].ToString();
                    mskcpf.Text = DS.Tables["funcionarios"].Rows[0][4].ToString();
                    mskfone.Text = DS.Tables["funcionarios"].Rows[0][5].ToString();
                    txtend.Text = DS.Tables["funcionarios"].Rows[0][6].ToString();
                    mskcep.Text = DS.Tables["funcionarios"].Rows[0][7].ToString();
                    txtcidade.Text = DS.Tables["funcionarios"].Rows[0][8].ToString();
                    cbocargo.Text = DS.Tables["funcionarios"].Rows[0][9].ToString();
                    cbosalario.Text = DS.Tables["funcionarios"].Rows[0][10].ToString();
                    cbodep.Text = DS.Tables["funcionarios"].Rows[0][11].ToString();
                    mskadmissao.Text = DS.Tables["funcionarios"].Rows[0][12].ToString();
                    txtemail.Text = DS.Tables["funcionarios"].Rows[0][13].ToString();
                }
            }

            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("código de usuário não cadastrado", "Alerta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnlimpar_Click(object sender, EventArgs e)
        {
            txtid.Clear();
            txtnome.Clear();
            txtsobrenome.Clear();
            txtsocial.Clear();
            mskcpf.Clear();
            mskfone.Clear();
            txtend.Clear();
            mskcep.Clear();
            txtcidade.Clear();
            cbocargo.Text = "Selecione";
            cbosalario.Text = "Selecione";
            cbodep.Text = "Selecione";
            mskadmissao.Clear();
            txtemail.Clear();
            txtid.Focus();
            toolStripComboBox1.Text = "";
            toolStripComboBox2.Text = "";
            toolStripComboBox3.Text = "";

        }

        private void btnalterar_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlalterar = "UPDATE funcionarios SET nome ='" + txtnome.Text + "', sobrenome ='" + txtsobrenome.Text + "', nome_social ='" + txtsocial.Text + "', cpf ='" + mskcpf.Text + "',fone='" + mskfone.Text + "', endereco ='" + txtend.Text + "', cep ='" + mskcep.Text + "', cidade ='" + txtcidade.Text + "', cargo ='" + cbocargo.Text + "', salario ='" + cbosalario.Text + "', departamento ='" + cbodep.Text + "', data_admissao ='" + mskadmissao.Text + "', email ='" + txtemail.Text + "' WHERE codigo=(" + txtid.Text + ")";
                FbCommand cmd = new FbCommand(sqlalterar, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cadastro alterado com sucesso", "Alerta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (FbException)
            {
                //MessageBox.Show(ex.Message, "Alerta");
                MessageBox.Show("Pesquise o código cadastrado" +
                  " , e altere o campo desejado e clique alterar", "Alerta",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            conn.Close(); //fecha conexao
            btnlimpar_Click(btnlimpar, e);
            btnlistar_Click(btnlistar, e);

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

            /*
             *observação codigo para fazer total de quantidade de usuarios cadastrados
             * esta´ visivel quando listar todos os usuarios ,codigo ativo no private void btnlistar_Click
             * 
             * conn = new FbConnection(strConn);
            FbCommand cmd = new FbCommand("SELECT COUNT(codigo) FROM funcionarios", conn);
            conn.Open();
            int u = (int)cmd.ExecuteScalar();

            statusStrip1.Items[2].Text = "Total de usuarios: " + u.ToString() + "";
            conn.Close();*/
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                FbCommand cmd1 = new FbCommand("SELECT " + toolStripComboBox1.Text + ", " + toolStripComboBox2.Text + ", " + toolStripComboBox3.Text + " FROM funcionarios;", conn); //executa o SQL ordenar item por coluna

                FbDataAdapter DA1 = new FbDataAdapter(cmd1); //cria uma variavel para coletar o resultado
                DataSet DS1 = new DataSet(); //cria uma variavel DATASET para distribuir o resultado
                conn.Open(); // abre conexao
                             //jogar os resultados no grid.
                DA1.Fill(DS1, "funcionarios"); //Diz para o DATASET qual a tabela 
                dataGridView1.DataSource = DS1; // Linca o GRID com o DATASET
                dataGridView1.DataMember = "funcionarios"; //especifica nome da base para grid
                btnlimpar_Click(btnlimpar, e);
                conn.Close(); //fecha conecxao


            }
            catch (FbException)
            {
                MessageBox.Show("Selecione todos os campos,para visualização por colunas");
            }
            conn.Close();//fecha conecxao

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {// if else para limpar dataGridVieW1 quando fazer seleção opcional por colunas 

            if (this.dataGridView1.DataSource != null)
            {
                this.dataGridView1.DataSource = null;
            }
            else
            {
                this.dataGridView1.Rows.Clear();

            }
        }


    }
}
