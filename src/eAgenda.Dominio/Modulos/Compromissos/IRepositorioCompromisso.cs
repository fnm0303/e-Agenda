using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.Dominio.Modulos.Compromissos;

public interface IRepositorioCompromisso : IRepositorio<Compromisso>
{
    public bool ExisteConflito(DateTime dataInicio, DateTime dataTermino, Guid? idIgnorar = null);
}
