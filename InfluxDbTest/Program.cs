using System.Diagnostics;

using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;

const string token = "kCZxIR5O_zrcK5SYDIgohYJRY-9GEj4yN0syBNlPHF3VKH3SfAnCl_Acy1hf1R0Esu4kK8xyaBwN10WShh_NuA==";
const string bucket = "database";
const string org = "roy";

using var client = InfluxDBClientFactory.Create("http://localhost:49153", token);

//using (var writeApi = client.GetWriteApi())
//{
//    //var point = new PointData
//    //          .Measurement("data3")
//    //          .Tag("host", "host2")
//    //          .Field("value", i)
//    //          .Timestamp(DateTime.UtcNow.AddMilliseconds(i), WritePrecision.Ns);

//    //writeApi.WritePoint(point, bucket, org);
//}

//using (var writeApi = client.GetWriteApi())
//{
//    for (int i = 0; i < 1000000; i++)
//    {
//        string data = $"mem,host=host6 value={i}";

//        writeApi.WriteRecord(bucket, WritePrecision.Ns, data, org);

//    }
//}
//Console.WriteLine("OK");

using (var writeApi = client.GetWriteApi())
{
    List<Mem?> mems = new List<Mem?>();
    try
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            var mem = new Mem { Host = "host1", Value = i, Time = DateTime.UtcNow.AddMilliseconds(i) };
            mems.Add(mem);
            //writeApi.WriteMeasurement(mem, WritePrecision.Ms, bucket, org);
        }

        writeApi.WriteMeasurements(mems, WritePrecision.Ms, bucket, org);
        stopwatch.Stop();
        Console.WriteLine("插入耗时：" + stopwatch.ElapsedMilliseconds);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

var query = "from(bucket: \"database\") " +
    "|> range(start: -1h) " +
    "|> filter(fn: (r) => r[\"_measurement\"] == \"data\") " +
    "|> filter(fn: (r) => r[\"_field\"] == \"value\")";


var tables = await client.GetQueryApi().QueryAsync(query, org);

//foreach (var record in tables.SelectMany(table => table.Records))
//{
//    Console.WriteLine($"{record.Values.Values}");
//}

[Measurement("data2")]
class Mem
{
    [Column("host", IsTag = true)] public string? Host { get; set; }
    [Column("value")] public long Value { get; set; }
    [Column(IsTimestamp = true)] public DateTime Time { get; set; }
}
