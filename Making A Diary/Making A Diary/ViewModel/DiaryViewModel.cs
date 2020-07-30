using Making_A_Diary.Common;
using Making_A_Diary.ViewModel;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Making_A_Diary.ViewModel
{
    public class Diary : ICloneable
    {
        public string DiaryTitle { get; set; }

        public object Clone()
        {
            return new Diary
            {
                DiaryTitle = this.DiaryTitle
            };
        }
    }

    public class DiaryViewModel
    {
        public delegate void OnWriteDiaryResultReceivedHandler();
        public event OnWriteDiaryResultReceivedHandler OnWriteDiaryResultReceived;

        #region Property
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Diary> diaryItems = new List<Diary>();
        public List<Diary> DiaryItems
        {
            get => diaryItems;
            set
            {
                diaryItems = value;
                NotifyPropertyChanged(nameof(DiaryItems));
            }
        }

        private string openDiaryContent;
        public string OpenDiaryContent
        {
            get => openDiaryContent;
            set
            {
                openDiaryContent = value;
                NotifyPropertyChanged(nameof(OpenDiaryContent));
            }
        }

        private string writeDiaryContent;
        public string WriteDiaryContent
        {
            get => writeDiaryContent;
            set
            {
                writeDiaryContent = value;
                NotifyPropertyChanged(nameof(WriteDiaryContent));
            }
        }

        public ICommand DiaryOpenCommand { get; set; }
        public ICommand DiaryWriteCommand { get; set; }
        #endregion

        public DiaryViewModel()
        {
            DiaryOpenCommand = new DelegateCommand(OnOpenDiary);
            DiaryWriteCommand = new DelegateCommand(OnDiaryWrite);
        }

        #region INIT
        public void IsDirectoryExsist()
        {
            DirectoryInfo directory = new DirectoryInfo(ComDef.folderPath);

            if (directory.Exists == false)
            {
                directory.Create();
            }
        }
        #endregion

        #region Diary Open
        private void OnOpenDiary()
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                {
                    openFile.Filter = "Text File | *.txt";
                    openFile.FileOk += OpenFile_FileOk;
                    openFile.CheckFileExists = true;
                    openFile.CheckPathExists = true;

                    if (openFile.ShowDialog().GetValueOrDefault())
                    {
                        ComDef.file = openFile.FileName;

                        using (StreamReader sr = new StreamReader(ComDef.file))
                        {
                            OpenDiaryContent = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void OpenFile_FileOk(object sender, CancelEventArgs e)
        {
            // File Path
            var filePath = (sender as OpenFileDialog).FileName;
            // FileName
            var fileName = (sender as OpenFileDialog).SafeFileName;
            filePath = filePath.Replace(fileName, "");
            filePath = filePath.Remove(filePath.Length - 1);

            if (!(filePath.Replace(fileName, "") == ComDef.folderPath))
            {
                MessageBox.Show("일기를 보고싶을 때 " + ComDef.folderPath + "이 위치에서 불러와 주세요.");
                e.Cancel = true;
            }
        }
        #endregion

        #region Diary Write
        private void OnDiaryWrite()
        {
            if(WriteDiaryContent != null && WriteDiaryContent.Length > 0)
            {
                try
                {
                    if (ComDef.file == "")
                    {
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.Filter = "Text File | *.txt";
                        saveFile.FileOk += SaveFile_FileOk;

                        if (saveFile.ShowDialog().GetValueOrDefault())
                        {
                            ComDef.file = saveFile.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(ComDef.file, false, Encoding.UTF8))
                    {
                        sw.Write(writeDiaryContent);
                    }

                    OnWriteDiaryResultReceived?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                finally
                {
                    GetDiaryList();
                }
            }
            else
            {
                MessageBox.Show("내용을 입력해 주세요!");
                return;
            }
        }

        private void SaveFile_FileOk(object sender, CancelEventArgs e)
        {
            if (!(Path.GetDirectoryName((sender as SaveFileDialog).FileName) == ComDef.folderPath))
            {
                MessageBox.Show("일기를 저장할 때 " + ComDef.folderPath + "이 위치에 저장해 주세요.");
                e.Cancel = true;
            }
        }
        #endregion

        #region GetDiaryList
        public void GetDiaryList()
        {
            DirectoryInfo directory = new DirectoryInfo(ComDef.folderPath);
            List<string> fileList = new List<string>();
            
            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.Extension.ToLower().CompareTo(".txt") == 0)
                {
                    string fileNameOnly = file.Name.Substring(0, file.Name.Length - 4);
                    fileList.Add(fileNameOnly);
                }
            }

            Diary diary = new Diary();

            foreach(var item in fileList)
            {
                diary.DiaryTitle = item;
                DiaryItems.Add((Diary)diary.Clone());
            }
        }
        #endregion
    }
}
