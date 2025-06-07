using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthDesctop.source.LocalDB.Tables
{
    // Класс для Локальной SQL БД - Список продуктов
    public class tbl_ListOfProducts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int Fk_ProductId { get; set; }
        public tbl_Products Product { get; set; }

        [ForeignKey(nameof(Category))]
        public int Fk_CategoryId { get; set; }
        public tbl_Category Category { get; set; }
    }

}