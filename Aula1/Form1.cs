using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aula1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Classe que trabalha com pixels
        Bitmap bmp;
        private void btnEscolher_Click(object sender, EventArgs e)
        {
            if (ofdArquivo.ShowDialog() == DialogResult.OK)
            {
                string nome = ofdArquivo.FileName;
                bmp = new Bitmap(nome);
                pbImagem.Image = bmp;
            }
        }

        SqlConnection conexao = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=bd_cliente;Data Source=PC-VIH\\SQLEXPRESS");

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Cliente cli = new Cliente();
            cli.Celular = mtbCelular.Text;
            cli.Cpf = mtbCpf.Text;
            cli.Endereco = txtEndereco.Text;
            cli.Nome = txtNome.Text;

            //Classe que guarda na memória
            MemoryStream memory = new MemoryStream();
            bmp.Save(memory, ImageFormat.Bmp);
            byte[] foto = memory.ToArray();

            SqlCommand sql = new SqlCommand("Insert into cad_cliente values(@nome, @endereco, @celular, @cpf, @imagem);",conexao);
            sql.Parameters.Add("@nome", SqlDbType.VarChar).Value = cli.Nome;
            sql.Parameters.Add("@endereco", SqlDbType.VarChar).Value = cli.Endereco;
            sql.Parameters.Add("@celular", SqlDbType.VarChar).Value = cli.Celular;
            sql.Parameters.Add("@cpf", SqlDbType.VarChar).Value = cli.Cpf;
            sql.Parameters.Add("@imagem", SqlDbType.VarBinary).Value = foto;
            try
            {
                conexao.Open();
                sql.ExecuteNonQuery();
                MessageBox.Show("Cadastrado com sucesso!");
            } catch(Exception ex)
            {
                MessageBox.Show("Não foi possível cadastrar!" + ex);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            Cliente c = new Cliente();
            c.Id = int.Parse(txtId.Text);
            SqlCommand sql = new SqlCommand("select * from cad_cliente where id=@id", conexao);
            sql.Parameters.Add("@id", SqlDbType.Int).Value = c.Id;
            try
            {
                conexao.Open();
                SqlDataReader pesquisar = sql.ExecuteReader();
                if (pesquisar.HasRows)
                {
                    pesquisar.Read();
                    string picture = pesquisar["imagem"].ToString();
                    
                    if(picture == "")
                    {
                        pbImagem.Image = null;
                    }
                    else
                    {
                        byte[] imagem = (byte[])pesquisar["imagem"];
                        MemoryStream memory = new MemoryStream(imagem);
                        pbImagem.Image = Image.FromStream(memory);
                    }
                    txtEndereco.Text = pesquisar["endereço"].ToString();
                    txtNome.Text = pesquisar["nome"].ToString();
                    mtbCelular.Text = pesquisar["celular"].ToString();
                    mtbCpf.Text = pesquisar["cpf"].ToString();

                }
                else
                {
                    MessageBox.Show("Cliente não encontrado!");
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex);
            }
            finally
            {
                conexao.Close();
            }

        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            MemoryStream memory = new MemoryStream();
            bmp.Save(memory, ImageFormat.Bmp);
            byte[] foto = memory.ToArray();

            Cliente cliente = new Cliente();
            cliente.Celular = mtbCelular.Text;
            cliente.Cpf = mtbCpf.Text;
            cliente.Id = int.Parse(txtId.Text);
            cliente.Nome = txtNome.Text;
            cliente.Endereco = txtEndereco.Text;

            SqlCommand sql = new SqlCommand("update cad_cliente set nome=@nome, endereço=@endereco, celular=@celular, cpf=@cpf, imagem=@imagem where id=@id", conexao);
            sql.Parameters.Add("@id", SqlDbType.Int).Value = cliente.Id;
            sql.Parameters.Add("@nome", SqlDbType.VarChar).Value = cliente.Nome;
            sql.Parameters.Add("@endereco", SqlDbType.VarChar).Value = cliente.Endereco;
            sql.Parameters.Add("@celular", SqlDbType.VarChar).Value = cliente.Celular;
            sql.Parameters.Add("@cpf", SqlDbType.VarChar).Value = cliente.Cpf;
            sql.Parameters.Add("@imagem", SqlDbType.VarBinary).Value = foto;

            try
            {
                conexao.Open();
                sql.ExecuteNonQuery();
                MessageBox.Show("Atualizado com sucesso!");
            } catch(Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            SqlCommand sql = new SqlCommand("delete from cad_cliente where id=@id", conexao);
            sql.Parameters.Add("@id", SqlDbType.Int).Value = txtId.Text;

            try
            {
                conexao.Open();
                sql.ExecuteNonQuery();
                MessageBox.Show("Excluído com sucesso");
                limpar();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex);
            }
            finally
            {
                conexao.Close();
            }
        }

        public void limpar()
        {
            txtEndereco.Text = null;
            txtNome.Text = null;
            mtbCelular.Text = null;
            mtbCpf.Text = null;
            pbImagem.Image = null;
            txtId.Text = null;
        }
    }
}
