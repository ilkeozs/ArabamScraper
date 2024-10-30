namespace ArabamScraper.DataAccess
{
    public class FileService
    {
        public void SavePricesToFile(string filePath, List<string> prices)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var price in prices)
                {
                    writer.WriteLine(price);
                }
            }
        }
    }
}