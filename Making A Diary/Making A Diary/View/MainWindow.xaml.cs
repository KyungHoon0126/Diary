using Making_A_Diary.Common;
using Making_A_Diary.Model;
using Making_A_Diary.ViewModel;
using Microsoft.Win32;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace Making_A_Diary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window window;
        ICollectionView view = CollectionViewSource.GetDefaultView(App.diaryViewModel.DiaryItems);

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
            App.diaryViewModel.GetDiaryList();
            view.Refresh();
        }

        private void btn_WriteDiary_Click(object sender, RoutedEventArgs e)
        {
            WriteDiaryWindow writeDiaryWindow = new WriteDiaryWindow();
            App.diaryViewModel.WriteDiaryContent = null;
            window = writeDiaryWindow;
            writeDiaryWindow.ShowDialog();
        }

        private void lvDiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvDiaryList.SelectedItem != null)
            {
                try
                {
                    string path = "C:\\diaryFolder\\" + (lvDiaryList.SelectedItem as Diary).DiaryTitle + ".txt";
                    tbDiaryContent.Text = File.ReadAllText(path);
                    // App.diaryViewModel.OpenDiaryContent = File.ReadAllText(path); // 바인딩하는 OpenDiaryContent에 바로 넣으려고 했으나 값이 보이지 않음.
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                }
            }
            else
            {
                return;
            }
        }

        private void btn_DiaryDelete_Click(object sender, RoutedEventArgs e)
        {
            if(lvDiaryList.SelectedItem == null)
            {
                MessageBox.Show("삭제할 목록을 선택해 주세요!");
            }
            else
            {
                try
                {
                    string path = "C:\\diaryFolder\\" + (lvDiaryList.SelectedItem as Diary).DiaryTitle + ".txt";

                    if (MessageBox.Show("정말 삭제하시겠습니까?", (lvDiaryList.SelectedItem as Diary).DiaryTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        File.Delete(path);
                        App.diaryViewModel.GetDiaryList();
                        view.Refresh();
                    }
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                }
            }
        }

        // TODO : 파일 열기로 내용 출력시 값은 들어오나 바인딩이 안됨.
        // TODO : 파일 목록에서 마지막 하나의 목록이 보이지 않음.
    }
}
