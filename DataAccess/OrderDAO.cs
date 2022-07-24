using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Order> GetOrderList()
        {
            List<Order> orders;
            try
            {
                var myStockDB = new FSTOREContext();
                orders = myStockDB.Orders.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public Order GetOrderByID(int orderID)
        {
            Order order = null;
            try
            {
                var myStockDB = new FSTOREContext();
                order = myStockDB.Orders.SingleOrDefault(order => order.OrderId == orderID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }
        public void AddNew(Order order)
        {
            try
            {
                Order _car = GetOrderByID(order.OrderId);
                if (_car == null)
                {
                    var myStockDB = new FSTOREContext();
                    myStockDB.Orders.Add(order);
                    myStockDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The order is already exist.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(Order order)
        {
            try
            {
                Order _order = GetOrderByID(order.OrderId);
                if (_order != null)
                {
                    var myStockDB = new FSTOREContext();
                    myStockDB.Entry<Order>(order).State = EntityState.Modified;
                    myStockDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The order does not already exist.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Remove(Order order)
        {
            try
            {
                Order _order = GetOrderByID(order.OrderId);
                if (_order != null)
                {
                    var myStockDB = new FSTOREContext();
                    myStockDB.Orders.Remove(order);
                    myStockDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The order does not already exist.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
