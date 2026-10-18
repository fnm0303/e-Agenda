using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.Dominio.Modulos.Compromissos;

public interface IRepositorioCompromisso : IRepositorio<Compromisso>
{
    public bool ExisteConflito(DateOnly dataOcorrencia, TimeOnly horaInicio, TimeOnly horaTermino, Guid? idIgnorar = null);
}
