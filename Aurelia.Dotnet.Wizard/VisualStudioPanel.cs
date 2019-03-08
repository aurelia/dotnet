using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aurelia.DotNet.Wizard
{
    public class VisualStudioPanel : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(VisualStudioPanel), new UIPropertyMetadata(""));

        public VisualStudioPanel()
        {
            Initialize();
        }

        private void Initialize()
        {
            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("/Aurelia.DotNet.Wizard;component/VisualStudioPanel.xaml", UriKind.RelativeOrAbsolute);
            this.Resources = resources;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
