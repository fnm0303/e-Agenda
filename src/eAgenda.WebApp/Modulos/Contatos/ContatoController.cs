using eAgenda.Aplicacao.Modulos.Contatos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Contatos;

public sealed class ContatoController(ServicoContato servicoContato) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ContatoDto> dtos = servicoContato.SelecionarTodas();

        return View(dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarContatoViewModel viewModel = new(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarContatoViewModel viewModel)
    {
        CadastrarContatoDto dto = new(
            viewModel.Nome,
            viewModel.Email,
            viewModel.Telefone,
            viewModel.Cargo ?? string.Empty,
            viewModel.Empresa ?? string.Empty
        );

        Result<Guid> resultado = servicoContato.Cadastrar(dto);
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
    public ActionResult Editar(Guid id)
    {
        Result<ContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        ContatoDto dto = resultado.Value;

        EditarContatoViewModel viewModel = new(
            id,
            dto.Nome,
            dto.Email,
            dto.Telefone,
            dto.Cargo,
            dto.Empresa
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarContatoViewModel viewModel)
    {
        EditarContatoDto dto = new(
            viewModel.Id,
            viewModel.Nome,
            viewModel.Email,
            viewModel.Telefone,
            viewModel.Cargo ?? string.Empty,
            viewModel.Empresa ?? string.Empty
        );

        Result resultado = servicoContato.Editar(dto);

        if (resultado.IsFailed)
        {
            if (!resultado.Errors.Any(e => e.HasMetadataKey("Campo")))
            {
                TempData["MensagemErro"] = resultado.Errors.First().Message;

                return RedirectToAction(nameof(Listar));
            }

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
    public ActionResult Excluir(Guid id)
    {
        Result<ContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        ContatoDto dto = resultado.Value;

        ExcluirContatoViewModel viewModel = new(
            dto.Id,
            dto.Nome,
            dto.Email,
            dto.Telefone,
            dto.Cargo,
            dto.Empresa
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirContatoViewModel excluirVm)
    {
        Result resultado = servicoContato.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(Listar));
    }
}
