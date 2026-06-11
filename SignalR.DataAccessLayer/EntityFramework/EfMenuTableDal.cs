using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfMenuTableDal : GenericRepository<MenuTable>, IMenuTableDal
    {
        public EfMenuTableDal(SignalRContext context) : base(context)
        {
        }

        public void ChangeMenuTableStatusToFalse(int id)
        {
            using var context = new SignalRContext();
            var value = context.MenuTables.Where(x => x.MenuTableID == id).FirstOrDefault();
            value.Status = false;
            context.SaveChanges();
        }

        public void ChangeMenuTableStatusToTrue(int id)
        {
            using var context = new SignalRContext();
            var value = context.MenuTables.Where(x => x.MenuTableID == id).FirstOrDefault();
            value.Status = true;
            context.SaveChanges();
        }

        public int MenuTableCount()
        {
            using var context = new SignalRContext();
            return context.MenuTables.Count();
        }

        public void ApproveOrder(int id)
        {
            using var context = new SignalRContext();
            var table = context.MenuTables.Where(x => x.MenuTableID == id).FirstOrDefault();
            if (table != null)
            {
                var order = context.Orders.Where(x => x.TableNumber == table.Name && x.Description == "Onay Bekliyor").FirstOrDefault();
                if (order != null)
                {
                    order.Description = "Onaylandı";
                    context.SaveChanges();
                }
            }
        }

        public void RejectOrder(int id)
        {
            using var context = new SignalRContext();
            var table = context.MenuTables.Where(x => x.MenuTableID == id).FirstOrDefault();
            if (table != null)
            {
                table.Status = false;
                var order = context.Orders.Where(x => x.TableNumber == table.Name && x.Description == "Onay Bekliyor").FirstOrDefault();
                if (order != null)
                {
                    order.Description = "Reddedildi";
                }
                context.SaveChanges();
            }
        }

        public void TransferTable(int oldTableId, int newTableId)
        {
            using var context = new SignalRContext();
            var oldTable = context.MenuTables.FirstOrDefault(x => x.MenuTableID == oldTableId);
            var newTable = context.MenuTables.FirstOrDefault(x => x.MenuTableID == newTableId);

            if (oldTable != null && newTable != null)
            {
                var activeOrder = context.Orders.FirstOrDefault(x => x.TableNumber == oldTable.Name && x.Description != "Ödendi" && x.Description != "Reddedildi");
                if (activeOrder != null)
                {
                    activeOrder.TableNumber = newTable.Name;
                }
                oldTable.Status = false;
                newTable.Status = true;
                context.SaveChanges();
            }
        }

        public void CloseTablePayment(int tableId)
        {
            using var context = new SignalRContext();
            var table = context.MenuTables.FirstOrDefault(x => x.MenuTableID == tableId);
            if (table != null)
            {
                var activeOrder = context.Orders.FirstOrDefault(x => x.TableNumber == table.Name && x.Description != "Ödendi" && x.Description != "Reddedildi");
                if (activeOrder != null)
                {
                    activeOrder.Description = "Ödendi";
                    // Add TotalPrice to MoneyCase
                    var moneyCase = context.MoneyCases.FirstOrDefault();
                    if (moneyCase != null)
                    {
                        moneyCase.TotalAmount += activeOrder.TotalPrice;
                    }
                    else
                    {
                        context.MoneyCases.Add(new MoneyCase { TotalAmount = activeOrder.TotalPrice });
                    }
                }
                table.Status = false;
                context.SaveChanges();
            }
        }

        public void AddProductToOrder(int orderId, int productId)
        {
            using var context = new SignalRContext();
            var order = context.Orders.FirstOrDefault(x => x.OrderID == orderId);
            var product = context.Products.FirstOrDefault(x => x.ProductID == productId);

            if (order != null && product != null)
            {
                var existingDetail = context.OrderDetails.FirstOrDefault(x => x.OrderID == orderId && x.ProductID == productId);
                if (existingDetail != null)
                {
                    existingDetail.Count++;
                    existingDetail.TotalPrice = existingDetail.Count * existingDetail.UnitPrice;
                }
                else
                {
                    context.OrderDetails.Add(new OrderDetail
                    {
                        OrderID = orderId,
                        ProductID = productId,
                        Count = 1,
                        UnitPrice = product.Price,
                        TotalPrice = product.Price
                    });
                }
                
                context.SaveChanges();

                // Recalculate order total price
                order.TotalPrice = context.OrderDetails.Where(x => x.OrderID == orderId).Sum(x => x.TotalPrice);
                context.SaveChanges();
            }
        }

        public void RemoveProductFromOrder(int orderDetailId)
        {
            using var context = new SignalRContext();
            var detail = context.OrderDetails.FirstOrDefault(x => x.OrderDetailID == orderDetailId);
            if (detail != null)
            {
                var order = context.Orders.FirstOrDefault(x => x.OrderID == detail.OrderID);
                if (detail.Count > 1)
                {
                    detail.Count--;
                    detail.TotalPrice = detail.Count * detail.UnitPrice;
                }
                else
                {
                    context.OrderDetails.Remove(detail);
                }
                
                context.SaveChanges();

                if (order != null)
                {
                    order.TotalPrice = context.OrderDetails.Where(x => x.OrderID == order.OrderID).Sum(x => x.TotalPrice);
                    context.SaveChanges();
                }
            }
        }
    }
}
