﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStore.Tests;

public class OrderControllerTests
{
    [Fact]
    public void Cannot_Checkout_Empty_Cart()
    {
        // Arrange
        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        Order order = new();
        OrderController target = new(mock.Object, cart);
        
        // Act
        ViewResult? result = target.Checkout(order) as ViewResult;
        
        // Assert - check that the order hasn't been stored
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never());
        // Assert - check that the method is returning the default view
        Assert.True(string.IsNullOrEmpty(result?.ViewName));
        // Assert - check I am passing an invalid model to the view
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void Cannot_Checkout_Invalid_ShippingDetails()
    {
        // Arrange - create a mock order repository
        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        cart.AddItem(new Product(), 1);
        OrderController target = new(mock.Object, cart);
        target.ModelState.AddModelError("error", "error");
        
        // Act
        ViewResult? result = target.Checkout(new Order()) as ViewResult;
        
        // Assert
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never());
        Assert.True(string.IsNullOrEmpty(result?.ViewName));
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void Can_Checkout_And_Submit_Order()
    {
        // Arrange
        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        cart.AddItem(new Product(), 1);
        OrderController target = new(mock.Object, cart);
        
        // Act
        RedirectToPageResult? result = target.Checkout(new Order()) as RedirectToPageResult;
        
        // Assert
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once());
        Assert.Equal("/Completed", result?.PageName);
    }
}