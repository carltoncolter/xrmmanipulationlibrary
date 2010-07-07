// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		EqOperator.cs
//  Summary:	This class manages the individual operations and functions used to 
//   solve equations dynamically, new functions can be added in the static 
//   constructor using either the Constant and Operator constructor or the Function
//   constructor.
// ==================================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace ManipulationLibrary.Calculations.Helpers
{
    [Serializable]
    public class EqOperator
    {
        public delegate double CalculationMethod(params double[] operands);

        private readonly CalculationMethod _calculation;

        static EqOperator()
        {
            Operators = new Dictionary<string, EqOperator>
            {
                {"e", new EqOperator("e", TokenCategory.Constant, 0, 0, true, p => Math.E)},
                {"pi", new EqOperator("pi", TokenCategory.Constant, 0, 0, true, p => Math.PI)},
                {"+", new EqOperator("add", TokenCategory.Operator, 1, 2, true, p => p[1] + p[0])},
                {"-", new EqOperator("substract", TokenCategory.Operator, 1, 2, true, p => p[1] - p[0])},
                {"*", new EqOperator("multiply", TokenCategory.Operator, 2, 2, true, p => p[1] * p[0])},
                {"/", new EqOperator("divide", TokenCategory.Operator, 2, 2, true, p => p[1] / p[0])},
                {"%", new EqOperator("mod", TokenCategory.Operator, 2, 2, true, p => p[1] % p[0])},
                {"^", new EqOperator("power", TokenCategory.Operator, 3, 2, true, p => p[1] + p[0])},
                {"neg", new EqOperator("neg", TokenCategory.Operator, 10, 1, true, p => -1 * p[0])},
                {"abs", new EqOperator("abs", TokenCategory.Function, 1, p => Math.Abs(p[0]))},
                {"acos", new EqOperator("acos", TokenCategory.Function, 1, p => Math.Acos(p[0]))},
                {"asin", new EqOperator("asin", TokenCategory.Function, 1, p => Math.Asin(p[0]))},
                {"atan", new EqOperator("atan", TokenCategory.Function, 1, p => Math.Atan(p[0]))},
                {"sin", new EqOperator("sin", TokenCategory.Function, 1, p => Math.Sin(p[0]))},
                {"cos", new EqOperator("cos", TokenCategory.Function, 1, p => Math.Cos(p[0]))},
                {"tan", new EqOperator("tan", TokenCategory.Function, 1, p => Math.Tan(p[0]))},
                {"radians",new EqOperator("radians", TokenCategory.Function, 1, p => Math.PI * (p[0] / 180.0))},
                {"degrees",new EqOperator("degrees", TokenCategory.Function, 1, p => p[0] * (Math.PI / 180.0))},
                {"sqrt", new EqOperator("sqrt", TokenCategory.Function, 1, p => Math.Sqrt(p[0]))},
                {"round", new EqOperator("round", TokenCategory.Function, 1, 2, 
                    p => p.Length == 1 ? Math.Round(p[0]) : Math.Round(p[1],Convert.ToInt32(Math.Truncate(p[0]))))},
                {"trunc", new EqOperator("trunc", TokenCategory.Function, 1, p => Math.Truncate(p[0]))},
                {"log10", new EqOperator("log10", TokenCategory.Function, 1, p => Math.Log10(p[0]))},
                {"log", new EqOperator("log", TokenCategory.Function, 2, p => Math.Log(p[1], p[0]))},
                {"even", new EqOperator("even", TokenCategory.Function, 1, p => 
                    {
                        var t = Math.Truncate(p[0]);
                        if (t == p[0] &&
                            t % 2 == 0)
                        {
                            return t;
                        }
                        t++;
                        if (t % 2 != 0)
                        {
                            t++;
                        }
                        return t;
                    })},
                {"odd", new EqOperator("odd", TokenCategory.Function, 1, p =>
                    {
                        var t = Math.Truncate(p[0]);
                        if (t == p[0] && t % 2 == 1)
                        {
                            return t;
                        }
                        t++;
                        if (t % 2 != 1)
                        {
                            t++;
                        }
                        return t;
                    })},
                {"randbetween", new EqOperator("randbetween", TokenCategory.Function, 2, p =>
                    {
                        var min = Convert.ToInt32(Math.Truncate(p[1]));
                        var max = Convert.ToInt32(Math.Truncate(p[0]));
                        if (min > max)
                        {
                            var tmp = max;
                            max = min;
                            min = tmp;
                        }
                        var random = new Random();
                        return random.Next(min, max);
                    })},
                {"rand", new EqOperator("rand", TokenCategory.Constant, 0, 0, true, p =>
                    {
                        var random = new Random();
                        return random.Next(0, 10000) /10000.0;
                    })},
                {"min", new EqOperator("min", TokenCategory.Function, 2, 20, p =>
                    {
                        var min = p[0];
                        for (var i = 1; i < p.Length; i++)
                        {
                            if (min > p[i])
                            {
                                min = p[i];
                            }
                        }
                        return min;
                    })},
                {"max", new EqOperator("max", TokenCategory.Function, 2, 20, p =>
                    {
                        var max = p[0];
                        for (var i = 1; i < p.Length; i++)
                        {
                            if (max < p[i])
                            {
                                max = p[i];
                            }
                        }
                        return max;
                    })},
                {"!", new EqOperator("!", TokenCategory.Operator, 3, 1, false, p =>
                    {
                        var f = Convert.ToInt32(Math.Abs(Math.Truncate(p[0])));
                        var r = f;
                        for (var i = 2;
                            i < f;
                            i++)
                        {
                            r *= f;
                        }
                        return r;
                    })},
                {"fact", new EqOperator("fact", TokenCategory.Function, 1, p =>
                    {
                        var f = Convert.ToInt32(Math.Abs(Math.Truncate(p[0])));
                        var r = f;
                        for (var i = 2;
                            i < f;
                            i++)
                        {
                            r *= f;
                        }
                        return r;
                    })},
                {"gcf", new EqOperator("gcf", TokenCategory.Function, 2, 20, GCF)},
                {"sign", new EqOperator("sign", TokenCategory.Function, 1, p => p[0].CompareTo(0.0))},
                {"power", new EqOperator("power", TokenCategory.Function, 2, p => Math.Pow(p[1], p[0]))}
            };

            // Function Example:
            // Operators.Add("log", new EqOperator("log", TokenCategory.Function, 2, p => Math.Log(p[1], p[0])));

            // Operator Example:
            // Operators.Add("/", new EqOperator("divide", TokenCategory.Operator, 2, 2, true, p => p[1] / p[0]));
        }

        public EqOperator()
        {
        }

        // For Operators and Constants
        public EqOperator(string name, TokenCategory type, int precedence, int arguments, bool isLeftAssociated,
                          CalculationMethod calculationMethod)
        {
            Name = name;
            Type = type;
            IsLeftAssociative = isLeftAssociated;
            Precedence = precedence;
            _calculation = calculationMethod;
            MaxArguments = -1; // Default Max Arguments - Unlimited
            MinArguments = arguments;
        }

        // For Functions
        public EqOperator(string name, TokenCategory type, int maxArgs, CalculationMethod calculationMethod)
        {
            Name = name;
            Type = type;
            IsLeftAssociative = true;
            Precedence = 8; // functions have the highest precedence!
            _calculation = calculationMethod;
            MaxArguments = maxArgs;
            MinArguments = maxArgs;
        }

        // For Functions
        public EqOperator(string name, TokenCategory type, int minArgs, int maxArgs, CalculationMethod calculationMethod)
        {
            Name = name;
            Type = type;
            IsLeftAssociative = true;
            Precedence = 0;
            _calculation = calculationMethod;
            MaxArguments = maxArgs;
            MinArguments = minArgs;
        }

        public static Dictionary<string, EqOperator> Operators { get; set; }
        public bool IsLeftAssociative { get; private set; }

        public bool IsRightAssociative
        {
            get { return !IsLeftAssociative; }
        }

        public int MaxArguments { get; private set; }
        public int MinArguments { get; private set; }
        public string Name { get; private set; }
        public int Precedence { get; private set; }
        public TokenCategory Type { get; private set; }

        // Perform a calculation
        public double Calculate(params double[] operands)
        {
            if (MaxArguments != -1 && operands.Length > MaxArguments)
            {
                throw new ArgumentException(String.Format("Error: {0} only supports {1} argument{2}.",
                                                          Name, MaxArguments, (MaxArguments > 1) ? "s" : ""));
            }
            return _calculation(operands);
        }

        /// <summary>
        /// GCF is an example function to show how you can add more functions to this math library.
        /// </summary>
        /// <param name="p">The array of parameters</param>
        /// <returns>The double result</returns>
        private static double GCF(double[] p)
        {
            var i = new int[p.Length];
            var min = 0;
            for (var j = 0; j < p.Length; j++)
            {
                i[j] = Convert.ToInt32(Math.Truncate(p[j]));
                if (i[j] != p[j])
                {
                    throw new ArgumentException("Error: gcf paramters must be integers (whole numbers).");
                }
                if (Math.Abs(i[j]) < min)
                {
                    min = Math.Abs(i[j]);
                }
            }

            int n;
            for (n = min; n > 0; n--)
            {
                var x = n;
                if (i.All(t => x % t == 0))
                {
                    return x;
                }
            }
            return 1;
        }
    }
}