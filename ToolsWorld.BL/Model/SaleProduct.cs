namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Проданные товары.
    /// </summary>
    public class SaleProduct
    {
        static readonly MyDbContext myDbContext = new MyDbContext();

        /// <summary>
        /// Первичный ключ.
        /// </summary>
        public int SaleProductId { get; set; }

        /// <summary>
        /// Идентификатор продажи.
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Сумма проданных товаров.
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Продажа. 
        /// </summary>
        public virtual Sale Sale { get; set; }

        /// <summary>
        /// Товар. 
        /// </summary>
        public virtual Product Product { get; set; }

        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="productId"></param>
        /// <param name="amount"></param>
        /// <param name="sum"></param>
        public static void Insert(int saleId, int productId, int amount, double sum)
        {
            SaleProduct saleProduct = new SaleProduct
            {
                SaleId = saleId,
                ProductId = productId,
                Amount = amount,
                Sum = sum
            };

            myDbContext.SaleProducts.Add(saleProduct);
            myDbContext.SaveChanges();
        }

        #endregion
    }
}
