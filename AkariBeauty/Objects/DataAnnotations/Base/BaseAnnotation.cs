using System.Reflection;

namespace AkariBeauty.Objects.Dtos.DataAnnotations.Base;

[AttributeUsage(AttributeTargets.Property)]
public abstract class BaseAnnotation : Attribute
{

    private PropertyInfo _property;
    private object _value;

    public string ErrorMessage { get; set; } = null!;

    public object[]? Parameters { get; set; }

    protected object Value
    {
        get => GetValue();
        set => SetValue(value);
    }

    public BaseAnnotation(params object[]? parameters)
    {
        Parameters = parameters;
    }

    public void Initialize(PropertyInfo property, object value)
    {
        _property = property;
        _value = value;
        Console.WriteLine("Executou o construtor BaseAnnotation");
    }

    public abstract void Execute();

    protected object GetValue()
    {
        return _property.GetValue(_value);
    }

    protected void SetValue(object newValue)
    {
        _property.SetValue(_value, newValue);
    }

    protected void ReturnError(string? mensage = null)
    {
        if (ErrorMessage is not null)
            throw new ArgumentException(ErrorMessage);
        
        if (mensage is not null)
            throw new ArgumentException(mensage);

        throw new ArgumentException("Erro padrão de anotação");
    }
}