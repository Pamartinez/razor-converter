using System.Collections.Generic;
using Telerik.RazorConverter.Razor.DOM;
using Telerik.RazorConverter.WebForms.DOM;

namespace Telerik.RazorConverter.Razor.Converters
{
    public class ScriptConverter : INodeConverter<IRazorNode>
    {
        private IRazorScriptFactory TextNodeFactory { get; set; }

        public ScriptConverter(IRazorScriptFactory nodeFactory)
        {
            TextNodeFactory = nodeFactory;
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var srcNode = node as IWebFormsScriptNode;
            var destNode = TextNodeFactory.CreateTextNode(srcNode.Text);
            return new IRazorNode[] {destNode};
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            return node is IWebFormsScriptNode;
        }
    }
}
