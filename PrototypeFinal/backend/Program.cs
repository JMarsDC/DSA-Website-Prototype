using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors();

BookingLinkedList bookingList = new BookingLinkedList();

app.MapGet("/", () => "🎬 YayMovies Backend is running!");

app.MapPost("/api/storeBooking", async (HttpContext context) =>
{
    try
    {
        var booking = await JsonSerializer.DeserializeAsync<Booking>(context.Request.Body);

        if (booking == null)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid booking data.");
            return;
        }

        bookingList.AddBooking(booking);
        bookingList.DisplayAll();

        await context.Response.WriteAsync($"Stored booking for {booking.name}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error: " + ex.Message);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal Server Error.");
    }
});
app.Run("http://localhost:5501");

public class Booking
{
    public string? name { get; set; }
    public string? seat { get; set; }
    public string? movieTitle { get; set; }
    public double cost { get; set; }
}

public class BookingNode
{
    public Booking Data { get; set; }
    public BookingNode? Next { get; set; }

    public BookingNode(Booking Data)
    {
        this.Data = Data;
        Next = null;
    }
}

public class BookingLinkedList
{
    private BookingNode? head = null;
    private BookingNode? tail = null;

    public void AddBooking(Booking booking)
    {
        var newNode = new BookingNode(booking);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }

        Console.WriteLine($"✅ Booking added: {booking.name} | seat: {booking.seat} | Movie: {booking.movieTitle} | cost: {booking.cost}");
    }

    public void DisplayAll()
    {
        Console.WriteLine("🎟️ All Bookings:");
        BookingNode? temp = head;
        while (temp != null)
        {
            Console.WriteLine($"→ {temp.Data.name}, seat {temp.Data.seat}, Movie {temp.Data.movieTitle}, cost: {temp.Data.cost}");
            temp = temp.Next;
        }
    }
}