namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Заказанные товары.
    /// </summary>
    public class OrderProduct
    {
        private static MyDbContext myDbContext = new MyDbContext();

        /// <summary>
        /// Первичный ключ.
        /// </summary>
        public int OrderProductId { get; set; }

        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Закупочная цена.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Заказ.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Товар.
        /// </summary>
        public virtual Product Product { get; set; }


        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <param name="productId">Идентификатор товара.</param>
        /// <param name="amount">Количество.</param>
        /// <param name="price">Цена.</param>
        public static void Insert(int orderId, int productId, int amount, double price)
        {
            OrderProduct orderProduct = new OrderProduct
            {
                OrderId = orderId,
                ProductId = productId,
                Amount = amount,
                Price = price,
                Sum = amount*price
            };

            myDbContext.OrderProducts.Add(orderProduct);
            myDbContext.SaveChanges();
        }

        #endregion
    }
}