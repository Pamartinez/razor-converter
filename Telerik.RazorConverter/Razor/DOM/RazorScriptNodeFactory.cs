using System.ComponentModel.Composition;

namespace Telerik.RazorConverter.Razor.DOM
{
    [Export(typeof (IRazorScriptFactory))]
    public class RazorScriptNodeFactory : IRazorScriptFactory
    {
        public IRazorScriptNode CreateTextNode(string text)
        {
            return new RazorScript {Text = text};
        }
    }
}
