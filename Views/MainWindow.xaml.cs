using KP_OOP.ViewModels;
using System;
using System.Windows;


namespace KP_OOP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MW_VM(this);
        }
    }
}
