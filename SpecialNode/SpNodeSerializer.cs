using System.Collections.Generic;
using System.Text;

namespace SpecialNodeLib
{
    public class SpNode
    {
        public int? Value { get; set; }
        public SpNode[] Values { get; set; }
    }

    public static class SpNodeSerializer
    {
        public static string Serialize(SpNode node)
        {
            var sb = new StringBuilder();
            Serialize(node, sb);
            return sb.ToString();
        }

        private static void Serialize(SpNode node, StringBuilder sb)
        {
            if (node.Value.HasValue)
            {
                sb.Append(node.Value.ToString());
                return;
            }

            sb.Append('[');
            for (int i = 0; i < node.Values.Length - 1; i++)
            {
                Serialize(node.Values[i], sb);
                sb.Append(',');
            }

            Serialize(node.Values[^1], sb);
            sb.Append(']');
        }

        public static SpNode Deserialize(string s)
        {
            var node = new SpNode();
            var index = 0;
            Deserialize(node, s, ref index);
            return node;
        }

        public static void Deserialize(SpNode node, string s, ref int index)
        {
            if (s[index] == '[')
            {
                node.Values = DeserializeValues(s, ref index);
            }
            else
            {
                node.Value = DeserializeValue(s, ref index);
            }
        }

        private static SpNode[] DeserializeValues(string s, ref int index)
        {
            var values = new List<SpNode>();

            index++;
            while (index < s.Length && s[index] != ']')
            {
                var node = new SpNode();
                values.Add(node);
                Deserialize(node, s, ref index);

                if (s[index] == ',')
                {
                    index++;
                }
            }

            index++;
            return values.ToArray();
        }

        private static int DeserializeValue(string s, ref int index)
        {
            var start = index;
            // read value until end, comma, or closing bracket
            while (index < s.Length && s[index] != ',' && s[index] != ']')
            {
                index++;
            }

            return int.Parse(s[start..index]);
        }
    }
}
