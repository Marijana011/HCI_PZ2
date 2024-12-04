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

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for NetworkServiceData.xaml
    /// </summary>
    public partial class NetworkEntitiesView : UserControl
    {
        public NetworkEntitiesView()
        {
            InitializeComponent();
            this.DataContext = new NetworkService.ViewModel.NetworkEntitiesViewModel();
        }

        private void Keyboard_input(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            VirtualKeyboard keyboard = new VirtualKeyboard(textbox, Window.GetWindow(this));
            if (keyboard.ShowDialog() == true)
            {
                textbox.Text = keyboard.Result;
            }
        }
    }
}
