// WunderVision 2023
// https://www.wundervisionengineering.com/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.Views
{
    public partial class NodeProperties : UserControl
    {
        public NodeProperties()
        {
            InitializeComponent();
        }

        private void UserControlKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (sender is TextBox textBox))
            {
                // An example of how to explicity update the Text binding of a text box
                //DependencyProperty prop = TextBox.TextProperty;
                //BindingExpression binding = BindingOperations.GetBindingExpression(textBox, prop);
                //if (binding != null) { binding.UpdateSource(); }

                // This clears the focus on the Text Box.. however, it doesn't 
                // Clear the IsFocused property until a different control is focused.
                Keyboard.ClearFocus();
                Window window = Window.GetWindow(textBox);
                FocusManager.SetFocusedElement(window, window);
            }
        }
    }
}
