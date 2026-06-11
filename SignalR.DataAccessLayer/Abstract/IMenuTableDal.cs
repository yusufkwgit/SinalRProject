using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IMenuTableDal : IGenericDal<MenuTable>
    {
        int MenuTableCount();
        void ChangeMenuTableStatusToTrue(int id);
        void ChangeMenuTableStatusToFalse(int id);
        void ApproveOrder(int id);
        void RejectOrder(int id);
        void TransferTable(int oldTableId, int newTableId);
        void CloseTablePayment(int tableId);
        void AddProductToOrder(int orderId, int productId);
        void RemoveProductFromOrder(int orderDetailId);
    }
}
