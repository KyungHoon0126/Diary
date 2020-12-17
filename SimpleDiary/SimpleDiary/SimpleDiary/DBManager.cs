using SimpleDiary.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiary
{
    public class DBManager
    {
        public DiaryDBContext diaryDBContext = new DiaryDBContext();

        public void SaveDiary(string title, string content, string writeDate)
        {
            diaryDBContext.Add(new DiaryData()
            {
                Title = title,
                Content = content,
                WrittenTime = Convert.ToDateTime(writeDate)
            });

            SaveDataBase();
        }

        public List<DiaryData> GetDiaryDatas()
        {
            return diaryDBContext.DiaryDatas.OrderBy(x => x.Idx).ToList();
        }

        // DB가 없으면 생성
        public void CreateDataBase()
        {
            diaryDBContext.Database.EnsureCreated();
        }

        private void SaveDataBase()
        {
            diaryDBContext.SaveChanges();
        }

        public void DeleteDiary(DiaryData selectedDiary)
        {
            diaryDBContext.DiaryDatas.Remove(selectedDiary);
            SaveDataBase();
        }
    }
}
