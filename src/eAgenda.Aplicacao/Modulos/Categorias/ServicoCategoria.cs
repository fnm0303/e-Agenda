using eAgenda.Dominio.Modulos.Categorias;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Categorias;

public sealed class ServicoCategoria(IRepositorioCategoria repositorioCategoria)
{
    public Result<Guid> Cadastrar(CadastrarCategoriaDto dto)
    {
        Categoria categoria = new Categoria(dto.Titulo);

        List<string> erros = categoria.Validar();

        if (repositorioCategoria.ExisteCategoriaPorTitulo(categoria.Titulo))
            erros.Add("Já existe um título cadastrado com esse nome.");

        if (erros.Count > 0)
            return Result.Fail(erros[0]);

        repositorioCategoria.Cadastrar(categoria);

        return Result.Ok(categoria.Id);
    }

    public List<CategoriaDto> SelecionarTodos()
    {
        List<Categoria> categorias = repositorioCategoria.SelecionarTodos();

        return categorias
            .Select(c => new CategoriaDto(c.Id, c.Titulo))
            .ToList();
    }
}
