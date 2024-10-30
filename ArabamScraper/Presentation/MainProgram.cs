using ArabamScraper.BusinessLogic;
using ArabamScraper.DataAccess;

namespace ArabamScraper.Presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string title = @"
  ___            _                     _____                                
 / _ \          | |                   /  ___|                               
/ /_\ \_ __ __ _| |__   __ _ _ __ ___ \ `--.  ___ _ __ __ _ _ __   ___ _ __ 
|  _  | '__/ _` | '_ \ / _` | '_ ` _ \ `--. \/ __| '__/ _` | '_ \ / _ \ '__|
| | | | | | (_| | |_) | (_| | | | | | /\__/ / (__| | | (_| | |_) |  __/ |   
\_| |_/_|  \__,_|_.__/ \__,_|_| |_| |_\____/ \___|_|  \__,_| .__/ \___|_|   
                                                           | |              
                                                           |_|              
";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(title);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The program can scrape a maximum of 2500 (50 pages, 50 ads) ad data.\nThe number of pages must be at least 1.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nPage URL: ");
            string baseUrl = Console.ReadLine();
            Console.ResetColor();

        totalPages:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Number of pages: ");
            if (!int.TryParse(Console.ReadLine(), out int totalPages) || totalPages <= 0 || totalPages > 50)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid page number entered!\n");
                Console.ResetColor();
                goto totalPages;
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nThe operation is starting!\nThe operation may take some time depending on the data size.\nPlease wait...");
            Console.ResetColor();

            ScrapingService scrapingService = new ScrapingService();
            List<string> allPrices = new List<string>();

            string take = "?take=50";

            for (int page = 1; page <= totalPages; page++)
            {
                string url = $"{baseUrl}{take}&page={page}";
                List<string> prices = await scrapingService.ScrapePrices(url);
                allPrices.AddRange(prices);
            }

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "arabam_scraper.txt");
            FileService fileService = new FileService();
            fileService.SavePricesToFile(filePath, allPrices);
            scrapingService.CalculateAndSaveStatistics(filePath, allPrices);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n\nOperation completed successfully!\nFile saved to: {filePath}");
            Console.ResetColor();
        }
    }
}