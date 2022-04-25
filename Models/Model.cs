namespace TimePolling;

public class Model
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }

    private static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static List<Model> GenerateRandom()
    {
        var list = new List<Model>();
        for (var i = 0; i < 10; i++)
            list.Add(new Model
            {
                Key = RandomString(10),
                Value = RandomString(10)
            });
        return list;
    }
}