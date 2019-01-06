using System;
using System.Collections.Generic;
using System.Linq;
using APIProdutos.Data;
using APIProdutos.Models;

namespace APIProdutos.Business
{
    public class ProdutoService
    {
        private CatalogoDbContext _context;

        public ProdutoService(CatalogoDbContext context)
        {
            _context = context;
        }

        public Produto Obter(string codigoBarras)
        {
            codigoBarras = codigoBarras?.Trim().ToUpper();
            if (!String.IsNullOrWhiteSpace(codigoBarras))
            {
                return _context.Produtos.Where(
                    p => p.CodigoBarras == codigoBarras).FirstOrDefault();
            }
            else
                return null;
        }

        public IEnumerable<Produto> ListarTodos()
        {
            return _context.Produtos
                .OrderBy(p => p.Nome).ToList();
        }

        public Resultado Incluir(Produto dadosProduto)
        {
            Resultado resultado = DadosValidos(dadosProduto);
            resultado.Acao = "Inclusão de Produto";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Produtos.Where(
                p => p.CodigoBarras == dadosProduto.CodigoBarras).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "Código de Barras já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                _context.Produtos.Add(dadosProduto);
                _context.SaveChanges();
            }

            return resultado;
        }

        public Resultado Atualizar(Produto dadosProduto)
        {
            Resultado resultado = DadosValidos(dadosProduto);
            resultado.Acao = "Atualização de Produto";

            if (resultado.Inconsistencias.Count == 0)
            {
                Produto produto = _context.Produtos.Where(
                    p => p.CodigoBarras == dadosProduto.CodigoBarras).FirstOrDefault();

                if (produto == null)
                {
                    resultado.Inconsistencias.Add(
                        "Produto não encontrado");
                }
                else
                {
                    produto.Nome = dadosProduto.Nome;
                    produto.Preco = dadosProduto.Preco;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(string codigoBarras)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Produto";

            Produto produto = Obter(codigoBarras);
            if (produto == null)
            {
                resultado.Inconsistencias.Add(
                    "Produto não encontrado");
            }
            else
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(Produto produto)
        {
            var resultado = new Resultado();
            if (produto == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Produto");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(produto.CodigoBarras))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Código de Barras");
                }
                if (String.IsNullOrWhiteSpace(produto.Nome))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Nome do Produto");
                }
                if (produto.Preco <= 0)
                {
                    resultado.Inconsistencias.Add(
                        "O Preço do Produto deve ser maior do que zero");
                }
            }

            return resultado;
        }
    }
}