using System.Text.RegularExpressions;
using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.Dominio.Modulos.Contatos;

public sealed class Contato : EntidadeBase<Contato>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;

    public Contato() { }
    public Contato(string nome, string email, string telefone) : this()
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }
    public override List<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(Nome) || Nome.Length < 2 || Nome.Length > 100)
            erros.Add(new ErroValidacao { Campo = nameof(Nome), Mensagem = "O campo \"Nome\" deve conter entre 2 e 100 caracteres." });

        string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, emailRegex))
            erros.Add(new ErroValidacao { Campo = nameof(Email), Mensagem = "O campo \"Email\" deve ser um endereço de e-mail válido." });

        string telefoneRegex = @"^\(\d{2}\)\s?\d{4,5}-\d{4}$";
        if (string.IsNullOrWhiteSpace(Telefone) || !Regex.IsMatch(Telefone, telefoneRegex))
        {
            erros.Add(new ErroValidacao { Campo = nameof(Telefone), Mensagem = "O campo \"Telefone\" deve estar no formato (XX) XXXXX-XXXX ou (XX) XXXX-XXXX." });
        }

        return erros;
    }

    public override void Atualizar(Contato entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Email = entidadeAtualizada.Email;
        Telefone = entidadeAtualizada.Telefone;
        Cargo = entidadeAtualizada.Cargo;
        Empresa = entidadeAtualizada.Empresa;
    }

}
