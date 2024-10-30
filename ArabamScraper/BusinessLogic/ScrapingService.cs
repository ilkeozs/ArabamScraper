using HtmlAgilityPack;
using System.Globalization;

namespace ArabamScraper.BusinessLogic
{
    public class ScrapingService
    {
        public async Task<List<string>> ScrapePrices(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var prices = new List<string>();

            var priceNodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='db no-wrap listing-price']");

            if (priceNodes != null)
            {
                foreach (var node in priceNodes)
                {
                    prices.Add(node.InnerText.Trim());
                }
            }

            return prices;
        }

        public void CalculateAndSaveStatistics(string filePath, List<string> prices)
        {
            int totalPrices = prices.Count;
            double sum = 0;

            foreach (var price in prices)
            {
                string cleanedPrice = price.Replace("TL", "").Replace(".", "").Trim();
                if (double.TryParse(cleanedPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out double priceValue))
                {
                    sum += priceValue;
                }
            }

            double averagePrice = totalPrices > 0 ? sum / totalPrices : 0;

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine();
                writer.WriteLine($"Total Number of Ads: {totalPrices}");
                writer.WriteLine($"Total Sum of Prices: {sum:N2} TL");
                writer.Write($"Arithmetic Average: {averagePrice:N2} TL");
            }
        }
    }
}