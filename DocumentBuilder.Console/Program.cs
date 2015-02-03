using DocumentBuilder.Dossiers;
using DocumentBuilder.Models;

namespace DocumentBuilder.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var document = Factory.Generate<ClientCopy>(new RouteContext
            {
                Order = new Order
                {
                    OrderLocations = new[] {new OrderLocation()}
                }
            });

            System.Console.WriteLine("Path {0}", document.Path);
            System.Console.WriteLine("Pages {0}", document.PageCount);

            System.Console.ReadLine();
        }
    }
}