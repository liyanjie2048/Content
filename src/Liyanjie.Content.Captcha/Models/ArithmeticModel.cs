namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ArithmeticModel
{
    /// <summary>
    /// 
    /// </summary>
    public int MaxWhenAddition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int MaxWhenSubtraction { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int MaxWhenMultiplication { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int MaxWhenDivision { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool UseZhInsteadOfOperator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool EndWithQuestion { get; set; }

    readonly string[] operators = { "+", "-", "×", "÷" };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public (string[] Equation, int Answer) Build(CaptchaOptions options)
    {
        var random = new Random();

        int x, y, z;
        var @operator = operators[random.Next(operators.Length)];
        switch (@operator)
        {
            case "+":
                x = random.Next(MaxWhenAddition);
                y = random.Next(MaxWhenAddition);
                z = x + y;
                break;
            case "-":
                int tmp = MaxWhenSubtraction / 2;
                x = random.Next(tmp, MaxWhenSubtraction);
                y = random.Next(tmp);
                z = x - y;
                break;
            case "×":
                x = random.Next(MaxWhenMultiplication);
                y = random.Next(MaxWhenMultiplication);
                z = x * y;
                break;
            case "÷":
                x = random.Next(1, MaxWhenDivision);
                var divisors = x.GetDivisors();
                y = divisors[random.Next(divisors.Count)];
                z = x / y;
                break;
            default:
                @operator = "+";
                x = random.Next(MaxWhenAddition);
                y = random.Next(MaxWhenAddition);
                z = x + y;
                break;
        }
        var equation = new List<string>
        {
            x.ToString(),
            UseZhInsteadOfOperator ? options.ArithmeticOperatorsText[@operator] : @operator.ToString(),
            y.ToString(),
            UseZhInsteadOfOperator ? "等于" : "="
        };
        if (EndWithQuestion)
            equation.Add("?");

        return (equation.ToArray(), z);
    }
}
