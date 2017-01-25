using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Configuration;
namespace DBSD.Services
{
    public class ProductDB:CommandManager
    {
        protected override DbCommand GetCommand(string sql)
        {
            SqlConnection connect = new SqlConnection (ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            return new SqlCommand(sql, connect);
        }
        public List<ProductProperties> GetItems()
        {
            string sql = "Select ProductId, ProductName, ProductDescription, ProductPrice, ProductQuantity from Products";
            var command = GetCommand(sql);
            List<ProductProperties> list = new List<ProductProperties>();
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProductProperties s = new ProductProperties();
                s.ProductId = Convert.ToInt64(reader.GetValue(0));
                s.ProductName = Convert.ToString(reader.GetValue(1));
                s.ProductDescription = Convert.ToString(reader.GetValue(2));
                s.ProductPrice = Convert.ToInt32(reader.GetValue(3));
                s.ProductQuantity = Convert.ToInt32(reader.GetValue(4));
                list.Add(s);
            }
            return list;
        }

        public void AddItem(ProductProperties product)
        {
            var fullSql = string.Format("Insert INTO Products (ProductName, ProductDescription, ProductPrice, ProductQuantity) Values ('{0}','{1}','{2}','{3}')", product.ProductName, product.ProductDescription, product.ProductPrice, product.ProductQuantity);
            var command = GetCommand(fullSql);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public List<ProductProperties> GetById(Int64 id)
        {
            var fullSql = string.Format("Select * from Products WHERE ProductID='{0}'", id);
            var command = GetCommand(fullSql);
            List<ProductProperties> result = new List<ProductProperties>();
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                ProductProperties pr = new ProductProperties();
                pr.ProductId = Convert.ToInt64(reader.GetValue(0));
                pr.ProductName = Convert.ToString(reader.GetValue(1));
                pr.ProductDescription = Convert.ToString(reader.GetValue(2));
                pr.ProductPrice = Convert.ToInt32(reader.GetValue(3));
                pr.ProductQuantity = Convert.ToInt32(reader.GetValue(4));
                result.Add(pr);
            }
            command.Connection.Close();
            return result;
        }

        public void RemoveItem(Int64 id)
        {
            var fullSql = string.Format("Delete from Products WHERE ProductID='{0}'", id);
            var command = GetCommand(fullSql);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public void UpdateItem(ProductProperties t)
        {
            var fullSql = string.Format("UPDATE Products SET ProductName='{0}', ProductDescription='{1}', ProductQuantity='{2}', ProductPrice='{3}' Where ProductId='{4}'", t.ProductName, t.ProductDescription, t.ProductQuantity, t.ProductPrice, t.ProductId);
            var command = GetCommand(fullSql);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public void Order(Int64 productId, Int64 userId) {
            var fullSql = string.Format("INSERT INTO Orders values ('{0}','{1}','{2}','Incomplete')", productId, userId, DateTime.UtcNow.ToString()); 
            var command = GetCommand(fullSql);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public List<OrderDetails> PlaceOrder(Int64 id)
        {
            var fullSql = string.Format("Select distinct o.ProductId, o.UserId, o.OrderId,o.OrderTime, p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice FROM Orders o, Products p, Users u Where p.ProductId = o.ProductId and o.OrderStatus = 'Incomplete' and o.UserId = {0}", id);
            var command = GetCommand(fullSql);
            List<OrderDetails> result = new List<OrderDetails>();
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                OrderDetails pr = new OrderDetails();
                pr.OProductId= Convert.ToInt64(reader.GetValue(0));
                pr.OUserId = Convert.ToInt64(reader.GetValue(1));
                pr.OrderId = Convert.ToInt64(reader.GetValue(2));
                pr.OrderTime = Convert.ToString(reader.GetValue(3));
                pr.PProductId = Convert.ToInt64(reader.GetValue(4));
                pr.PProductName= Convert.ToString(reader.GetValue(5));
                pr.PProductDescription = Convert.ToString(reader.GetValue(6));
                pr.PProductDescription = Convert.ToString(reader.GetValue(7));
                result.Add(pr);
            }
            command.Connection.Close();
            return result;
        }

    }
}

