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
namespace Gerenciadordeacesso
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string strConn = @"DataSource = localhost; 
        Database =" + Application.StartupPath + @"\usuario.fdb; username = SYSDBA; password = masterkey";
        FbConnection conn;


        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = new FbConnection(strConn);

            try
            {

                FbCommand cmd = new FbCommand("SELECT COUNT(id) FROM tblogin WHERE usuario = '" + cbousuario.Text + "' AND senha ='" + txtsenha.Text + "'", conn);

                conn.Open();
                if (cbousuario.Text == "admin")
                {
                    int v = (int)cmd.ExecuteScalar();
                    if (v > 0)
                    {
                        MessageBox.Show("Logado com Sucesso", "Alerta",
               MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        this.Hide();
                        Form2 form2 = new Form2();
                        form2.Show();

                    }
                    else
                    {
                        lbmsnsenha.Text = "usuario e senha inválidos";

                    }
                }
                else
                {
                    int v = (int)cmd.ExecuteScalar();
                    if (v > 0)
                    {
                        MessageBox.Show("Logado com Sucesso");
                        System.Diagnostics.Process.Start(@"" + Application.StartupPath + @"\bin\Debug\Cadastrofuncionario.exe");
                        this.Close();
                    }
                    else
                    {
                        lbmsnsenha.Text = "usuario e senha inválidos";

                    }
                }

            }

            catch (FbException errro)
            {
                MessageBox.Show(errro + "Erro na conexão com banco de dados", "Alerta",
               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}