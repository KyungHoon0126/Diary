using Microsoft.Win32;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Making_A_Diary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string file = "";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_OpenDiary_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            {
                openFile.Filter = "txt File | *.txt";
                openFile.CheckFileExists = true;
                openFile.CheckPathExists = true;

                if(openFile.ShowDialog().GetValueOrDefault())
                {
                    file = openFile.FileName;
                    this.Title = file;

                    using (StreamReader sr = new StreamReader(file))
                    {
                        tbDiaryContent.Text = sr.ReadToEnd();
                    }
                }
            }
        }

        private void btn_WriteDiary_Click(object sender, RoutedEventArgs e)
        {
            WriteDiaryWindow writeDiaryWindow = new WriteDiaryWindow();
            writeDiaryWindow.ShowDialog();
        }
    }
}
