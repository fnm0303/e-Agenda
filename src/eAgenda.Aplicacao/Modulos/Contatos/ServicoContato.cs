using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.WebApp.Compartilhado.Dominio;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Contatos;

public sealed class ServicoContato(IRepositorioContato repositorioContato)
{
    public Result<Guid> Cadastrar(CadastrarContatoDto dto)
    {
        Contato contato = new Contato(dto.Nome, dto.Email, dto.Telefone)
        {
            Cargo = dto.Cargo ?? string.Empty,
            Empresa = dto.Empresa ?? string.Empty
        };

        List<ErroValidacao> erros = contato.Validar();

        if (repositorioContato.ExisteEmail(contato.Email))
            erros.Add(new ErroValidacao { Campo = nameof(Contato.Email), Mensagem = "Este e-mail já está cadastrado no sistema." });


        if (repositorioContato.ExisteTelefone(contato.Telefone))
            erros.Add(new ErroValidacao { Campo = nameof(Contato.Telefone), Mensagem = "Este telefone já está cadastrado no sistema." });

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        repositorioContato.Cadastrar(contato);

        return Result.Ok(contato.Id);
    }

    public Result Editar(EditarContatoDto dto)
    {
        Contato contatoAtualizado = new Contato(dto.Nome, dto.Email, dto.Telefone)
        {
            Cargo = dto.Cargo ?? string.Empty,
            Empresa = dto.Empresa ?? string.Empty
        };

        List<ErroValidacao> erros = contatoAtualizado.Validar();

        if (repositorioContato.ExisteEmail(contatoAtualizado.Email, dto.Id))
            erros.Add(new ErroValidacao { Campo = nameof(Contato.Email), Mensagem = "Este e-mail já está cadastrado em outro contato." });


        if (repositorioContato.ExisteTelefone(contatoAtualizado.Telefone, dto.Id))
            erros.Add(new ErroValidacao { Campo = nameof(Contato.Telefone), Mensagem = "Este telefone já está cadastrado em outro contato." });

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        bool conseguiuEditar = repositorioContato.Editar(dto.Id, contatoAtualizado);

        if (!conseguiuEditar)
            return Result.Fail("Não foi possível editar o contato.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioContato.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir o contato.");

        return Result.Ok();
    }

    public Result<ContatoDto> SelecionarPorId(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato não encontrado");

        ContatoDto dto = new(contato.Id, contato.Nome, contato.Email, contato.Telefone, contato.Cargo, contato.Empresa);

        return Result.Ok(dto);
    }

    public List<ContatoDto> SelecionarTodas()
    {
        return repositorioContato
            .SelecionarTodos()
            .Select(c => new ContatoDto(
                c.Id,
                c.Nome,
                c.Email,
                c.Telefone,
                c.Cargo,
                c.Empresa
            )).ToList();
    }

}
