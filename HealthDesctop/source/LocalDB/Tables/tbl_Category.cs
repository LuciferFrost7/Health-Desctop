using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthDesctop.source.LocalDB.Tables
{

    // Класс для Локальной SQL БД - Категория продукта
    public class tbl_Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Fk_Color { get; set; }

        public tbl_CategoryColor Color { get; set; }

        public ICollection<tbl_ListOfProducts> ListOfProducts { get; set; }
    }

}