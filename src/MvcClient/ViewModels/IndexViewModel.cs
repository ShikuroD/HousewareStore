using System.Collections.Generic;
using MvcClient.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcClient.ViewModels
{
    public class IndexViewModel
    {
        public string SearchString { get; set; }
        public string ItemCategory { get; set; }

        [Display(Name = "Min Price")]
        public double minPrice { get; set; }

        [Display(Name = "Max Price")]
        public double maxPrice { get; set; }
        public string SortOrder { get; set; }
        public IList<Item> LatestItems { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public PaginatedList<Item> ItemsPaging { get; set; }
        public string OwnerId { get; set; }
        public string UserRole { get; set; }
        public int PageIndex { get; set; }
        public int PageTotal { get; set; }
        public IEnumerable<int> CategoriesId { get; set; }
        public IList<Item> Items { get; set; }
    }
}