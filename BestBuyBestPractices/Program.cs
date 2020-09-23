using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BestBuyBestPractices
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            IDbConnection conn = new MySqlConnection(config.GetConnectionString("DefaultConnection"));

            bool quit = false;
            do
            {

                Console.WriteLine("\nNew Department:    D,    New Product:     P\n" +
                                  "Remove Department: X,    Update Product:  U\n" +
                                  "Quit:              Q,    Remove Product:  R\n");

                switch (Console.ReadLine().ToLower())
                {
                    case "d":
                    {
                        var repo = new DapperDepartmentRepository(conn);

                        Console.WriteLine("Enter a new department name:");

                        var newDepartment = Console.ReadLine();

                        repo.InsertDepartment(newDepartment);
                        Console.WriteLine();

                        foreach (var item in repo.GetAllDepartments())
                            Console.WriteLine(item.Name);

                    } break;

                    case "x":
                    {
                        var repo = new DapperDepartmentRepository(conn);
                        foreach (var item in repo.GetAllDepartments())
                            Console.WriteLine(item.Name);
                          
                        Console.WriteLine("\nEnter name of Department to remove:");
                        string temp = Console.ReadLine();
                        if(null == repo.GetDepartment(temp))
                        {
                            Console.WriteLine("Invalid Department Name");
                            continue;
                        }

                        Console.WriteLine($"Deleting {temp} department\n");
                        repo.DeleteDepartment(temp);
                        foreach (var item in repo.GetAllDepartments())
                            Console.WriteLine(item.Name);
                    } break;

                    case "q": quit = true; break;

                    case "p":
                    {
                        Console.WriteLine("Add a new product (category = \'Other\'):");
                        var otherName = Console.ReadLine();
                        Console.WriteLine("Price:");
                        var otherPrice = Double.Parse(Console.ReadLine());

                        var prodRepo = new DapperProductRepository(conn);
                        prodRepo.CreateProduct(otherName, otherPrice, 10);

                        Console.WriteLine($"\n\nFind new {otherName} at the bottom:");
                        foreach (var item in prodRepo.GetAllProducts())
                            Console.WriteLine($"{item.Name}: ${item.Price}, ID {item.ProductID}");
                    } break;

                    case "u":
                    {
                        var prodRepo = new DapperProductRepository(conn);
                        foreach (var item in prodRepo.GetAllProducts())
                            Console.WriteLine($"{item.Name}: ${item.Price}, ID {item.ProductID}");
                        Console.WriteLine("\nProduct ID to change:");

                        int changeProductID;
                        if (Int32.TryParse(Console.ReadLine(), out changeProductID) == false)
                        {
                            Console.WriteLine("Invalid input");
                            continue;
                        }
                        
                        var queryProduct = new Product();
                        queryProduct = prodRepo.GetProduct(changeProductID);
                        if (queryProduct == null)
                        {
                            Console.WriteLine("Invalid ProductID");
                            continue;
                        }

                        Console.WriteLine($"New name (or just <Enter> to keep '{queryProduct.Name}'):");
                        var temp = Console.ReadLine();
                        var otherName = temp == "" ? queryProduct.Name : temp;

                        Console.WriteLine($"New price (or just <Enter> to keep {queryProduct.Price}):");
                        temp = Console.ReadLine();

                        double otherPrice;
                        if (temp == "")
                            otherPrice = queryProduct.Price;
                        else
                            otherPrice = double.Parse(temp);

                        prodRepo.UpdateProduct(otherName, otherPrice, changeProductID);

                        Console.WriteLine($"{queryProduct.Name}: ${queryProduct.Price}, ID {changeProductID} changed to");
                        queryProduct = prodRepo.GetProduct(changeProductID);
                        Console.WriteLine($"{queryProduct.Name}: ${queryProduct.Price}, ID {changeProductID}");
                    } break;

                    case "r":
                    {
                        var prodRepo = new DapperProductRepository(conn);
                        foreach (var item in prodRepo.GetAllProducts())
                            Console.WriteLine($"{item.Name}: ${item.Price}, ID {item.ProductID}");
                        Console.WriteLine("\nProduct ID to remove:");

                        int changeProductID;
                        if (Int32.TryParse(Console.ReadLine(), out changeProductID) == false)
                        {
                            Console.WriteLine("Invalid input");
                            continue;
                        }

                        var queryProduct = new Product();
                        queryProduct = prodRepo.GetProduct(changeProductID);
                        if (queryProduct == null)
                        {
                            Console.WriteLine("Invalid ProductID");
                            continue;
                        }

                        Console.WriteLine("Removing product:\n" +
                            $"{queryProduct.Name}: ${queryProduct.Price}, ID {queryProduct.ProductID}\n");
                        prodRepo.RemoveProduct(changeProductID);

                        var salesRepo = new DapperSalesRepository(conn);
                        if (salesRepo.GetSalesItem(changeProductID).Count() != 0)
                        {
                            Console.WriteLine("Removing from Sales");
                            salesRepo.RemoveProduct(changeProductID);
                        }
                        else
                            Console.WriteLine("Not found in Sales");

                        var reviewsRepo = new DapperReviewsRepository(conn);
                        if (reviewsRepo.GetReviewItem(changeProductID).Count() != 0)
                        {
                            Console.WriteLine("Removing from Reviews");
                            reviewsRepo.RemoveProduct(changeProductID);
                        }
                        else
                            Console.WriteLine("Not found in Reviews");

                        foreach (var item in prodRepo.GetAllProducts())
                            Console.WriteLine($"{item.Name}: ${item.Price}, ID {item.ProductID}");
                    } break;

                    default: quit = true; break;
                }
            }
            while (quit == false);
        }
    }
}
