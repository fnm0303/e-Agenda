using eAgenda.Dominio.Modulos.Tarefas;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Tarefas;

public sealed class ServicoTarefa(IRepositorioTarefa repositorioTarefa)
{
    public Result<Guid> Cadastrar(CadastrarTarefaDto dto)
    {
        return Result.Ok();
    }

    public Result<Guid> Editar(EditarTarefaDto dto)
    {
        return Result.Ok();
    }

    public Result<Guid> Excluir(Guid id)
    {
        return Result.Ok();
    }

    public TarefaDto? SelecionarPorId(Guid id)
    {
        return null;
    }
    public List<TarefaDto> SelecionarTodas()
    {
        return repositorioTarefa
            .SelecionarTodos()
            .Select(t => new TarefaDto(t.Id, t.Titulo, t.Prioridade, t.DataCriacao))
            .ToList();
    }
}
