using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private IOrderRepository repository;
    private Cart cart;

    public OrderController(IOrderRepository repositoryService, Cart cartService)
    {
        repository = repositoryService;
        cart = cartService;
    }
    public ViewResult Checkout() => View(new Order());

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (!cart.Lines.Any())
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }

        if (ModelState.IsValid)
        {
            order.Lines = cart.Lines.ToArray();
            repository.SaveOrder(order);
            cart.Clear();
            return RedirectToPage("/Completed", new {orderId = order.OrderID});
        }

        return View();
    }
}