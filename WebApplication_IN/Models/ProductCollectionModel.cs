using WebApplication_IN.Enums;

namespace WebApplication_IN.Models
{
    public class ProductCollectionModel
    {
        /// <summary>
        /// Default page of the product collection, if not specified by the user
        /// </summary>
        private int _defaultPage = 1;
        /// <summary>
        /// Default number of products on the page, if not specified by the user
        /// </summary>
        private int _defaultPageSize = 5;

        /// <summary>
        /// The page number
        /// </summary>
        public int Page 
        {
            get
            {
                return _defaultPage;
            }
            set
            {
                if (value > 0)
                {
                    _defaultPage = value;
                }
            }
        }

        /// <summary>
        /// The number of items on page
        /// </summary>
        public int PageSize
        {
            get
            {
                return _defaultPageSize;
            }
            set
            {
                if (value > 0)
                {
                    _defaultPageSize = value;
                }
            }
        }

        /// <summary>
        /// Category name <p><i>Options:</i> <ul><li>1 = Food</li><li>2 = Drinks</li><li>3 = Clothes</li><li>4 = Sport</li></ul></p>
        /// </summary>
        public IEnumerable<Categories>? Category { get; }


    }
}
