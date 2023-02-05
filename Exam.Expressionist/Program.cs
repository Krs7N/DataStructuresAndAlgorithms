using System;

namespace Exam.Expressionist
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Expressionist expressionist = new Expressionist();
            Expression leftChild = new Expression("2", "5", ExpressionType.Value, null, null);
            Expression rightChild = new Expression("3", "4", ExpressionType.Value, null, null);
            Expression expression2 = new Expression("4", "6", ExpressionType.Value, null, null);
            Expression expression3 = new Expression("5", "7", ExpressionType.Value, null, null);
            Expression expression4 = new Expression("6", "8", ExpressionType.Value, null, null);
            Expression expression5 = new Expression("7", "9", ExpressionType.Value, null, null);
            Expression expression = new Expression("1", "+", ExpressionType.Operator, null, null);


            expressionist.AddExpression(expression);
            expressionist.AddExpression(leftChild, "1");
            expressionist.AddExpression(rightChild, "1");
            expressionist.AddExpression(expression2, "2");
            expressionist.AddExpression(expression3, "2");
            expressionist.AddExpression(expression4, "3");
            expressionist.AddExpression(expression5, "3");

            expressionist.Contains(rightChild);

            expressionist.Evaluate();

            expressionist.RemoveExpression("3");
        }
    }
}
