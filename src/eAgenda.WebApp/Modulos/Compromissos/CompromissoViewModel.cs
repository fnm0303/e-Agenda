using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Despesas;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[ValidateNever]
public record CadastrarCompromissoViewModel(
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LinkOuLocal,
    List<Guid> ContatosSelecionados
);

[ValidateNever]
public record EditarCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LinkOuLocal,
    List<Guid> ContatosSelecionados
);

[ValidateNever]
public record ExcluirCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LinkOuLocal,
    List<Contato> Contatos
);