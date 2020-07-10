using System.Collections.Generic;
using System.ComponentModel;
using ToolsWorld.BL.Model;

namespace ToolsWorld.BL.ViewModel
{
    public class SupplierViewModel
    {
        /// <summary>
        /// Коллекция поставщиков.
        /// </summary>
        public ICollection<Supplier> Suppliers { get; set; }

        public SupplierViewModel()
        {
            Suppliers = Supplier.GetSuppliers();
        }
    }
}
