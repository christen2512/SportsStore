using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests
{
    [Fact]
    public void Can_Select_Categories()
    {
        //Arrange
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[]
        {
            new() {ProductID = 1, Name = "P1", Category = "Apples"},
            new() {ProductID = 2, Name = "P2", Category = "Apples"},
            new() {ProductID = 3, Name = "P3", Category = "Plums"},
            new() {ProductID = 4, Name = "P4", Category = "Oranges"}
        }).AsQueryable());

        NavigationMenuViewComponent target = new(mock.Object);
        
        //Act = get the set of categories
        string[] results = ((IEnumerable<string>?)(target.Invoke()
                                as ViewViewComponentResult)?.ViewData?.Model
                            ?? Enumerable.Empty<string>()).ToArray();
        
        //Assert
        Assert.True(Enumerable.SequenceEqual(new string[] {"Apples", "Oranges", "Plums"}, results));
    }

    [Fact]
    public void Indicates_Selected_Category()
    {
        //Arrange
        string categoryToSelect = "Apples";
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new () {ProductID = 1, Name = "P1", Category = "Apples"},
            new () {ProductID = 1, Name = "P1", Category = "Oranges"}
        }.AsQueryable());
        
        NavigationMenuViewComponent target = new(mock.Object);
        target.ViewComponentContext = new ViewComponentContext {
            ViewContext = new ViewContext {
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            }
        };
        target.RouteData.Values["category"] = categoryToSelect;
        
        //Action
        string? result = (string?)(target.Invoke() 
            as ViewViewComponentResult)?.ViewData?["SelectedCategory"];
        
        //Assert
        Assert.Equal(categoryToSelect, result);
    }
}