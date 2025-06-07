using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace HealthDesctop.source.LocalDB.Tables
{

    // Класс для Локальной SQL БД - Цвет категории продукта
    public class tbl_CategoryColor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public Color GetColor() => Color.FromRgb(this.Red, this.Green, this.Blue);
    }
}