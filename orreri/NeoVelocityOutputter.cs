using System.Globalization;
using CsvHelper;

public class NeoVelocityOutputter(string filePath)
{
    public string FilePath { get; } = filePath;

    public void WriteRecords(List<NEOVelocityRecord> records)
    {
        using (var writer = new StreamWriter(FilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            // Write headers
            csv.WriteField("Name");
            csv.WriteField("VelX");
            csv.WriteField("VelY");
            csv.WriteField("VelZ");
            csv.WriteField("pha");
            csv.WriteField("mass");
            csv.NextRecord();

            // Write each record
            foreach (var record in records)
            {
                csv.WriteField(record.Name);
                csv.WriteField(record.VelX);
                csv.WriteField(record.VelY);
                csv.WriteField(record.VelZ);
                csv.WriteField(record.dangerous);
                csv.WriteField(record.mass);
                csv.NextRecord();
            }
        }

        Console.WriteLine($"CSV file '{FilePath}' created successfully!");
    }
}
