using System;
using System.Collections.Generic;

namespace Liyanjie.Contents.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class _ArithmeticModel
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

        readonly char[] operators = { '+', '-', '×', '÷' };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public (string[] Equation, int Answer) Build(VerificationCodeOptions options)
        {
            var random = new Random();

            int a, b, c, oi = random.Next(operators.Length);
            var o = operators[oi];
            switch (o)
            {
                case '+':
                    a = random.Next(MaxWhenAddition);
                    b = random.Next(MaxWhenAddition);
                    c = a + b;
                    break;
                case '-':
                    int tmp = MaxWhenSubtraction / 2;
                    a = random.Next(tmp, MaxWhenSubtraction);
                    b = random.Next(tmp);
                    c = a - b;
                    break;
                case '×':
                    a = random.Next(MaxWhenMultiplication);
                    b = random.Next(MaxWhenMultiplication);
                    c = a * b;
                    break;
                case '÷':
                    a = random.Next(1, MaxWhenDivision);
                    var divisors = a.Divisors();
                    b = divisors[random.Next(divisors.Count)];
                    c = a / b;
                    break;
                default:
                    o = '+';
                    a = random.Next(MaxWhenAddition);
                    b = random.Next(MaxWhenAddition);
                    c = a + b;
                    break;
            }
            var equation = new List<string>
            {
                a.ToString(),
                UseZhInsteadOfOperator ? options.ArithmeticOperatorsText[oi] : o.ToString(),
                b.ToString(),
                UseZhInsteadOfOperator ? "等于" : "="
            };
            if (EndWithQuestion)
                equation.Add("?");

            return (equation.ToArray(), c);
        }
    }
}
