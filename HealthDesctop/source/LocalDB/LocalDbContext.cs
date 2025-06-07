using Microsoft.EntityFrameworkCore;
using HealthDesctop.source.LocalDB.Tables;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace HealthDesctop.source.LocalDB
{
    public class LocalDbContext : DbContext
    {
        public DbSet<tbl_Products> Products { get; set; }
        public DbSet<tbl_Category> Categories { get; set; }
        public DbSet<tbl_CategoryColor> CategoryColors { get; set; }
        public DbSet<tbl_ListOfProducts> ListOfProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=HealthDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей, если нужно (по желанию)
        }
    }
    

    public class DbCommands
    {
        // ---------------------- PRODUCT ----------------------
        
        public static void AddProduct(string name, int callories, int proteins, int fats, int carbs)
        {
            using var db = new LocalDbContext();
            var product = new tbl_Products
            {
                Name = name,
                Calories = callories,
                Proteins = proteins,
                Fats = fats,
                Carbohydrates = carbs
            };
            db.Products.Add(product);
            db.SaveChanges();
        }
        
        public static List<tbl_Products> GetAllProducts()
        {
            using var db = new LocalDbContext();
            return db.Products.ToList();
        }
        
        public static void UpdateProduct(int id, string newName, int? callories = null, int? proteins = null, int? fats = null, int? carbs = null)
        {
            using var db = new LocalDbContext();
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Name = newName ?? product.Name;
                if (callories.HasValue) product.Calories = callories.Value;
                if (proteins.HasValue) product.Proteins = proteins.Value;
                if (fats.HasValue) product.Fats = fats.Value;
                if (carbs.HasValue) product.Carbohydrates = carbs.Value;

                db.SaveChanges();
            }
        }
        
        public static void DeleteProduct(int id)
        {
            using var db = new LocalDbContext();
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }
        
        public static tbl_Products GetProductById(int id)
        {
            using var db = new LocalDbContext();
            return db.Products.FirstOrDefault(p => p.Id == id);
        }
        
         // ---------------------- CATEGORY COLOR ----------------------

        public static void AddCategoryColor(string name, byte r, byte g, byte b)
        {
            using var db = new LocalDbContext();
            var color = new tbl_CategoryColor
            {
                Name = name,
                Red = r,
                Green = g,
                Blue = b
            };
            db.CategoryColors.Add(color);
            db.SaveChanges();
        }

        public static List<tbl_CategoryColor> GetAllCategoryColors()
        {
            using var db = new LocalDbContext();
            return db.CategoryColors.ToList();
        }

        public static void UpdateCategoryColor(int id, string name = null, byte? r = null, byte? g = null, byte? b = null)
        {
            using var db = new LocalDbContext();
            var color = db.CategoryColors.FirstOrDefault(c => c.Id == id);
            if (color != null)
            {
                color.Name = name ?? color.Name;
                if (r.HasValue) color.Red = r.Value;
                if (g.HasValue) color.Green = g.Value;
                if (b.HasValue) color.Blue = b.Value;
                db.SaveChanges();
            }
        }

        public static void DeleteCategoryColor(int id)
        {
            using var db = new LocalDbContext();
            var color = db.CategoryColors.FirstOrDefault(c => c.Id == id);
            if (color != null)
            {
                db.CategoryColors.Remove(color);
                db.SaveChanges();
            }
        }

        // ---------------------- CATEGORY ----------------------

        public static void AddCategory(string name, int fk_color)
        {
            using var db = new LocalDbContext();
            var category = new tbl_Category
            {
                Name = name,
                Fk_Color = fk_color
            };
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public static List<tbl_Category> GetAllCategories()
        {
            using var db = new LocalDbContext();
            return db.Categories.Include(c => c.Color).ToList();
        }

        public static void UpdateCategory(int id, string name = null, int? fk_color = null)
        {
            using var db = new LocalDbContext();
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.Name = name ?? category.Name;
                if (fk_color.HasValue) category.Fk_Color = fk_color.Value;
                db.SaveChanges();
            }
        }

        public static void DeleteCategory(int id)
        {
            using var db = new LocalDbContext();
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
            }
        }

        // ---------------------- LIST OF PRODUCTS ----------------------

        public static void AddListEntry(int fk_productId, int fk_categoryId)
        {
            using var db = new LocalDbContext();
            var entry = new tbl_ListOfProducts
            {
                Fk_ProductId = fk_productId,
                Fk_CategoryId = fk_categoryId
            };
            db.ListOfProducts.Add(entry);
            db.SaveChanges();
        }

        public static List<tbl_ListOfProducts> GetAllListEntries()
        {
            using var db = new LocalDbContext();
            return db.ListOfProducts
                .Include(lp => lp.Product)
                .Include(lp => lp.Category)
                    .ThenInclude(c => c.Color)
                .ToList();
        }

        public static void DeleteListEntry(int id)
        {
            using var db = new LocalDbContext();
            var entry = db.ListOfProducts.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                db.ListOfProducts.Remove(entry);
                db.SaveChanges();
            }
        }
    }

}