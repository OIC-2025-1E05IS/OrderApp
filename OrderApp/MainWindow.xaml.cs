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

namespace OrderApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //a
        private void Ramen_btn(object sender, RoutedEventArgs e)
        {
            var window = new Menu(MenuCategory.Ramen);
            window.Owner = this;
            this.Hide();
            window.Show();
        
        }

        private void Sirunasi_btn(object sender, RoutedEventArgs e)
        {
            var window = new Menu(MenuCategory.Soupless);
            window.Owner = this;
            this.Hide();
            window.Show();
        }

        private void Tsukemen_btn(object sender, RoutedEventArgs e)
        {
            var window = new Menu(MenuCategory.Tsukemen);
            window.Owner = this;
            this.Hide();
            window.Show();
       
        }

        private void Ramen_img_click(object sender, MouseButtonEventArgs e)
        {
            Ramen_btn(sender, e);
        }

        private void sirunasi_img_click(object sender, MouseButtonEventArgs e)
        {
            Sirunasi_btn(sender, e);
        }

        private void tukemen_img_click(object sender, MouseButtonEventArgs e)
        {
            Tsukemen_btn(sender, e);
        }
    }
}
