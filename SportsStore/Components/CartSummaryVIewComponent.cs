using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class CartSummaryVIewComponent : ViewComponent
{
    private Cart cart;

    public CartSummaryVIewComponent(Cart cartService)
    {
        cart = cartService;
    }

    public IViewComponentResult Invoke() =>
        View(cart);
}