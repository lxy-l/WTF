
User user1 = new() { Id=1,Name="AAA"};
User user2 = (User)user1.Clone();
user2.Id = 2;
user2.Name = "你好,Hello";
Console.WriteLine(user2.Name.GetUnicodeByteCount());
Console.WriteLine(user2.Name.ToUnicode());
Console.WriteLine(user2.Name.ToUnicodeBytes());
Console.WriteLine(user2.Name.Length);
Console.WriteLine("User1:"+user1.ToString());
Console.WriteLine("User2:"+user2.ToString());























static class StringExt
{
    public static long GetUnicodeByteCount(this string str)
    {
        return System.Text.Encoding.Unicode.GetByteCount(str);
    }
    public static string ToUnicode(this string str)
    {
        byte[] bytes =System.Text.Encoding.Unicode.GetBytes(str);
        return System.Text.Encoding.Unicode.GetString(bytes);
    }

    public static byte[] ToUnicodeBytes(this string str)
    {
       return System.Text.Encoding.Unicode.GetBytes(str);
    }
}




class User:ICloneable
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public override string ToString()=>"Id:"+Id+"; Name:"+Name;
}
