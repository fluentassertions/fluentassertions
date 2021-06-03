using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FluentAssertions.Xml.Equivalency
{
    internal sealed class Node
    {
        private readonly List<Node> children = new();
        private readonly string name;
        private int count;

        public static Node CreateRoot() => new Node(null, null);

        private Node(Node parent, string name)
        {
            Parent = parent;
            this.name = name;
        }

        public string GetXPath()
        {
            var resultBuilder = new StringBuilder();

            foreach (Node location in GetPath().Reverse())
            {
                if (location.count > 1)
                {
                    resultBuilder.AppendFormat(CultureInfo.InvariantCulture, "/{0}[{1}]", location.name, location.count);
                }
                else
                {
                    resultBuilder.AppendFormat(CultureInfo.InvariantCulture, "/{0}", location.name);
                }
            }

            if (resultBuilder.Length == 0)
            {
                return "/";
            }

            return resultBuilder.ToString();
        }

        private IEnumerable<Node> GetPath()
        {
            Node current = this;
            while (current.Parent is not null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public Node Parent { get; }

        public Node Push(string localName)
        {
            Node node = children.Find(e => e.name == localName)
                        ?? AddChildNode(localName);

            node.count++;

            return node;
        }

        public void Pop()
        {
            children.Clear();
        }

        private Node AddChildNode(string name)
        {
            var node = new Node(this, name);
            children.Add(node);
            return node;
        }
    }
}
