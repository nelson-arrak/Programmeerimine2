using System;
using System.Collections.Generic;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        private const int RowCount = 35;

        public static void Generate(ApplicationDbContext context)
        {
            SeedCustomers(context);
            SeedCategories(context);
            SeedAddresses(context);
            SeedProducts(context);
            SeedOrders(context);
            SeedOrderItems(context);
            SeedPayments(context);
        }

        private static void SeedCustomers(ApplicationDbContext context)
        {
            if (context.Customers.Any())
                return;

            var customers = new List<Customer>();

            for (int i = 1; i <= RowCount; i++)
            {
                customers.Add(new Customer
                {
                    Name = $"user{i}",
                    PasswordHash = $"password{i}",
                    Email = $"customer{i}@example.com",
                    PhoneNumber = $"{i:00000000}"
                });
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        private static void SeedCategories(ApplicationDbContext context)
        {
            if (context.Category.Any())
                return;

            var categories = new List<Category>();

            for (int i = 1; i <= RowCount; i++)
            {
                categories.Add(new Category
                {
                    Name = $"category{i}"
                });
            }

            context.Category.AddRange(categories);
            context.SaveChanges();
        }

        private static void SeedAddresses(ApplicationDbContext context)
        {
            if (context.Addresses.Any())
                return;

            var addresses = new List<Address>();

            for (int i = 1; i <= RowCount; i++)
            {
                addresses.Add(new Address
                {
                    Street = $"Example Street {i}",
                    City = "Tallinn",
                    State = "Harjumaa",
                    ZipCode = $"{10000 + i}",
                    Country = "Estonia"
                });
            }

            context.Addresses.AddRange(addresses);
            context.SaveChanges();
        }

        private static void SeedProducts(ApplicationDbContext context)
        {
            if (context.Products.Any())
                return;

            var categories = context.Category
                .OrderBy(c => c.Id)
                .ToList();

            if (categories.Count < RowCount)
                return;

            var products = new List<Product>();

            for (int i = 1; i <= RowCount; i++)
            {
                products.Add(new Product
                {
                    Name = $"product{i}",
                    Description = $"Description for product {i}",
                    Price = 5.00m + i,
                    Category = categories[i - 1]
                });
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void SeedOrders(ApplicationDbContext context)
        {
            if (context.Orders.Any())
                return;

            var customers = context.Customers
                .OrderBy(c => c.Id)
                .ToList();

            if (customers.Count < RowCount)
                return;

            var orders = new List<Order>();

            for (int i = 1; i <= RowCount; i++)
            {
                orders.Add(new Order
                {
                    Customer = customers[i - 1],
                    OrderDate = new DateTime(2026, 1, 1).AddDays(i - 1),
                    Status = i % 3 == 0 ? "Completed" : "Pending",
                    OrderItems = new List<OrderItem>()
                });
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        private static void SeedOrderItems(ApplicationDbContext context)
        {
            if (context.OrderItems.Any())
                return;

            var products = context.Products
                .OrderBy(p => p.Id)
                .ToList();

            var orders = context.Orders
                .OrderBy(o => o.Id)
                .ToList();

            if (products.Count < RowCount || orders.Count < RowCount)
                return;

            var orderItems = new List<OrderItem>();

            for (int i = 0; i < RowCount; i++)
            {
                orderItems.Add(new OrderItem
                {
                    Order = orders[i],
                    Product = products[i],
                    Quantity = 1,
                    Price = products[i].Price
                });
            }

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();
        }

        private static void SeedPayments(ApplicationDbContext context)
        {
            if (context.Set<Payment>().Any())
                return;

            var orders = context.Orders
                .OrderBy(o => o.Id)
                .ToList();

            var orderItems = context.OrderItems
                .OrderBy(oi => oi.Id)
                .ToList();

            if (orders.Count < RowCount || orderItems.Count < RowCount)
                return;

            var payments = new List<Payment>();

            for (int i = 0; i < RowCount; i++)
            {
                payments.Add(new Payment
                {
                    OrderId = orders[i].Id,
                    Order = orders[i],
                    Amount = orderItems[i].Price * orderItems[i].Quantity,
                    PaymentDate = new DateTime(2026, 1, 1).AddDays(i),
                    PaymentMethod = i % 2 == 0 ? "Card" : "Bank transfer",
                    Status = i % 3 == 0
                });
            }

            context.Set<Payment>().AddRange(payments);
            context.SaveChanges();
        }
    }
}