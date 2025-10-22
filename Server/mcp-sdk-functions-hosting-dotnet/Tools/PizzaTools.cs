using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace QuickstartWeatherServer.Tools;

[McpServerToolType]
public sealed class PizzaTools
{
    // Static data to simulate a pizza ordering system
    private static readonly List<PizzaMenuItem> Menu = new()
    {
        new("Margherita", "Classic tomato sauce, fresh mozzarella, basil", "Small: $12.99, Medium: $15.99, Large: $18.99"),
        new("Pepperoni", "Tomato sauce, mozzarella, pepperoni", "Small: $14.99, Medium: $17.99, Large: $20.99"),
        new("Supreme", "Tomato sauce, mozzarella, pepperoni, sausage, bell peppers, onions, olives", "Small: $17.99, Medium: $21.99, Large: $25.99"),
        new("Hawaiian", "Tomato sauce, mozzarella, ham, pineapple", "Small: $15.99, Medium: $18.99, Large: $21.99"),
        new("Veggie Deluxe", "Tomato sauce, mozzarella, bell peppers, mushrooms, onions, olives, tomatoes", "Small: $16.99, Medium: $19.99, Large: $22.99"),
        new("BBQ Chicken", "BBQ sauce, mozzarella, grilled chicken, red onions, cilantro", "Small: $16.99, Medium: $19.99, Large: $22.99")
    };

    private static readonly Dictionary<string, Order> Orders = new();
    private static int orderCounter = 1000;

    [McpServerTool, Description("Get the complete pizza menu with available pizzas, descriptions, and prices.")]
    public static Task<string> GetMenu()
    {
        var menuText = "🍕 **PIZZA PALACE MENU** 🍕\n\n";
        
        foreach (var item in Menu)
        {
            menuText += $"**{item.Name}**\n";
            menuText += $"   {item.Description}\n";
            menuText += $"   {item.Pricing}\n\n";
        }
        
        menuText += "📞 Call us at (555) PIZZA-01 or order online!\n";
        menuText += "🚚 Free delivery on orders over $25!";
        
        return Task.FromResult(menuText);
    }

    [McpServerTool, Description("Place a pizza order with customer details.")]
    public static Task<string> PlaceOrder(
        [Description("Pizza type (e.g., Margherita, Pepperoni, Supreme, Hawaiian, Veggie Deluxe, BBQ Chicken)")] string pizzaType,
        [Description("Size: Small, Medium, or Large")] string size,
        [Description("Customer name")] string customerName,
        [Description("Customer phone number")] string phoneNumber,
        [Description("Delivery address")] string address)
    {
        // Generate order ID
        var orderId = $"PZ{orderCounter++}";
        
        // Find the pizza in menu
        var pizza = Menu.FirstOrDefault(p => p.Name.Equals(pizzaType, StringComparison.OrdinalIgnoreCase));
        if (pizza == null)
        {
            return Task.FromResult($"❌ Sorry, '{pizzaType}' is not on our menu. Please check our menu and try again.");
        }

        // Validate size
        if (!new[] { "Small", "Medium", "Large" }.Contains(size, StringComparer.OrdinalIgnoreCase))
        {
            return Task.FromResult("❌ Invalid size. Please choose Small, Medium, or Large.");
        }

        // Create order
        var order = new Order
        {
            OrderId = orderId,
            PizzaType = pizzaType,
            Size = size,
            CustomerName = customerName,
            PhoneNumber = phoneNumber,
            Address = address,
            OrderTime = DateTime.Now,
            Status = "Confirmed",
            EstimatedDelivery = DateTime.Now.AddMinutes(30 + Random.Shared.Next(0, 20))
        };

        Orders[orderId] = order;

        var response = $"✅ **ORDER CONFIRMED!**\n\n";
        response += $"🎫 Order ID: {orderId}\n";
        response += $"🍕 Pizza: {pizzaType} ({size})\n";
        response += $"👤 Customer: {customerName}\n";
        response += $"📞 Phone: {phoneNumber}\n";
        response += $"📍 Address: {address}\n";
        response += $"⏰ Order Time: {order.OrderTime:yyyy-MM-dd HH:mm}\n";
        response += $"🚚 Estimated Delivery: {order.EstimatedDelivery:yyyy-MM-dd HH:mm}\n\n";
        response += $"💰 Total: ${GetPriceForSize(size)}\n";
        response += $"📱 Track your order with ID: {orderId}";

        return Task.FromResult(response);
    }

