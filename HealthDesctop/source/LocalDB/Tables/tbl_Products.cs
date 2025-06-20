﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthDesctop.source.LocalDB.Tables
{
    // Класс для Локальной SQL БД - Продукт
    public class tbl_Products
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Calories { get; set; }       // Исправлено
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }

        // Навигационное свойство к связующей таблице
        public ICollection<tbl_ListOfProducts> ListOfProducts { get; set; }
        
    }
}