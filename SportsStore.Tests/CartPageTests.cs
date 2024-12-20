﻿using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;
using SportsStore.Pages;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace SportsStore.Tests;

public class CartPageTests
{
    [Fact]
    public void Can_Load_Cart()
    {
        // Arrange
        // - create mock repo
        Product p1 = new Product { ProductID = 1, Name = "P1" };
        Product p2 = new Product { ProductID = 2, Name = "P2" };
        Mock<IStoreRepository> mockRepo = new ();
        mockRepo.Setup(m => m.Products).Returns(new[]
        {
            p1, p2
        }.AsQueryable());
        
        // - create test cart
        Cart testCart = new();
        testCart.AddItem(p1, 2);
        testCart.AddItem(p2, 1);
        
        CartModel cartModel = new(mockRepo.Object, testCart);
        cartModel.OnGet("myUrl");
        
        // Assert
        Assert.Equal(2, cartModel.Cart?.Lines.Count);
        Assert.Equal("myUrl", cartModel.ReturnUrl);
    }
    
    [Fact]
    public void Can_Update_Cart() 
    {
        // Arrange
        // - create a mock repository
        Mock<IStoreRepository> mockRepo =
            new Mock<IStoreRepository>();
        mockRepo.Setup(m => m.Products).Returns((new[] {
            new Product { ProductID = 1, Name = "P1" }
        }).AsQueryable<Product>());
        
        Cart testCart = new();
        
        // Act
        CartModel cartModel = new(mockRepo.Object, testCart);
        cartModel.OnPost(1, "myUrl");
        //Assert
        Assert.Single(testCart.Lines);
        Assert.Equal("P1", testCart.Lines.First().Product.Name);
        Assert.Equal(1, testCart.Lines.First().Quantity);
    }
}