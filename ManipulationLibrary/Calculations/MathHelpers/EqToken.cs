// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		EqToken.cs
//  Summary:	The EqTokens are the tokens that store the operations, values, 
//   functions and constants to be used in solving an equation.
// ==================================================================================

using System;

namespace ManipulationLibrary.Calculations.MathHelpers
{
    [Serializable]
    public class EqToken
    {
        private double _dvalue;

        public EqToken()
        {
        }

        public EqToken(string value)
        {
            Value = value;
            Type = TokenCategory.Invalid;

            double dvalue;
            if (Double.TryParse(value, out dvalue))
            {
                // Number
                Type = TokenCategory.Number;
                DValue = dvalue;
                NumberOfOperands = 0;
                return;
            }

            switch (value)
            {
                case ",": Type = TokenCategory.ArgumentSeparator;
                    return;
                case "(":
                    Type = TokenCategory.LeftParenthesis;
                    return;
                case ")":
                    Type = TokenCategory.RightParenthesis;
                    return;
            }
            
            if (EqOperator.Operators.ContainsKey(value))
            {
                var op = EqOperator.Operators[value];

                if (op.Type == TokenCategory.Constant)
                {
                    DValue = Op.Calculate();
                    Type = TokenCategory.Number;
                    NumberOfOperands = 0;
                    return;
                }
                Op = op;
                Type = Op.Type;
                NumberOfOperands = 1;
            }
        }

        public double DValue
        {
            get { return _dvalue; }
            set
            {
                _dvalue = value;
                Value = String.Format("{0:0.0#####}", _dvalue);
            }
        }

        public int NumberOfOperands { get; set; } // For Functions only
        public EqOperator Op { get; set; }
        public TokenCategory Type { get; set; }
        public string Value { get; set; }
    }
}