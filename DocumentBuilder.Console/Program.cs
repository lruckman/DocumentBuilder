using DocumentBuilder.Models;

namespace DocumentBuilder.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var document = Factory.GenerateClientCopy(new Request
            {
                Locations = new[]
                {
                    new Location(),
                    new Location()
                }
            });

            System.Console.WriteLine("Path {0}", document.Path);
            System.Console.WriteLine("Pages {0}", document.PageCount);

            System.Console.ReadLine();
        }
    }
}