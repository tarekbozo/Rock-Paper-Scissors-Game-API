using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using RockPaperScissorsGame.Api.Middlewares;

public class RateLimitingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldReturn429_WhenRateLimitExceeded()
    {
        // Arrange
        var next = new Mock<RequestDelegate>();
        var context = new DefaultHttpContext();
        var middleware = new RateLimitingMiddleware(next.Object);

        var ip = "127.0.0.1";
        context.Connection.RemoteIpAddress = IPAddress.Parse(ip);

        // Act - First request (allowed)
        await middleware.InvokeAsync(context);

        // Assert - Second request (should hit rate limit)
        var response = new DefaultHttpContext();
        response.Connection.RemoteIpAddress = IPAddress.Parse(ip);

        await middleware.InvokeAsync(response);

        Assert.Equal(429, response.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenRequestIsAllowed()
    {
        // Arrange
        var next = new Mock<RequestDelegate>();
        var context = new DefaultHttpContext();
        var middleware = new RateLimitingMiddleware(next.Object);

        var ip = "127.0.0.1";
        context.Connection.RemoteIpAddress = IPAddress.Parse(ip);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        next.Verify(n => n(context), Times.Once);
    }
}
