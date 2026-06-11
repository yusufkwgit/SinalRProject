using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinnesLayer.Abstract
{
    public interface IMenuTableService : IGenericService<MenuTable>
    {
        int TMenuTableCount();
        void TChangeMenuTableStatusToTrue(int id);
        void TChangeMenuTableStatusToFalse(int id);
        void TApproveOrder(int id);
        void TRejectOrder(int id);
        void TTransferTable(int oldTableId, int newTableId);
        void TCloseTablePayment(int tableId);
        void TAddProductToOrder(int orderId, int productId);
        void TRemoveProductFromOrder(int orderDetailId);
    }
}
