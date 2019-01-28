// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		Equation.cs
//  Summary:	This class utilizes reverse polish notation to solve equations. 
// ==================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ManipulationLibrary.Calculations.MathHelpers
{
    public enum TokenCategory
    {
        Invalid,
        Number,
        Constant,
        Function,
        ArgumentSeparator,
        Operator,
        LeftParenthesis,
        RightParenthesis,
    }

    [Serializable]
    public class Equation
    {
        // Cache the patterns as static variables to avoid recreating them each time.
        private static string _ptnSpacer;
        private static string _ptnNegative;
        
        public static double Solve(string equation)
        {
            var clean = Clean(equation);
            var postfix = Postfix(clean);
            return Solve(postfix);
        }

        /// <summary>
        /// Clean the infix expression, spacing out the variables properly
        /// </summary>
        /// <param name="dirty">the dirty expression</param>
        /// <returns>the clean expression</returns>
        private static string Clean(string dirty)
        {
            var clean = dirty.ToLower();

            // Build and save patterns as static objects
            if (String.IsNullOrEmpty(_ptnSpacer))
            {
                BuildCleaningPatterns();
            }

            // Space out sections
            clean = Regex.Replace(clean, _ptnSpacer, " ${match} ");

            // Clean whitespace
            clean = Regex.Replace(clean, @"\s+", " ").Trim();

            // Captures negative sign operations:
            clean = clean.Replace("-", "neg");
            clean = Regex.Replace(clean, _ptnNegative, "${match} -");

            return clean;
        }

        /// <summary>
        /// Build the static patterns used to clean the infix equation
        /// </summary>
        private static void BuildCleaningPatterns()
        {
            const string ptnNumbers = @"\d+(\.\d+)?";

            // Put the operators in a list and sort them in descending length
            var operatorList = EqOperator.Operators.Keys.ToList();
            operatorList.Sort((a, b) => b.Length.CompareTo(a.Length));

            // Build the stringbuilders to assist with the patterns
            var ptnFunctionsAndConstants = new StringBuilder();
            var ptnSingleCharOperators = new StringBuilder("[");
            var ptnOperators = new StringBuilder();
            foreach (var op in operatorList)
            {
                if (op.Length==1)
                {
                    ptnSingleCharOperators.Append(op);
                } else
                {
                    ptnOperators.Append(op);
                    ptnOperators.Append('|');
                }
                if (EqOperator.Operators[op].Type == TokenCategory.Function 
                    || EqOperator.Operators[op].Type == TokenCategory.Constant)
                {
                    ptnFunctionsAndConstants.Append('|');
                    ptnFunctionsAndConstants.Append(op);
                }
            }
            ptnFunctionsAndConstants.Remove(0,1);
            ptnSingleCharOperators.Replace("-", @"\-");
            ptnSingleCharOperators.Append(']'); 
            ptnOperators.Append(ptnSingleCharOperators.ToString());

            _ptnSpacer = String.Format("(?<match>({0}|[()]|{1}))",
                                       ptnNumbers, ptnOperators);

                
            // How do we handle - - or + - or sin(2) * - sin?
            
            // - 4 - 5 + - 6
            // replace all negative numbers with neg
            // neg 4 neg 5 + neg 6
            // replace number or function(x) neg with x -
            // neg 4 - 5 + neg 6
                
            _ptnNegative = String.Format("(?<match>({0}|{1}))\\sneg", ptnNumbers,
                                         ptnFunctionsAndConstants);
        }

        /// <summary>
        /// Simplify an equation in postfix form down to a single double answer.
        /// </summary>
        /// <param name="postfix">The enumerable postfix tokens</param>
        /// <returns>A double answer</returns>
        private static double Solve(IEnumerable<EqToken> postfix)
        {
            var stack = new Stack<double>();
            var values = new List<double>();

            // Process each item in the postfix
            foreach (var token in postfix)
            {
                if (token.Type == TokenCategory.Number)
                {
                    // Number is the most common occurrence, check for it and process first.
                    stack.Push(token.DValue);
                }
                else if (stack.Count >= 1)
                {
                    // Get the operands for the functions
                    int o = token.NumberOfOperands;
                    if (token.Type==TokenCategory.Operator)
                    { // Use MinArguments for Operators.
                        o = token.Op.MinArguments;
                    }
                    values.Clear();
                    for (var i = 0; i < o; i++)
                    {
                        values.Add(stack.Pop());
                    }

                    // Perform the calculation and push the result onto the stack
                    stack.Push(token.Op.Calculate(values.ToArray()));
                }
            }

            // If there is more than one result, then the formula was not valid.
            if (stack.Count > 1)
                throw new ArgumentException("Error: Invalid formula.");

            // Return the answer
            return stack.Pop();
        }

        /// <summary>
        /// Convert and Infix equation to Postfix
        /// </summary>
        /// <param name="infix">A clean infix (space delimited) equation</param>
        /// <returns>A tokenized postfix equation</returns>
        private static IEnumerable<EqToken> Postfix(string infix)
        {
            var output = new List<EqToken>();
            var stack = new Stack<EqToken>();
            var functions = new Stack<EqToken>();

            // For each token in the infix expression
            foreach (var token in infix.Split(' ').Select(t => new EqToken(t)))
            {
                switch (token.Type)
                {
                    case TokenCategory.Invalid:
                        continue;
                    case TokenCategory.Number:
                        // Token is a number, add it to the output queue
                        output.Add(token);
                        continue;
                    case TokenCategory.Function:
                        // Token is a function, push it onto the stack and onto the functions stack for operand counting
                        stack.Push(token);
                        
                        // push the function onto the function stack.
                        functions.Push(token);
                        continue;
                    case TokenCategory.ArgumentSeparator:
                        // Increase the number of Operands each time an argument separator
                        if (functions.Count == 0)
                        {
                            throw new ArgumentException("Error: Arguments specified not in a function.");
                        }
                        functions.Peek().NumberOfOperands++;

                        // Pop a neg if it is at the top of the stack. This is because neg doesn't use parenthesis.
                        if (stack.Peek().Value == "neg")
                        {
                            output.Add(stack.Pop());
                        }

                        break;
                    case TokenCategory.Operator:
                        while (stack.Count > 0 && 
                            (stack.Peek().Type == TokenCategory.Operator || 
                             stack.Peek().Type == TokenCategory.Function))
                        {
                            var token2 = stack.Peek();

                            // Check precedence & Associative Properties
                            if ((token.Op.IsLeftAssociative && token.Op.Precedence <= token2.Op.Precedence) ||
                                (token.Op.IsRightAssociative && token.Op.Precedence < token2.Op.Precedence))
                            {
                                // pop toke2 and push it onto the stack.
                                output.Add(stack.Pop());
                            }
                            else
                            {
                                break;
                            }
                        }
                        // Push the current token onto the stack
                        stack.Push(token);
                        continue;
                    case TokenCategory.LeftParenthesis:
                        // Push the left parenthesis token onto the stack.
                        stack.Push(token);
                        continue;
                    case TokenCategory.RightParenthesis:
                        // If the stack is empty, then there is nothing else to do
                        if (stack.Count == 0) continue;

                        // Pop the next item off of the stack and add it to the postfix
                        while (stack.Count != 0 && stack.Peek().Type != TokenCategory.LeftParenthesis)
                        {
                            output.Add(stack.Pop());
                        }

                        // If it is balanced, it should exit before reaching 0
                        if (stack.Count == 0)
                            throw new ArgumentException("Error: Unbalanced parenthesis in formula.");

                        // Remove the left Parenthesis
                        stack.Pop();

                        // If the item at the top of the stack is a function... 
                        if (stack.Count > 0 &&
                            stack.Peek().Type == TokenCategory.Function)
                        {
                            // Pop a funciton off of the stack to stop increasing its operand count
                            functions.Pop();
                            // And add it to the postfix expression
                            output.Add(stack.Pop());
                        }
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Clear the stack to the postfix expression
            while (stack.Count != 0)
            {
                if (stack.Peek().Type == TokenCategory.LeftParenthesis)
                {
                    throw new ArgumentException("Error: Unbalanced parenthesis in formula.");
                }
                // Add it to the postfix expression
                output.Add(stack.Pop());
            }

            return output;
        }
    }
}
