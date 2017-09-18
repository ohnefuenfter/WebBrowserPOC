using System.Windows;
using System.Windows.Navigation;
using mshtml;

namespace WebBrowserPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string style = ".btn-group .button {" +
                                     "background-color: white; " +
                                     "border: 1px solid gray; " +
                                     "color: black; " +
                                     "padding: 4px 8px; " +
                                     "    text-align: center; " +
                                     "    text-decoration: none; " +
                                     "display: inline-block; " +
                                     "    font-size: 12px; " +
                                     "cursor: pointer; " +
                                     "     width: 7%" +
                                     "}" +
                                     ".btn-group .button:not(:last-child) { " +
                                     "border-right: none; /* Prevent double borders */ " +
                                     "}" +
                                     ".btn-group .button:hover { " +
                                     "background-color: #e7e7e7; " +
                                     "}";
        public MainWindow()
        {
            InitializeComponent();

            Browser.NavigateToString("<html><head></head><body> " +
                                     "<p>Click the 'Try it' button to toggle between hiding and showing the DIV element:</p> " +
                                     "<div class='btn-group'><hr style='display:inline-block; width:85%;' /><button class='button' id='toggleVis1' onclick = \"toggleVisibility('myDIV1')\">&#x2796;</button><button class='button' id='toggleWeb1' onclick = \"toggleWebVisibility('myDIV1')\">&#x2713;</button></div> " +
                                     "<p style='clear:both'><div style='background-color:white' id = 'myDIV1'>This is my DIV 1 element.</div></p>" +
                                     "<div class='btn-group'><hr style='display:inline-block; width:85%;' /><button class='button' id='toggleVis2' onclick = \"toggleVisibility('myDIV2')\">&#x2795;</button><button class='button' id='toggleWeb2' onclick = \"toggleWebVisibility('myDIV2')\">&#x2717;</button></div>" +
                                     "<p style='clear:both'><div style='display:none;background-color:#e7e7e7' id = 'myDIV2'>This is my DIV 2 element.</div></p>" +
                                     "<p><b>Note:</b> The element will not take up any space when the display property set to 'none'.</p> " +
                                     "<script> " +
                                     "function toggleVisibility(elementId) " +
                                     "{" +
                                     "var x = document.getElementById(elementId); " +
                                     "if (x.style.display === 'none') " +
                                     "{" +
                                     "    x.style.display = 'block'; " +
                                     "}" +
                                     "else " +
                                     "{" +
                                     "    x.style.display = 'none'; " +
                                     "}" +
                                     "}" +
                                     "function toggleWebVisibility(elementId) " +
                                     "{" +
                                     "var x = document.getElementById(elementId); " +
                                     "if (x.style.backgroundColor === 'white') " +
                                     "{" +
                                     "    x.style.backgroundColor = '#e7e7e7'; " +
                                     "}" +
                                     "else " +
                                     "{" +
                                     "    x.style.backgroundColor = 'white'; " +
                                     "}" +
                                     "}" +
                                     "</script > </body></html>");
            Browser.LoadCompleted += BrowserOnLoadCompleted;
        }

        private readonly bool[] state = {
            true,
            false
        };

        private readonly bool[] webstate = {
            true,
            false
        };

        private void BrowserOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            var mshtmlDoc = Browser.Document as HTMLDocument;
            if (mshtmlDoc == null) return;
            foreach (var lnk in mshtmlDoc.getElementsByTagName("BUTTON"))
            {
                var clickable = lnk as HTMLButtonElement;
                var events = clickable as HTMLButtonElementEvents2_Event;
                if (events == null) continue;                
                events.onclick += Link2Clicked;
            }
            var doc2 = mshtmlDoc as IHTMLDocument2;
            if (doc2 == null) return;
            var ss = doc2.createStyleSheet("", 0);
            ss.cssText = style;
        }

        private bool Link2Clicked(IHTMLEventObj pevtobj)
        {
            var srcElement = pevtobj.srcElement;
            if (srcElement.id.StartsWith("toggleVis"))
            {
                var index = srcElement.id == "toggleVis1" ? 0 : 1;
                srcElement.innerText = state[index]
                    ? string.Format("{0}", (char) 0x2795)
                    : string.Format("{0}", (char) 0x2796);
                state[index] = !state[index];
            }
            else if (srcElement.id.StartsWith("toggleWeb"))
            {
                var index = srcElement.id == "toggleWeb1" ? 0 : 1;
                srcElement.innerText = webstate[index]
                    ? string.Format("{0}", (char)0x2717)
                    : string.Format("{0}", (char)0x2713);
                webstate[index] = !webstate[index];
            }
            return true;
        }

    }
}
