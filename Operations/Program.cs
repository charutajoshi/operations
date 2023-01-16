using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapRazorPages();

app.Run(async (HttpContext context) =>
{
    StreamReader reader = new StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();
    Dictionary<string, StringValues> queryDictionary = QueryHelpers.ParseQuery(body);

    if (queryDictionary.ContainsKey("firstNumber"))
    {
        var firstNum = queryDictionary["firstNumber"];

        if (queryDictionary.ContainsKey("secondNumber"))
        {
            var secondNum = queryDictionary["secondNumber"];

            if (queryDictionary.ContainsKey("operation"))
            {
                var operation = queryDictionary["operation"];

                switch(operation)
                {
                    case "add":
                        context.Response.WriteAsync($"The sum of {firstNum} and {secondNum} is {Int32.Parse(firstNum) + Int32.Parse(secondNum)}");
                        break;
                    case "subtract":
                        context.Response.WriteAsync($"The difference between {firstNum} and {secondNum} is {Int32.Parse(firstNum) - Int32.Parse(secondNum)}");
                        break;
                    case "multiply":
                        context.Response.WriteAsync($"The product of {firstNum} and {secondNum} is {Int32.Parse(firstNum) * Int32.Parse(secondNum)}");
                        break;
                    case "divide":
                        context.Response.WriteAsync($"The quotient of {firstNum} and {secondNum} is {Int32.Parse(firstNum) / Int32.Parse(secondNum)}");
                        break;
                    default:
                        context.Response.WriteAsync($"Invalid input for 'operation'");
                        break;
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                string response = "Invalid input for 'operation'";
                context.Response.WriteAsync(response);
            }
        }
        else
        {
            context.Response.StatusCode = 400;
            string response = "Invalid input for 'secondNumber'";
            context.Response.WriteAsync(response);
        }
    }
    else
    {
        context.Response.StatusCode = 400;
        string response = "Invalid input for 'firstNumber'";
        context.Response.WriteAsync(response);
    }
});

app.Run();

