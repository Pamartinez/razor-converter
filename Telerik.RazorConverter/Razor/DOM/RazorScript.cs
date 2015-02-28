namespace Telerik.RazorConverter.Razor.DOM
{
    public class RazorScript : RazorNode, IRazorScriptNode
    {
        public string Text { get; set; }

        public RazorScript()
        {
        }

        public RazorScript(string text)
        {
            Text = text;
        }
    }
}
