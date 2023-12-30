using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DijkstraCoffeeAndCode.Views
{
    /// <summary>
    /// Interaction logic for NodeProperties.xaml
    /// </summary>
    public partial class NodeProperties : UserControl
    {
        public NodeProperties()
        {
            InitializeComponent();
        }

        private void UserControlKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                DependencyObject ancestor = Parent;
                while (ancestor != null)
                {
                    var element = ancestor as UIElement;
                    if (element != null && element.Focusable)
                    {
                        element.Focus();
                        break;
                    }

                    ancestor = VisualTreeHelper.GetParent(ancestor);
                }
            }
        }
    }
}
