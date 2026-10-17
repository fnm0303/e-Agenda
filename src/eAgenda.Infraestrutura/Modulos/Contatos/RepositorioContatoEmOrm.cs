using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Modulos.Contatos;

public sealed class RepositorioContatoEmOrm(eAgendaDbContext dbContext) : RepositorioBaseEmOrm<Contato>(dbContext), IRepositorioContato
{
    public override Contato? SelecionarPorId(Guid idSelecionado)
    {
        Contato? contato = registros
            //.Include(c => c.Compromissos)
            .SingleOrDefault(t => t.Id == idSelecionado);

        return contato;
    }

    public override List<Contato> SelecionarTodos()
    {
        List<Contato> contatos = registros
            .OrderBy(c => c.Nome)
            //.Include(c => c.Compromissos)
            .ToList();

        return contatos;
    }

    public bool ExisteEmail(string email, Guid? idAtual = null)
    {
        // Verifica se já existe outro contato com esse e-mail (útil para inserção e edição)
        return registros.Any(c => c.Email == email && (!idAtual.HasValue || c.Id != idAtual.Value));
    }

    public bool ExisteTelefone(string telefone, Guid? idAtual = null)
    {
        return registros.Any(c => c.Telefone == telefone && (!idAtual.HasValue || c.Id != idAtual.Value));
    }
}
