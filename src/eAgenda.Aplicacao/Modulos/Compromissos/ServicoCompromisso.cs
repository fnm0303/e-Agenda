using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.WebApp.Compartilhado.Dominio;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Compromissos;

public sealed class ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso, IRepositorioContato repositorioContato)
{
    public Result<Guid> Cadastrar(CadastrarCompromissoDto dto)
    {
        List<Contato> contatosSelecionados = repositorioContato
            .SelecionarTodos()
            .Where(c => dto.ContatosIds.Contains(c.Id))
            .ToList();

        Compromisso compromisso = new(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.TipoCompromisso,
            dto.LinkOuLocal,
            contatosSelecionados
        );

        List<ErroValidacao> erros = compromisso.Validar();

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        bool temConflito = repositorioCompromisso.ExisteConflito(dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino);

        if (temConflito)
        {
            return Result.Fail(new Error("Já existe um compromisso agendado neste mesmo dia e horário.")
                .WithMetadata("Campo", nameof(dto.HoraInicio)));
        }

        repositorioCompromisso.Cadastrar(compromisso);

        return Result.Ok(compromisso.Id);

    }

    public Result Editar(EditarCompromissoDto dto)
    {
        List<Contato> contatosSelecionados = repositorioContato
            .SelecionarTodos()
            .Where(c => dto.ContatosIds.Contains(c.Id))
            .ToList();

        Compromisso compromissoAtualizado = new(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.TipoCompromisso,
            dto.LinkOuLocal,
            contatosSelecionados
        );


        List<ErroValidacao> erros = compromissoAtualizado.Validar();

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        bool temConflito = repositorioCompromisso.ExisteConflito(dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino, dto.Id);

        if (temConflito)
        {
            return Result.Fail(new Error("Já existe um compromisso agendado neste mesmo dia e horário.")
                .WithMetadata("Campo", nameof(dto.HoraInicio)));
        }

        bool conseguiuEditar = repositorioCompromisso.Editar(dto.Id, compromissoAtualizado);

        if (!conseguiuEditar)
            return Result.Fail("Não foi possível editar o compromisso.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioCompromisso.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir o compromisso.");

        return Result.Ok();
    }

    public Result<CompromissoDto> SelecionarPorId(Guid id)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Compromisso não encontrado");

        CompromissoDto dto = new(
            compromisso.Id,
            compromisso.Assunto,
            compromisso.DataOcorrencia,
            compromisso.HoraInicio,
            compromisso.HoraTermino,
            compromisso.TipoCompromisso,
            compromisso.LinkOuLocal,
            compromisso.Contatos);

        return Result.Ok(dto);
    }

    public List<CompromissoDto> SelecionarTodas()
    {
        return repositorioCompromisso
            .SelecionarTodos()
            .Select(c => new CompromissoDto(
                c.Id,
                c.Assunto,
                c.DataOcorrencia,
                c.HoraInicio,
                c.HoraTermino,
                c.TipoCompromisso,
                c.LinkOuLocal,
                c.Contatos
            )).ToList();
    }
}
