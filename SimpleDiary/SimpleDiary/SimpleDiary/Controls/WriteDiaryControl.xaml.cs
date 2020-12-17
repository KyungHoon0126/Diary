using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDiary.Controls
{
    /// <summary>
    /// Interaction logic for WriteDiaryControl.xaml
    /// </summary>
    public partial class WriteDiaryControl : UserControl
    {
        public delegate void OnWriteDiaryPostHandler();
        public event OnWriteDiaryPostHandler OnDiaryPost;
        public WriteDiaryControl()
        {
            InitializeComponent();
            btnWritePost.IsEnabled = false;
        }

        private void btnWritePost_Click(object sender, RoutedEventArgs e)
        {
            var title = tbTitle.Text;
            var content = tbContent.Text;
            DateTime? writeDate = dpDate.SelectedDate;
            
            if (writeDate.Value != null && title != null && content != null)
            {
                btnWritePost.IsEnabled = true;
                App.dBManager.SaveDiary(title, content, writeDate.Value.ToString("yyyy-MM-dd"));
                ClearPostData();
                OnDiaryPost?.Invoke();
            }
        }

        private void DiaryContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (tbTitle.Text.Length > 0 && tbContent.Text.Length > 0 && dpDate.SelectedDate != null)
            {
                btnWritePost.IsEnabled = true;
            }
            else
            {
                btnWritePost.IsEnabled = false;
            }
        }

        public void ClearPostData()
        {
            dpDate.SelectedDate = null;
            tbTitle.Text = string.Empty;
            tbContent.Text = string.Empty;
        }
    }
}
