using Calculator.Models;

namespace Calculator.Utils;

public static class Calculate
{
    public static string ConvertOperatorToString(Operator mode) => mode switch
    {
        Operator.Add => "+",
        Operator.Minus => "-",
        Operator.Multiply => "×",
        Operator.Divide => "÷",
        _ => string.Empty
    };
}