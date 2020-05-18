using HealthStation_Test.Data;
using HealthStation_Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthStation_Test.Services
{
   public class CategoriesSevice
    {
        #region Define as Singleton
        private static CategoriesSevice _Instance;

        public static CategoriesSevice Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CategoriesSevice();
                }

                return (_Instance);
            }
        }

        private CategoriesSevice()
        {
        }
        #endregion

        public List<Category> GetAllCategories()
        {
            MyContext context = new MyContext();

            return context.Categories.OrderBy(x => x.Name).ToList();
        }

        //public List<Category> GetFeaturedCategories(int recordSize = 5)
        //{
        //    MyContext context = new MyContext();

        //    return context.Categories.Where(x => x.isFeatured).OrderBy(x => x.DisplaySeqNo).Take(recordSize).ToList();
        //}

        //public List<Category> GetAllParentCategories()
        //{
        //    MyContext context = new MyContext();

        //    return context.Categories.Where(x => !x.ParentCategoryID.HasValue).OrderBy(x => x.DisplaySeqNo).ToList();
        //}

        public Category GetCategoryByID(int ID)
        {
            using (var context = new MyContext())
            {
                return context.Categories.Find(ID);
            }
        }

        public Category GetCategoryByName(string sanitizedCategoryName)
        {
            MyContext context = new MyContext();

            return context.Categories.FirstOrDefault(x => x.Name.Equals(sanitizedCategoryName));
        }

        public void SaveCategory(Category category)
        {
            MyContext context = new MyContext();

            context.Categories.Add(category);

            context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            MyContext context = new MyContext();

            context.Entry(category).State = System.Data.Entity.EntityState.Modified;

            context.SaveChanges();
        }

        public bool DeleteCategory(int ID)
        {
            using (var context = new MyContext())
            {
                var category = context.Categories.Find(ID);

                context.Categories.Remove(category);

                return context.SaveChanges() > 0;
            }
        }

        public List<Category> SearchCategories(int? CategoryID, string searchTerm, int? pageNo, int pageSize)
        {
            MyContext context = new MyContext();

            var categories = context.Categories.AsQueryable();

            if (CategoryID.HasValue && CategoryID.Value > 0)
            {
                categories = categories.Where(x => x.ID == CategoryID.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                categories = categories.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * pageSize;

            return categories.OrderBy(x => x.Name).Skip(skipCount).Take(pageSize).ToList();
        }

        public int GetCategoriesCount(int? CategoryID, string searchTerm)
        {
            MyContext context = new MyContext();

            var categories = context.Categories.AsQueryable();

            if (CategoryID.HasValue && CategoryID.Value > 0)
            {
                categories = categories.Where(x => x.ID == CategoryID.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                categories = categories.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return categories.Count();
        }
    }
}
