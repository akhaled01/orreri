using System.Globalization;
using CsvHelper;


public class Program
{

    static List<NEORecord> ReadCSV()
    {
        // Path to the CSV file
        string csvFilePath = "./raw_data.csv";

        // List to store NEO data
        List<NEORecord> neoList = [];

        // Read and parse the first 10,000 lines of the CSV
        using var reader = new StreamReader(csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        // Limit the number of records to read
        // int maxRecords = 1000000;
        int count = 0;

        while (csv.Read())
        {
            var neo = csv.GetRecord<NEORecord>();
            neoList.Add(neo);
            count++;
        }

        return neoList;
    }


    public static void Main()
    {

        List<NEOVelocityRecord> outputRecords = [];
        List<NEORecord> neoList = ReadCSV();

        foreach (var neo in neoList)
        {
            // if near earth, calculate velocity
            if (neo.neo != "N" || neo.pha != "N")
            {
                Console.WriteLine($"NEO Name: {neo.name}, Diameter: {neo.diameter} km, Neo Status: {neo.neo}, PHA: {neo.pha}");
                outputRecords.Add(VelocityCalculator.NEORecordVelocity(neo));
            }
            // else
            // {
            //     outputRecords.Add(new NEOVelocityRecord(neo.name ?? "unknown", 0, 0, 0));
            // }
        }

        new NeoVelocityOutputter("./output_data.csv").WriteRecords(outputRecords);
    }  
}