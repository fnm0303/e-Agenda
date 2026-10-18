using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Despesas;

namespace eAgenda.Aplicacao.Modulos.Compromissos;

public record CompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LinkOuLocal,
    List<Contato> Contatos
);

public record CadastrarCompromissoDto(
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LinkOuLocal,
    List<Contato> Contatos
);

public record EditarCompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LinkOuLocal,
    List<Contato> Contatos
);

