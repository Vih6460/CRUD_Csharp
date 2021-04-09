using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula1
{
    class Cliente
    {
        private int id;
        private string nome;
        private string endereco;
        private string celular;
        private string cpf;

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Celular { get => celular; set => celular = value; }
        public string Cpf { get => cpf; set => cpf = value; }
    }
}
