using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Nustache.Core.Tests
{
    [TestFixture]
    public class JObjectTrees
    {
        public static Guid TestGuid = new Guid("7F3CD8AF-91D3-4664-A272-96CA082C26C3");
        [Test]

        public void Flat_lists_must_be_transformed_into_trees()
        {
            var list = GetNodeList();
            var tree = new AllTrees() {List = new List<TreeNode>(GetNodeTree(list))};         

            var template =
@"{{<node}} 
    <li>{{Description}}{{height}}{{rings}}
        {{#HasChildren}}
            <ul>
                {{#Children}}
                    {{>node}}
                {{/Children}}
            </ul>
        {{/HasChildren}}
    </li>
{{/node}}

<ul>
{{#list}}
    {{#.}}
        {{>node}}
    {{/.}}
{{/list}}
</ul>
";

            var result = Render.StringToString(template, this.DryPuff(tree));

            Assert.AreEqual(
@"<ul>
    <li>A1.17f3cd8af-91d3-4664-a272-96ca082c26c3</li>
    <li>B2.27f3cd8af-91d3-4664-a272-96ca082c26c3
        <ul>
            <li>C7f3cd8af-91d3-4664-a272-96ca082c26c3
                <ul>
                    <li>D7f3cd8af-91d3-4664-a272-96ca082c26c3</li>
                </ul>
            </li>
            <li>E7f3cd8af-91d3-4664-a272-96ca082c26c3</li>
        </ul>
    </li>
    <li>F7f3cd8af-91d3-4664-a272-96ca082c26c3</li>
</ul>".RemoveWhitespace(), result.RemoveWhitespace());
        }

        public object DryPuff(object anObject)
        {

            var settings = new JsonSerializerSettings()
            {

                Formatting = Newtonsoft.Json.Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };

            var dry = Newtonsoft.Json.JsonConvert.SerializeObject(anObject, settings);

            return JsonConvert.DeserializeObject(dry);
        }

        public class AllTrees
        {
            public List<TreeNode> List { get; set; }
        }
        public class ListNode
        {
            public float Height { get; set; }
            public int Id { get; set; }
            public string Description { get; set; }
            public int ParentId { get; set; }
        }

        private IEnumerable<ListNode> GetNodeList()
        {
            return new[]
                   {
                       new ListNode {Height =1.1f, Id = 1, Description = "A" },
                       new ListNode {Height =2.2f, Id = 2, Description = "B" },
                       new ListNode { Id = 3, Description = "C", ParentId = 2 },
                       new ListNode { Id = 4, Description = "D", ParentId = 3 },
                       new ListNode { Id = 5, Description = "E", ParentId = 2 },
                       new ListNode { Id = 6, Description = "F" },
                   };
        }

        public class TreeNode
        {
            public Guid Rings { get; set; }
            public float Height { get; set; }
            public int Id { get; set; }
            public string Description { get; set; }
            public int ParentId { get; set; }
            public IEnumerable<TreeNode> Children { get; set; }
            public bool HasChildren { get { return Children.Any(); } }
        }

        private IEnumerable<TreeNode> GetNodeTree(IEnumerable<ListNode> listNodes)
        {
            var treeNodes = listNodes.Select(n => new TreeNode
                                                  {
                                                      Rings = TestGuid,
                                                      Height = n.Height,
                                                      Id = n.Id,
                                                      Description = n.Description,
                                                      ParentId = n.ParentId
                                                  })
                                     .ToArray();

            var lookup = treeNodes.ToLookup(n => n.ParentId);

            foreach (var treeNode in treeNodes)
            {
                treeNode.Children = lookup[treeNode.Id].ToArray();
            }

            return lookup[0].ToArray();
        }
    }
}