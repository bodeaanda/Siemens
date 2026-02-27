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
