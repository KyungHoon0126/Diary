using System;

namespace Making_A_Diary.Model
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
}
