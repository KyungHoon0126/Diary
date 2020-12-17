using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleDiary.Model
{
    [Table("diary_data")]
    public class DiaryData
    {
        [Key]
        public int Idx { get; set; }
        
        [Column("diary_title")]
        public string Title { get; set; }
        [Column("diary_content")]
        public string Content { get; set; }
        [Column("written_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // CURRENT_TIMESTAMP
        public DateTime WrittenTime { get; set; }
    }
}
