using System.Linq.Expressions;

namespace TaskTest;
class Program
{
     static async Task Main(string[] args)
    {
        Console.WriteLine("Main开始");

        await A();
        await B();
        await C();
        Console.WriteLine("Main结束");
    }


    static Task A()
    {
        Task.Delay(5000);
        Console.WriteLine("A");
        return Task.CompletedTask;
    }

    static async Task B()
    {
        await Task.Delay(5000);
        Console.WriteLine("B");
    }

    static async Task C()
    {
        await Task.Delay(5000);
        await Task.Run(() => Console.WriteLine("C"));
    }

}

