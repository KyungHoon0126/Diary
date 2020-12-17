using SimpleDiary.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace SimpleDiary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Property
        private List<DiaryData> _diaryItems;
        public List<DiaryData> DiaryItems
        {
            get => _diaryItems;
            set
            {
                _diaryItems = value;
                NotifyPropertyChanged(nameof(DiaryItems));
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            ctrlWriteDiary.btnMoveToHome.Click += (s, e) =>
            {
                ctrlWriteDiary.ClearPostData();
                ctrlWriteDiary.Visibility = Visibility.Collapsed;
                gdMain.Visibility = Visibility.Visible;
            };
            ctrlWriteDiary.OnDiaryPost += CtrlWriteDiary_OnDiaryPost;
        }

        private void CtrlWriteDiary_OnDiaryPost()
        {
            ctrlWriteDiary.Visibility = Visibility.Collapsed;
            gdMain.Visibility = Visibility.Visible;
            SetDiaryItems();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.dBManager.CreateDataBase();
            SetDiaryItems();
            this.DataContext = this;
        }

        private void btnWriteDiary_Click(object sender, RoutedEventArgs e)
        {
            gdMain.Visibility = Visibility.Collapsed;
            ctrlWriteDiary.Visibility = Visibility.Visible;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnDeleteDiary_Click(object sender, RoutedEventArgs e)
        {
            if (lvDiaryList.SelectedItem == null)
            {
                MessageBox.Show("삭제할 일기를 선택해 주세요!");
                return;
            }
            App.dBManager.DeleteDiary(lvDiaryList.SelectedItem as DiaryData);
            SetDiaryItems();
        }

        private void SetDiaryItems()
        {
            if (DiaryItems != null)
            {
                DiaryItems.Clear();
            }
            DiaryItems = new List<DiaryData>(App.dBManager.GetDiaryDatas());
        }
    }
}
