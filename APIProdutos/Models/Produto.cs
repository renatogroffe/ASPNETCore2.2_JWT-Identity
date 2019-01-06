namespace APIProdutos.Models
{
    public class Produto
    {
        private string _codigoBarras;
        public string CodigoBarras
        {
            get => _codigoBarras;
            set => _codigoBarras = value?.Trim().ToUpper();
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set => _nome = value?.Trim();
        }

        public double Preco { get; set; }
    }
}