    [McpServerTool, Description("Check the status of an existing order by order ID.")]
    public static Task<string> CheckOrderStatus(
        [Description("Order ID to check (e.g., PZ1001)")] string orderId)
    {
        if (!Orders.TryGetValue(orderId, out var order))
        {
            return Task.FromResult($"❌ Order {orderId} not found. Please check your order ID and try again.");
        }

        // Simulate order progression
        var minutesSinceOrder = (DateTime.Now - order.OrderTime).TotalMinutes;
        string currentStatus;
        
        if (minutesSinceOrder < 5)
            currentStatus = "Order Received - Preparing your pizza! 👨‍🍳";
        else if (minutesSinceOrder < 15)
            currentStatus = "In the Oven - Your pizza is baking! 🔥";
        else if (minutesSinceOrder < 25)
            currentStatus = "Quality Check - Almost ready! ✅";
        else if (minutesSinceOrder < 35)
            currentStatus = "Out for Delivery - On the way to you! 🚚";
        else
            currentStatus = "Delivered - Enjoy your pizza! 🎉";

        var response = $"📋 **ORDER STATUS UPDATE**\n\n";
        response += $"🎫 Order ID: {orderId}\n";
        response += $"🍕 Pizza: {order.PizzaType} ({order.Size})\n";
        response += $"👤 Customer: {order.CustomerName}\n";
        response += $"📍 Address: {order.Address}\n";
        response += $"⏰ Order Time: {order.OrderTime:yyyy-MM-dd HH:mm}\n";
        response += $"🚚 Estimated Delivery: {order.EstimatedDelivery:yyyy-MM-dd HH:mm}\n\n";
        response += $"📊 **Current Status:** {currentStatus}";

        return Task.FromResult(response);
    }

    [McpServerTool, Description("Check the delivery status and estimated delivery time for an order.")]
    public static Task<string> CheckDeliveryStatus(
        [Description("Order ID to track delivery (e.g., PZ1001)")] string orderId)
    {
        if (!Orders.TryGetValue(orderId, out var order))
        {
            return Task.FromResult($"❌ Order {orderId} not found. Please check your order ID and try again.");
        }

        var minutesSinceOrder = (DateTime.Now - order.OrderTime).TotalMinutes;
        string deliveryStatus;
        string driverInfo = "";
        
        if (minutesSinceOrder < 25)
        {
            deliveryStatus = "🍕 Still preparing your order";
            var remainingTime = Math.Max(0, 25 - minutesSinceOrder);
            deliveryStatus += $"\n⏱️ Estimated prep completion: {remainingTime:F0} minutes";
        }
        else if (minutesSinceOrder < 35)
        {
            deliveryStatus = "🚚 Out for delivery!";
            driverInfo = "\n👨‍🚚 Driver: Mike Johnson\n🚗 Vehicle: Red Honda Civic (License: ABC-123)";
            var eta = Math.Max(0, 35 - minutesSinceOrder);
            deliveryStatus += $"\n⏰ ETA: {eta:F0} minutes";
            deliveryStatus += driverInfo;
        }
        else
        {
            deliveryStatus = "✅ Delivered!";
            deliveryStatus += "\n🎉 Your pizza has been delivered. Enjoy!";
        }

        var response = $"📍 **DELIVERY TRACKING**\n\n";
        response += $"🎫 Order ID: {orderId}\n";
        response += $"🍕 Pizza: {order.PizzaType} ({order.Size})\n";
        response += $"📍 Delivery Address: {order.Address}\n";
        response += $"📞 Contact: {order.PhoneNumber}\n\n";
        response += $"📊 **Delivery Status:**\n{deliveryStatus}\n\n";
        response += $"🚚 Original ETA: {order.EstimatedDelivery:HH:mm}";

        return Task.FromResult(response);
    }

    private static string GetPriceForSize(string size) => size.ToLower() switch
    {
        "small" => "14.99",
        "medium" => "17.99", 
        "large" => "20.99",
        _ => "16.99"
    };
}

// Helper classes
public record PizzaMenuItem(string Name, string Description, string Pricing);

public class Order
{
    public string OrderId { get; set; } = string.Empty;
    public string PizzaType { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime OrderTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime EstimatedDelivery { get; set; }
}