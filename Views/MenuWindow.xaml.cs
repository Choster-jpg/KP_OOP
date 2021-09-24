using KP_OOP.ViewModels;
using System.Windows;


namespace KP_OOP
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            DataContext = new MenuW_VM(this);
        }

        private void Button_ClickCloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
