using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Expressionist
{
    public class Expressionist : IExpressionist
    {
        private HashSet<Expression> expressions = new HashSet<Expression>();
        private Dictionary<string, Expression> expressionById = new Dictionary<string, Expression>();

        public void AddExpression(Expression expression)
        {
            if (expressions.Count != 0)
            {
                throw new ArgumentException();
            }

            expressions.Add(expression);
            expressionById.Add(expression.Id, expression);
        }

        public void AddExpression(Expression expression, string parentId)
        {
            var parentExpression = expressionById[parentId];

            if (parentExpression == null)
            {
                throw new ArgumentException();
            }

            if (parentExpression.LeftChild != null && parentExpression.RightChild != null)
            {
                throw new ArgumentException();
            }

            if (parentExpression.LeftChild == null)
            {
                parentExpression.LeftChild = expression;
            }
            else
            {
                parentExpression.RightChild = expression;
            }

            expressionById.Add(expression.Id, expression);
        }

        public bool Contains(Expression expression) => expressionById.ContainsKey(expression.Id);

        public int Count() => expressionById.Count;

        public string Evaluate()
        {
            var root = expressions.FirstOrDefault();
            return Evaluate(root);
        }

        private string Evaluate(Expression expression)
        {
            if (expression.Type == ExpressionType.Value)
            {
                return expression.Value;
            }

            var leftChild = Evaluate(expression.LeftChild);
            var rightChild = Evaluate(expression.RightChild);

            return "(" + leftChild + " " + expression.Value + " " + rightChild + ")";
        }

        public Expression GetExpression(string expressionId)
        {
            var expression = expressionById[expressionId];

            if (expression == null)
            {
                throw new ArgumentException();
            }

            return expression;
        }

        public void RemoveExpression(string expressionId)
        {
            if (!expressionById.ContainsKey(expressionId))
            {
                throw new ArgumentException();
            }

            var expression = expressionById[expressionId];
            var parentExpression = GetParentExpression(expressionId);

            if (parentExpression != null)
            {
                if (parentExpression.LeftChild != null && parentExpression.LeftChild.Id == expressionId)
                {
                    parentExpression.LeftChild = parentExpression.RightChild;
                }

                parentExpression.RightChild = null;
            }

            RemoveChildNodes(expression);
        }

        private void RemoveChildNodes(Expression expression)
        {
            if (expression.LeftChild != null)
            {
                RemoveChildNodes(expression.LeftChild);
            }

            if (expression.RightChild != null)
            {
                RemoveChildNodes(expression.RightChild);
            }

            expressionById.Remove(expression.Id);
            expressions.Remove(expression);
        }

        private Expression GetParentExpression(string expressionId)
        {
            var parentExpression = expressions.FirstOrDefault();

            if (parentExpression.Id == expressionId)
            {
                return null;
            }

            foreach (var expression in expressionById.Values)
            {
                if (expression.LeftChild != null && expression.LeftChild.Id == expressionId)
                {
                    return expression;
                }

                if (expression.RightChild != null && expression.RightChild.Id == expressionId)
                {
                    return expression;
                }
            }

            return null;
        }
    }
}