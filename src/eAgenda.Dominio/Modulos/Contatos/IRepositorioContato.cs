using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.Dominio.Modulos.Contatos;

public interface IRepositorioContato : IRepositorio<Contato>
{
    public bool ExisteEmail(string email, Guid? idAtual = null);

    public bool ExisteTelefone(string telefone, Guid? idAtual = null);
}
