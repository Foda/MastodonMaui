using DynamicData;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml.Linq;

namespace MastodonMaui.Controls
{
    public class StatusTextControl : ContentView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text", typeof(string), typeof(StatusTextControl), propertyChanged: TextChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StatusTextControl control = bindable as StatusTextControl;
            if (control != null)
            {
                control.RebuildLabel();
            }
        }

        public ICommand UrlNavigateCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        private void RebuildLabel()
        {
            var modifiedText = string.Format("<div>{0}</div>", Text);
            modifiedText = Regex.Replace(modifiedText, "<br>", "<br></br>", RegexOptions.IgnoreCase);

            XElement element = XElement.Parse(modifiedText);

            List<Span> inlines = new();
            ParseText(element, inlines, this.UrlNavigateCommand);

            FormattedString formattedString = new();
            formattedString.Spans.AddRange(inlines);

            Label label = new()
            {
                FormattedText = formattedString
            };
            this.Content = label;
        }

        public static void ParseText(XElement? element, IList<Span> inlines, ICommand urlNavigateCommand)
        {
            var currentInlines = inlines;
            var elementName = element.Name.ToString().ToUpper();

            if (elementName == "A")
            {
                Span s = new()
                {
                    Text = element.Value + " ",
                    TextColor = Colors.MediumPurple
                };

                if (element.Attribute("href") != null)
                {
                    var onTap = new TapGestureRecognizer()
                    {
                        Command = urlNavigateCommand,
                        CommandParameter = element.Attribute("href").Value
                    };
                    s.GestureRecognizers.Add(onTap);
                }
                currentInlines.Add(s);
            }
            else
            {
                foreach (var node in element.Nodes())
                {
                    if (node is XText textElement)
                    {
                        Span s = new()
                        {
                            Text = textElement.Value
                        };
                        currentInlines.Add(s);
                    }
                    else
                    {
                        ParseText(node as XElement, currentInlines, urlNavigateCommand);
                    }
                }
            }
        }
    }
}