using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        //Using singleton pattern
        private static OrderDetailDAO instance = null;
        public static readonly object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetailList()
        {
            List<OrderDetail> OrderDetails;
            try
            {
                var myStockDB = new FSTOREContext();
                OrderDetails = myStockDB.OrderDetails.Include(o => o.Product).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return OrderDetails;

        }

        public OrderDetail GetOrderDetailByID(int OrderID, int ProductID)
        {
            OrderDetail orderDetail = null;
            try
            {
                var FStoreDB = new FSTOREContext();
                orderDetail = FStoreDB.OrderDetails.SingleOrDefault
                    (orderDetail => orderDetail.OrderId == OrderID &&
                    orderDetail.ProductId == ProductID);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetail;
        }

        public void AddNew(OrderDetail orderDetail)
        {
            try
            {
                OrderDetail _orderDetail = GetOrderDetailByID(orderDetail.OrderId, orderDetail.ProductId);
                if (_orderDetail == null)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.OrderDetails.Add(orderDetail);
                    FStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The OrderDetail is already exist. ");
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddMany(IEnumerable<OrderDetail> orderDetails)
        {
            try
            {
                foreach (var _orderDetail in orderDetails)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.OrderDetails.AddRange(orderDetails);
                    FStoreDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(OrderDetail orderDetail)
        {
            try
            {
                OrderDetail _orderDetail = GetOrderDetailByID
                    (orderDetail.OrderId, orderDetail.ProductId);

                if (_orderDetail != null)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.Entry<OrderDetail>(orderDetail).State = EntityState.Modified;
                    FStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The OrderDetail does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(OrderDetail orderDetail)
        {
            try
            {
                OrderDetail _orderDetail = GetOrderDetailByID
                    (orderDetail.OrderId, orderDetail.ProductId);
                if (_orderDetail != null)
                {
                    var FStoreDB = new FSTOREContext();
                    FStoreDB.OrderDetails.Remove(orderDetail);
                    FStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The OrderDetail does not already exist. ");
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
