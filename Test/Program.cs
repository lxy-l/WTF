
User user1 = new() { Id=1,Name="AAA"};
User user2 = (User)user1.Clone();
user2.Id = 2;
user2.Name = "BBB";
Console.WriteLine("User1:"+user1.ToString());
Console.WriteLine("User2:"+user2.ToString());



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
