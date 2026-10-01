using eAgenda.Dominio.Modulos.Categorias;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Categorias;

public sealed class CategoriaController(IRepositorioCategoria repositorioCategoria) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<Categoria> categorias = repositorioCategoria.SelecionarTodos();

        List<ListarCategoraViewModel> listarVm = categorias.Select(c => new ListarCategoraViewModel(c.Id, c.Titulo))
        .ToList();

        return View();
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCategoriaViewModel cadastrarVm = new("");
        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCategoriaViewModel viewModel)
    {
        Categoria categoria = new Categoria(viewModel.Titulo);

        List<string> erros = categoria.Validar();

        if (repositorioCategoria.ExisteCategoriaPorTitulo(categoria.Titulo))
            erros.Add("Já existe um título cadastrado com esse nome.");

        if (erros.Count > 0)
        {
            ModelState.AddModelError(string.Empty, erros[0]);
            return View(viewModel);
        }

        repositorioCategoria.Cadastrar(categoria);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Editar(Guid id, EditarCategoriaViewModel viewModel)
    {
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Excluir(Guid id, EditarCategoriaViewModel viewModel)
    {
        return RedirectToAction(nameof(Listar));
    }
}
