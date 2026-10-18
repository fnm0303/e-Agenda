using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Despesas;
using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.Dominio.Modulos.Compromissos;

public sealed class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateOnly DataOcorrencia { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraTermino { get; set; }
    public TipoCompromisso TipoCompromisso { get; set; }
    public string LinkOuLocal { get; set; } = string.Empty;
    public List<Contato> Contatos { get; set; } = [];

    public Compromisso() { }

    public Compromisso(string assunto, DateOnly dataOcorrencia, TimeOnly horaInicio, TimeOnly horaTermino, TipoCompromisso tipoCompromisso, string linkOuLocal, List<Contato> contatos)
    {
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        TipoCompromisso = tipoCompromisso;
        LinkOuLocal = linkOuLocal;
        Contatos = contatos;
    }

    public override List<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(Assunto) || Assunto.Length < 2 || Assunto.Length > 100)
            erros.Add(new ErroValidacao { Campo = nameof(Assunto), Mensagem = "O campo \"Assunto\" deve conter entre 2 e 100 caracteres." });

        if (DataOcorrencia == DateOnly.MinValue)
            erros.Add(new ErroValidacao { Campo = nameof(DataOcorrencia), Mensagem = "O campo \"Data de Ocorrência\" deve conter uma data válida." });

        if (HoraInicio == TimeOnly.MinValue)
            erros.Add(new ErroValidacao { Campo = nameof(HoraInicio), Mensagem = "O campo \"Hora de Início\" deve conter um horário válido." });

        if (HoraTermino == TimeOnly.MinValue)
            erros.Add(new ErroValidacao { Campo = nameof(HoraTermino), Mensagem = "O campo \"Hora de Término\" deve conter um horário válido." });
        else if (HoraTermino <= HoraInicio)
            erros.Add(new ErroValidacao { Campo = nameof(HoraTermino), Mensagem = "O campo \"Hora de Término\" deve ser posterior à hora de início." });

        if (string.IsNullOrWhiteSpace(LinkOuLocal))
            erros.Add(new ErroValidacao { Campo = nameof(LinkOuLocal), Mensagem = "O campo \"Local ou Link\" deve ser preenchido." });

        return erros;
    }

    public override void Atualizar(Compromisso entidadeAtualizada)
    {
        Assunto = entidadeAtualizada.Assunto;
        DataOcorrencia = entidadeAtualizada.DataOcorrencia;
        HoraInicio = entidadeAtualizada.HoraInicio;
        HoraTermino = entidadeAtualizada.HoraTermino;
        TipoCompromisso = entidadeAtualizada.TipoCompromisso;
        LinkOuLocal = entidadeAtualizada.LinkOuLocal;
        Contatos = entidadeAtualizada.Contatos;
    }
}
