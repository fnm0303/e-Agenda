using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Compartilhado.Orm;

namespace eAgenda.Infraestrutura.Modulos.Tarefas;

public sealed class RepositorioTarefaEmOrm(eAgendaDbContext dbContext) : RepositorioBaseEmOrm<Tarefa>(dbContext), IRepositorioTarefa
{
    public override Tarefa? SelecionarPorId(Guid idSelecionado)
    {
        return base.SelecionarPorId(idSelecionado);
    }

    public override List<Tarefa> SelecionarTodos()
    {
        return registros
            .OrderByDescending(t => t.Prioridade)
            .ThenBy(t => t.DataCriacao)
            .ToList();
    }
}
