namespace eAgenda.Aplicacao.Modulos.Categorias;

public record CategoriaDto(
    Guid Id,
    string Titulo
);
//DTO = Data transfer object
public record CadastrarCategoriaDto(
    string Titulo
);
