using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.RazorConverter.Razor.DOM;

namespace Telerik.RazorConverter.Razor.Rendering
{
    public class ScriptRenderer : IRazorNodeRenderer
    {
        public string RenderNode(IRazorNode node)
        {
            var textNode = node as IRazorScriptNode;
            return ConvertScript(textNode.Text);
        }

        public bool CanRenderNode(IRazorNode node)
        {
            return node is IRazorScriptNode;
        }


        private string ConvertScript(string aspxCode)
        {
            string text2 = aspxCode;


            //expressions 
            string text4 = Regex.Replace(text2, @"<%[=:](?<expr>.*?)%>", m =>
            {
                var expr = m.Groups["expr"].Value.Trim();
                var cleanExpr = Regex.Replace(expr,
                    @"(""(\\""|[^""])*"")|(@""([^""]|"""")*"")|(\([^\(\)]*(((?'Open'\()[^\(\)]*)+((?'Close-Open'\))[^\(\)]*)+)*\))",
                    m2 => "");
                return cleanExpr.Contains(' ') ? "@(" + expr + ")" : "@" + expr;
            }, RegexOptions.Singleline);

            //code blocks
            var text5 = Regex.Replace(text4, @"<%(?<code>.*?)%>", m =>
            {
                var code = m.Groups["code"].Value.Trim();

                var stringLiterals = new Dictionary<string, string>();

                code = Regex.Replace(code, @"(""(\\""|[^""])*"")|(@""([^""]|"""")*"")", m2 =>
                {
                    string key = "<$" + stringLiterals.Count + "$>";
                    stringLiterals.Add(key, m2.Value);
                    return key;
                });

                var result = Regex.Replace(code,
                    @"((?<blockHeader>(else|(for|switch|foreach|using|while|if)\s*\([^\(\)]*(((?'Open'\()[^\(\)]*)+((?'Close-Open'\))[^\(\)]*)+)*\))\s*)" +
                    @"((?<fullBlock>{[^{}]*(((?'OpenCurly'{)[^{}]*)+((?'CloseCurly-OpenCurly'})[^{}]*)+)*})|(?<openblock>{.*))|" +
                    @"(?<text>((?!({|}|\s)(for|switch|foreach|using|while|if|else)(\s|{|\()).)+))",
                    m2 =>
                    {
                        if (m2.Value.Trim().Length == 0 || m2.Value.StartsWith("else") || m2.Value.StartsWith("}"))
                            return m2.Value;

                        if (m2.Groups["text"].Success)
                            return "@{ " + m2.Value.Trim() + "}\r\n";

                        return "@" + m2.Value;
                    }, RegexOptions.ExplicitCapture | RegexOptions.Singleline);

                result = Regex.Replace(result, @"<\$\d+\$>",
                    m2 => stringLiterals[m2.Value]);

                return result;
            }, RegexOptions.Singleline);

            return text5;
        }
    }
}
