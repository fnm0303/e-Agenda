using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using eAgenda.WebApp.Compartilhado.Dominio;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Modulos.Compromissos;

public sealed class RepositorioCompromissoEmOrm(eAgendaDbContext dbContext) : RepositorioBaseEmOrm<Compromisso>(dbContext), IRepositorioCompromisso
{
    public override Compromisso? SelecionarPorId(Guid idSelecionado)
    {
        Compromisso? compromisso = registros
            .Include(c => c.Contatos)
            .SingleOrDefault(t => t.Id == idSelecionado);

        return compromisso;
    }

    public override List<Compromisso> SelecionarTodos()
    {
        List<Compromisso> compromissos = registros
            .Include(c => c.Contatos)
            .OrderByDescending(c => c.DataOcorrencia)
            .ToList();

        return compromissos;
    }
    public bool ExisteConflito(DateOnly dataOcorrencia, TimeOnly horaInicio, TimeOnly horaTermino, Guid? idIgnorar = null)
    {
        var query = registros.Where(c => c.DataOcorrencia == dataOcorrencia);

        // Se estivermos editando, ignoramos o próprio ID para não acusar conflito com ele mesmo
        if (idIgnorar.HasValue)
        {
            query = query.Where(c => c.Id != idIgnorar.Value);
        }

        // Regra matemática de sobreposição de horários:
        // (NovoInicio < TerminoExistente) E (NovoTermino > InicioExistente)
        return query.Any(c => horaInicio < c.HoraTermino && horaTermino > c.HoraInicio);
    }
}
