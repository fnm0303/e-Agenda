using eAgenda.Aplicacao.Modulos.Compromissos;
using eAgenda.Aplicacao.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Contatos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Compromissos;

public sealed class CompromissoController(ServicoCompromisso servicoCompromisso, ServicoContato servicoContato) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<CompromissoDto> dtos = servicoCompromisso.SelecionarTodas();

        return View(dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        var contatosDisponiveis = servicoContato.SelecionarTodas();
        ViewBag.Contatos = contatosDisponiveis;

        CadastrarCompromissoViewModel viewModel = new(
            string.Empty,
            DateOnly.FromDateTime(DateTime.Today),
            TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
            default,
            string.Empty,
            []
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCompromissoViewModel viewModel)
    {
        List<Contato> contatosCarregados = servicoContato
                .SelecionarTodas()
                .Where(c => viewModel.ContatosSelecionados.Contains(c.Id))
                .Select(c => new Contato(c.Nome, c.Email, c.Telefone) { Id = c.Id })
                .ToList();

        CadastrarCompromissoDto dto = new(
            viewModel.Assunto,
            viewModel.DataOcorrencia,
            viewModel.HoraInicio,
            viewModel.HoraTermino,
            viewModel.TipoCompromisso,
            viewModel.LinkOuLocal,
            viewModel.ContatosSelecionados ?? []
        );

        Result<Guid> resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ViewBag.Contatos = servicoContato.SelecionarTodas();

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
        Result<CompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        CompromissoDto dto = resultado.Value;

        ViewBag.Contatos = servicoContato.SelecionarTodas();

        EditarCompromissoViewModel viewModel = new(
            id,
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.TipoCompromisso,
            dto.LinkOuLocal,
            []
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarCompromissoViewModel viewModel)
    {
        var idsContatos = viewModel.ContatosSelecionados ?? [];

        EditarCompromissoDto dto = new(
            viewModel.Id,
            viewModel.Assunto,
            viewModel.DataOcorrencia,
            viewModel.HoraInicio,
            viewModel.HoraTermino,
            viewModel.TipoCompromisso,
            viewModel.LinkOuLocal,
            idsContatos
        );

        Result resultado = servicoCompromisso.Editar(dto);

        if (resultado.IsFailed)
        {
            if (!resultado.Errors.Any(e => e.HasMetadataKey("Campo")))
            {
                TempData["MensagemErro"] = resultado.Errors.First().Message;

                return RedirectToAction(nameof(Listar));
            }

            ViewBag.Contatos = servicoContato.SelecionarTodas();

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
        Result<CompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        CompromissoDto dto = resultado.Value;

        ViewBag.Contatos = servicoContato.SelecionarTodas();

        ExcluirCompromissoViewModel viewModel = new(
            dto.Id,
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.TipoCompromisso,
            dto.LinkOuLocal,
            dto.Contatos
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCompromissoViewModel excluirVm)
    {
        Result resultado = servicoCompromisso.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(Listar));
    }

}
