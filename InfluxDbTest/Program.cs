using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;

const string token = "kCZxIR5O_zrcK5SYDIgohYJRY-9GEj4yN0syBNlPHF3VKH3SfAnCl_Acy1hf1R0Esu4kK8xyaBwN10WShh_NuA==";
const string bucket = "database";
const string org = "roy";

using var client = InfluxDBClientFactory.Create("http://localhost:49153", token);


const string data = "mem,host=host1 used_percent=23.43234543";
using (var writeApi = client.GetWriteApi())
{
    writeApi.WriteRecord(bucket, WritePrecision.Ns, data, org);
}

var point = PointData
  .Measurement("mem")
  .Tag("host", "host1")
  .Field("used_percent", 23.43234543)
  .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

using (var writeApi = client.GetWriteApi())
{
    writeApi.WritePoint(point,bucket, org);
}

var mem = new Mem { Host = "host1", UsedPercent = 23.43234543, Time = DateTime.UtcNow };

using (var writeApi = client.GetWriteApi())
{
    writeApi.WriteMeasurement(mem,  WritePrecision.Ns,bucket,org);
}

var query = "from(bucket: \"database\") |> range(start: -1h)";
var tables = await client.GetQueryApi().QueryAsync(query, org);

foreach (var record in tables.SelectMany(table => table.Records))
{
    Console.WriteLine($"{record}");
}

[Measurement("mem")]
class Mem
{
    [Column("host", IsTag = true)] public string Host { get; set; }
    [Column("used_percent")] public double? UsedPercent { get; set; }
    [Column(IsTimestamp = true)] public DateTime Time { get; set; }
}
