namespace Telerik.RazorConverter.WebForms.DOM
{
    public class ScriptNode : WebFormsNode, IWebFormsScriptNode
    {
        public string Text { get; set; }

        string IWebFormsContentNode.Content
        {
            get { return Text; }

            set { Text = value; }
        }

        public ScriptNode()
        {
            Type = NodeType.Script;
        }
    }
}
