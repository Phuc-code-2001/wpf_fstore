using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public Order GetOrderByID(int orderId) => OrderDAO.Instance.GetOrderByID(orderId);
        public IEnumerable<Order> GetOrders() => OrderDAO.Instance.GetOrderList();
        public void InsertOrder(Order order) => OrderDAO.Instance.AddNew(order);
        public void DeleteOrder(Order order) => OrderDAO.Instance.Remove(order);
        public void UpdateOrder(Order order) => OrderDAO.Instance.Update(order);
    }
}
