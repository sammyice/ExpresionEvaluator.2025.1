using System.Globalization;

namespace Evaluator.Logic;

public class FunctionEvaluator
{
    public static double Evalute(string infix)
    {
        var postfix = ToPostfix(infix);
        return Calculate(postfix);
    }

    private static double Calculate(string postfix)
    {
        var stack = new Stack<double>();
        foreach (var item in postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (item.Length == 1 && IsOperator(item[0]))
            {
                var operator2 = stack.Pop();
                var operator1 = stack.Pop();
                stack.Push(Result(operator1, item[0], operator2));
            }
            else
            {
                double value = double.Parse(item, CultureInfo.InvariantCulture);
                stack.Push(value);
            }
        }
        return stack.Pop();
    }

    private static double Result(double operator1, char item, double operator2)
    {
        return item switch
        {
            '+' => operator1 + operator2,
            '-' => operator1 - operator2,
            '*' => operator1 * operator2,
            '/' => operator1 / operator2,
            '^' => Math.Pow(operator1, operator2),
            _ => throw new Exception("Invalid expresion"),
        };
    }

    private static string ToPostfix(string infix)
    {
        var stack = new Stack<char>();
        var postfix = string.Empty;
        for (int i = 0; i < infix.Length; i++)
        {
            char item = infix[i];
            if (IsOperator(item))
            {
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')
                    {
                        do
                        {
                            postfix +=  stack.Pop() + " ";
                        } while (stack.Peek() != '(');
                        stack.Pop();
                    }
                    else
                    {
                        if (PriorityExpression(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            postfix +=  stack.Pop() + " ";
                            stack.Push(item);
                        }
                    }
                }
            }
            else
            {
                string number = item.ToString();
                while (i + 1 < infix.Length && (char.IsDigit(infix[i + 1]) || infix[i + 1] == '.'))
                {
                    i++;
                    number += infix[i];
                }
                postfix += number + " ";
            }
        }
        do
        {
            postfix += stack.Pop();
        } while (stack.Count > 0);
        return postfix;
    }

    private static int PriorityStack(char item)
    {
        return item switch
        {
            '^' => 3,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 0,
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static int PriorityExpression(char item)
    {
        return item switch
        {
            '^' => 4,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 5,
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static bool IsOperator(char item) => "()^*/+-".IndexOf(item) >= 0;
}