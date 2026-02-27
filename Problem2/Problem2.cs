public class OrderItem
{
    public string ProductName {get; set;}
    public int Quantity {get; set;}
    public decimal UnitPrice {get; set;}
    public decimal TotalPrice => Quantity * UnitPrice;
}

public class Order
{
    public string CustomerName {get; set;}
    public List<OrderItem> Items {get; set;} = new List<OrderItem>();

    public decimal CalculateFinalPrice()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.TotalPrice;
        }

        if(total > 500)
        {
            total = total - (total * 0.10m);
        }

        return total;
    } 
}

public class Manager
{
    public string SpentTheMost(List<Order> allOrders)
    {
        return allOrders
            .GroupBy(o => o.CustomerName)
            .Select(group => new
            {
                CustomerName = group.Key,
                TotalSpent = group.Sum(o => o.CalculateFinalPrice())
            })
            .OrderByDescending(x => x.TotalSpent)
            .FirstOrDefault()?.CustomerName;
    }

    public Dictionary<string, int> PopularProducts(List<Order> allOrders)
    {
        return allOrders
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ProductName)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(i => i.Quantity)
            );
    }
}

class Prog
{
    static void Main()
    {
        var orders = new List<Order>
        {
            new Order
            {
                CustomerName = "Mihai Pop",
                Items =
                {
                    new OrderItem
                    {
                        ProductName = "Laptop", Quantity = 1, UnitPrice =  1000
                    },
                    new OrderItem
                    {
                        ProductName = "Smart Watch", Quantity = 3, UnitPrice = 500
                    }
                    
                }
            },
            new Order
            {
                CustomerName = "Ana Ion",
                Items =
                {
                    new OrderItem {ProductName = "Mouse", Quantity = 1, UnitPrice = 100}
                }
            }
        };

        var mng = new Manager();
        var topCustomer = mng.SpentTheMost(orders);

        var totalSpent = orders
            .Where(o => o.CustomerName == topCustomer)
            .Sum(o => o.CalculateFinalPrice());

        Console.Write("The client who spent the most is ");
        Console.WriteLine(topCustomer);

        Console.Write("The customer spent a total of ");
        Console.WriteLine(totalSpent);

        Console.WriteLine("\nPopular products:");
        foreach(var p in mng.PopularProducts(orders))
        {
            Console.WriteLine($"{p.Key}: {p.Value}");
        }
    }
}
