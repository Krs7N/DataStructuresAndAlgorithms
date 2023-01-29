using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Categorization
{
    public class Categorizator : ICategorizator
    {
        private class Node
        {
            public Category Value { get; set; }
            public Node Parent { get; set; }
            public HashSet<Node> Children { get; set; }

            public Node(Category value)
            {
                this.Value = value;
                Children = new HashSet<Node>();
            }
        }

        private Node root;
        private Dictionary<Category, Node> nodesByCategories;
        private Dictionary<string, Category> categoriesById;

        public Categorizator()
        {
            this.nodesByCategories = new Dictionary<Category, Node>();
            this.categoriesById = new Dictionary<string, Category>();
        }

        public void AddCategory(Category category)
        {
            if (categoriesById.ContainsKey(category.Id) || nodesByCategories.ContainsKey(category))
            {
                throw new ArgumentException();
            }

            categoriesById.Add(category.Id, category);

            if (nodesByCategories.Count == 0)
            {
                root = new Node(category);
                nodesByCategories.Add(category, root);
            }
            else
            {
                var node = new Node(category);
                nodesByCategories.Add(category, node);
            }

        }

        public void AssignParent(string childCategoryId, string parentCategoryId)
        {
            if (!categoriesById.ContainsKey(childCategoryId) || !categoriesById.ContainsKey(parentCategoryId) || nodesByCategories[categoriesById[parentCategoryId]].Children.Any(c => c == nodesByCategories[categoriesById[childCategoryId]]))
            {
                throw new ArgumentException();
            }

            var node = nodesByCategories[categoriesById[childCategoryId]];
            node.Parent = nodesByCategories[categoriesById[parentCategoryId]];

            nodesByCategories[categoriesById[parentCategoryId]].Children.Add(node);
        }

        public bool Contains(Category category) => categoriesById.ContainsKey(category.Id);

        public IEnumerable<Category> GetChildren(string categoryId)
        {
            if (!categoriesById.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            var children = new HashSet<Node>();
            var queue = new Queue<Node>();
            var startingNode = nodesByCategories[categoriesById[categoryId]];
            queue.Enqueue(startingNode);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                foreach (var child in currentNode.Children.Where(child => !children.Contains(child)))
                {
                    queue.Enqueue(child);
                    children.Add(child);
                }
            }

            return children.Select(n => n.Value);
        }

        public IEnumerable<Category> GetHierarchy(string categoryId)
        {
            if (!categoriesById.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            var hierarchy = new List<Category>();
            var node = nodesByCategories[categoriesById[categoryId]];

            GetHierarchyRecursive(node, hierarchy);
            hierarchy.Reverse();

            return hierarchy;
        }

        private void GetHierarchyRecursive(Node curNode, List<Category> hierarchy)
        {
            if (curNode == null)
            {
                return;
            }

            hierarchy.Add(curNode.Value);
            GetHierarchyRecursive(curNode.Parent, hierarchy);
        }

        public IEnumerable<Category> GetTop3CategoriesOrderedByDepthOfChildrenThenByName() => nodesByCategories.Values
            .OrderByDescending(n => n.Children.Max(GetDepth))
            .ThenBy(n => n.Value.Name)
            .Take(3)
            .Select(n => n.Value);

        private int GetDepth(Node node)
        {
            int depth = 0;

            while (node.Parent != null)
            {
                depth++;
                node = node.Parent;
            }

            return depth;
        }

        public void RemoveCategory(string categoryId)
        {
            if (!categoriesById.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            categoriesById.Remove(categoryId);

            var node = nodesByCategories[categoriesById[categoryId]];
            var parentNode = node.Parent;

            parentNode.Children.Remove(node);

            foreach (var child in node.Children)
            {
                parentNode.Children.Add(child);
            }

            foreach (var child in node.Children)
            {
                this.nodesByCategories[child.Value].Parent = parentNode;
            }

            nodesByCategories.Remove(categoriesById[categoryId]);
        }

        public int Size() => categoriesById.Count;
    }
}