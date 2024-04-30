using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebApplication_IN.Models
{
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Product Id
        /// </summary>
        [IgnoreDataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }
        /// <summary>
        /// EAN
        /// </summary>
        [XmlElement("Ean")]
        public int Ean { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [XmlElement("Quantity")]
        public int Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [XmlElement("Price")]
        public int Price { get; set; }
        /// <summary>
        /// Currency
        /// </summary>
        [XmlElement("Currency")]
        public string Currency { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        [XmlElement("Category")]
        public string Category { get; set; }
        /// <summary>
        /// PriceUpdated
        /// </summary>
        [IgnoreDataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedPrice { get; set; }
    }
}
