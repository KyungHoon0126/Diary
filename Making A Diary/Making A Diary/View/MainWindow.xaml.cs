using Making_A_Diary.Common;
using Making_A_Diary.ViewModel;
using Microsoft.Win32;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Window window;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.diaryViewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.diaryViewModel.IsDirectoryExsist();
            App.diaryViewModel.GetDiaryList();
            App.diaryViewModel.OnWriteDiaryResultReceived += DiaryViewModel_OnWriteDiaryResultReceived;
        }

        private void DiaryViewModel_OnWriteDiaryResultReceived()
        {
            window.Close();
        }

        private void btn_WriteDiary_Click(object sender, RoutedEventArgs e)
        {
            WriteDiaryWindow writeDiaryWindow = new WriteDiaryWindow();
            window = writeDiaryWindow;
            writeDiaryWindow.ShowDialog();
        }

        private void lvDiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string path = "C:\\diaryFolder\\" + (lvDiaryList.SelectedItem as Diary).DiaryTitle + ".txt";
                tbDiaryContent.Text = File.ReadAllText(path);
                // App.diaryViewModel.OpenDiaryContent = File.ReadAllText(path);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        // TOOD : 파일 목록 나타낼 시 하나가 안나옴.
        // TODO : 파일 열기로 내용 출력시 값은 들어오나 바인딩이 안됨.
    }
}
