using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }

        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance != null) return instance;
                    instance = new ProductDAO();
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetProductList()
        {
            List<Product> products;
            try
            {
                FSTOREContext myStockDB = new FSTOREContext();
                products = myStockDB.Products.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Catch Exception: {ex.Message}");
            }

            return products;
        }

        public Product GetProductByID(int productID)
        {
            Product product;
            try
            {
                var myStockDB = new FSTOREContext();
                product = myStockDB.Products.SingleOrDefault(p => p.ProductId == productID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Catch Exception: {ex.Message}");
            }

            return product;

        }

        public void AddNew(Product product)
        {
            try
            {
                Product _product = GetProductByID(product.ProductId);
                if (_product == null)
                {
                    var myStockDb = new FSTOREContext();
                    myStockDb.Products.Add(product);
                    myStockDb.SaveChanges();
                }
                else
                {
                    throw new Exception("The product already exist.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Catch Exception: " + ex.Message);
            }
        }

        public void Update(Product product)
        {
            try
            {
                Product p = GetProductByID(product.ProductId);
                if (p != null)
                {
                    var myStockDb = new FSTOREContext();
                    myStockDb.Entry<Product>(product).State = EntityState.Modified;
                    myStockDb.SaveChanges();
                }
                else
                {
                    throw new Exception("The car does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Catch Exception: " + ex.Message);
            }
        }

        public void Remove(Product product)
        {
            try
            {
                Product _product = GetProductByID(product.ProductId);
                if (_product != null)
                {
                    var myStockDb = new FSTOREContext();
                    myStockDb.Remove(product);
                    myStockDb.SaveChanges();
                }
                else
                {
                    throw new Exception("The car does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Catch Exception: " + ex.Message);
            }
        }
    }
}
