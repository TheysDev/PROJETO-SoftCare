namespace SoftCare.Retornos;

public class Result<T>
{
    public bool Sucesso { get; }
    public T Dado { get; }
    public IEnumerable<string> Erros { get; }
    
    private Result(bool sucesso, T dado, IEnumerable<string> erros)
    {
        Sucesso = sucesso;
        Dado = dado;
        Erros = erros;
    }

    public static Result<T> Ok(T dado)
    {
        return new Result<T>(true, dado, []);
    }

    public static Result<T> Fail(string mensagemErro)
    {
        return new Result<T>(false, default(T), [mensagemErro]);
    }
    
    public static Result<T> Fail(IEnumerable<string> mensagensErros)
    {
        return new Result<T>(false, default(T), mensagensErros);
    }
}