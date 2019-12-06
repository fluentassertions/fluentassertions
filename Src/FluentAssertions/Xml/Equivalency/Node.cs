using System.Collections.Generic;

namespace FluentAssertions.Xml.Equivalency
{
    internal sealed class Node
    {
        private readonly Node parent;
        private readonly List<Node> children = new List<Node>();

        public string Name { get; }
        public int Count { get; private set; }

        private Node(Node parent, string name)
        {
            this.parent = parent;
            Name = name;
        }

        public static Node CreateRoot() => new Node(null, null);

        public IEnumerable<Node> GetPath()
        {
            Node current = this;
            while (current.parent is object)
            {
                yield return current;
                current = current.parent;
            }
        }

        public Node Pop() => parent;

        public Node Push(string name)
        {
            Node node = children.Find(e => e.Name == name)
                        ?? AddChildNode(name);

            node.Count++;

            return node;
        }

        private Node AddChildNode(string name)
        {
            Node node = new Node(this, name);
            children.Add(node);
            return node;
        }
    }
}
