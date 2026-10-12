using eAgenda.Aplicacao.Modulos.Categorias;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Categorias;

public sealed class CategoriaController(ServicoCategoria servicoCategoria) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<CategoriaDto> categorias = servicoCategoria.SelecionarTodos();

        return View(categorias);
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
        Result<Guid> resultado = servicoCategoria.Cadastrar(new CadastrarCategoriaDto(viewModel.Titulo));

        if (resultado.IsFailed)
        {
            string mensagemErro = resultado.Errors.Select(e => e.Message).First();

            ModelState.AddModelError(string.Empty, mensagemErro);

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<CategoriaDto> resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        CategoriaDto dto = resultado.Value;

        EditarCategoriaViewModel editarVm = new EditarCategoriaViewModel(
            dto.Id,
            dto.Titulo
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(Guid id, EditarCategoriaViewModel viewModel)
    {
        EditarCategoriaDto dto = new(
            viewModel.Id,
            viewModel.Titulo ?? string.Empty
        );

        Result resultado = servicoCategoria.Editar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                string campo = erro.Metadata["Campo"].ToString() ?? string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View(viewModel);
        }

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
