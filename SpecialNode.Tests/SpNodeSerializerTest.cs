using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecialNodeLib;

namespace SpecialNode.Tests
{
    [TestClass]
    public class SpNodeSerializerTest
    {
        public TestContext TestContext { get; set; }

        #region Serialize

        [TestMethod]
        // 1
        public void Serialize_Given_a_single_value_Should_return_the_value()
        {
            var node = new SpNode { Value = 1 };

            var actual = SpNodeSerializer.Serialize(node);

            Assert.AreEqual("1", actual);
        }

        [TestMethod]
        // [1]
        public void Serialize_Given_values_with_one_value_Should_return_the_value_in_brakets()
        {
            var node = new SpNode { Value = 1 };
            node = new SpNode
            {
                Values = new SpNode[] { node }
            };

            var actual = SpNodeSerializer.Serialize(node);

            Assert.AreEqual("[1]", actual);
        }

        [TestMethod]
        // [1,2]
        public void Serialize_Given_values_with_two_values_Should_return_the_values_in_brakets()
        {
            var node1 = new SpNode { Value = 1 };
            var node2 = new SpNode { Value = 2 };
            var node = new SpNode { Values = new SpNode[] { node1, node2 } };

            var actual = SpNodeSerializer.Serialize(node);

            Assert.AreEqual("[1,2]", actual);
        }

        [TestMethod]
        // [1,[2,3]]
        public void Serialize_Complex_scenario_1()
        {
            var node1 = new SpNode { Value = 1 };
            var node2 = new SpNode { Value = 2 };
            var node3 = new SpNode { Value = 3 };
            var node = new SpNode
            {
                Values = new SpNode[]
                {
                    node1,
                    new SpNode { Values = new SpNode[] { node2, node3 } }
                }
            };

            var actual = SpNodeSerializer.Serialize(node);

            Assert.AreEqual("[1,[2,3]]", actual);
        }

        [TestMethod]
        // [1,[2,3],[4,5]]
        public void Serialize_Complex_scenario_2()
        {
            var node1 = new SpNode { Value = 1 };
            var node2 = new SpNode { Value = 2 };
            var node3 = new SpNode { Value = 3 };
            var node4 = new SpNode { Value = 4 };
            var node5 = new SpNode { Value = 5 };
            var node = new SpNode
            {
                Values = new SpNode[]
                {
                    node1,
                    new SpNode { Values = new SpNode[] { node2, node3 } },
                    new SpNode { Values = new SpNode[] { node4, node5 } }
                }
            };

            var actual = SpNodeSerializer.Serialize(node);

            Assert.AreEqual("[1,[2,3],[4,5]]", actual);
        }

        [TestMethod]
        // [[1]]
        public void Serialize_Complex_scenario_3()
        {
            var node1 = new SpNode { Value = 1 };
            var node = new SpNode
            {
                Values = new SpNode[]
                {
                    new SpNode { Values = new SpNode[] { node1 } }
                }
            };

            var actual = SpNodeSerializer.Serialize(node);

            Assert.AreEqual("[[1]]", actual);
        }

        #endregion

        [TestMethod]
        [DataRow("1", 1)]
        [DataRow("100", 100)]
        public void Deserialize_Scenario_1(string input, int expected)
        {
            var actual = SpNodeSerializer.Deserialize(input);
            Print(actual);
            Assert.AreEqual(input, SpNodeSerializer.Serialize(actual));

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.Value);
            Assert.IsNull(actual.Values);
        }

        [TestMethod]
        // [1]
        public void Deserialize_Scenario_2()
        {
            var input = "[1]";
            var actual = SpNodeSerializer.Deserialize(input);
            Print(actual);
            Assert.AreEqual(input, SpNodeSerializer.Serialize(actual));

            Assert.IsNotNull(actual);

            Assert.IsNull(actual.Value);

            Assert.IsNotNull(actual.Values);
            Assert.AreEqual(1, actual.Values.Length);
            Assert.AreEqual(1, actual.Values[0].Value);
            Assert.IsNull(actual.Values[0].Values);
        }

        [TestMethod]
        // [[1]]
        public void Deserialize_Scenario_3()
        {
            var input = "[[1]]";
            var actual = SpNodeSerializer.Deserialize(input);
            Print(actual);
            Assert.AreEqual(input, SpNodeSerializer.Serialize(actual));

            Assert.IsNotNull(actual);

            Assert.IsNull(actual.Value);

            Assert.IsNotNull(actual.Values);
            Assert.AreEqual(1, actual.Values.Length);

            Assert.IsNull(actual.Values[0].Value);
            Assert.AreEqual(1, actual.Values[0].Values.Length);
            Assert.AreEqual(1, actual.Values[0].Values[0].Value);
        }

        [TestMethod]
        // [1,2]
        public void Deserialize_Scenario_4()
        {
            var input = "[1,2]";
            var actual = SpNodeSerializer.Deserialize(input);
            Print(actual);
            Assert.AreEqual(input, SpNodeSerializer.Serialize(actual));

            Assert.IsNotNull(actual);

            Assert.IsNull(actual.Value);

            Assert.IsNotNull(actual.Values);
            Assert.AreEqual(2, actual.Values.Length);

            Assert.AreEqual(1, actual.Values[0].Value);
            Assert.IsNull(actual.Values[0].Values);

            Assert.AreEqual(2, actual.Values[1].Value);
            Assert.IsNull(actual.Values[1].Values);
        }

        [TestMethod]
        // [[1],2]
        public void Deserialize_Scenario_5()
        {
            var input = "[[1],2]";
            var actual = SpNodeSerializer.Deserialize(input);
            Print(actual);
            Assert.AreEqual(input, SpNodeSerializer.Serialize(actual));

            Assert.IsNotNull(actual);

            Assert.IsNull(actual.Value);
            Assert.IsNotNull(actual.Values);
            Assert.AreEqual(2, actual.Values.Length);
        }

        [TestMethod]
        [DataRow("[[1],[2]]")]
        [DataRow("[1,[2,3]]")]
        [DataRow("[1,[2,3],[4,5]]")]
        public void Deserialize_Other_scenarios(string input)
        {
            var actual = SpNodeSerializer.Deserialize(input);
            Print(actual);
            Assert.AreEqual(input, SpNodeSerializer.Serialize(actual));
        }

        private void Print(SpNode node)
        {
            TestContext.WriteLine(SpNodeSerializer.Serialize(node));
        }
    }
}
