using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIProdutos.Business;
using APIProdutos.Models;

namespace APIProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProdutosController : ControllerBase
    {
        private ProdutoService _service;

        public ProdutosController(ProdutoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Produto> Get()
        {
            return _service.ListarTodos();
        }

        [HttpGet("{codigoBarras}")]
        public ActionResult<Produto> Get(string codigoBarras)
        {
            var produto = _service.Obter(codigoBarras);
            if (produto != null)
                return produto;
            else
                return NotFound();
        }

        [HttpPost]
        public Resultado Post([FromBody]Produto produto)
        {
            return _service.Incluir(produto);
        }

        [HttpPut]
        public Resultado Put([FromBody]Produto produto)
        {
            return _service.Atualizar(produto);
        }

        [HttpDelete("{codigoBarras}")]
        public Resultado Delete(string codigoBarras)
        {
            return _service.Excluir(codigoBarras);
        }
    }
}