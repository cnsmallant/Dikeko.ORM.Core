using System;

namespace Dikeko.ORM.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = UserInfo.Current.GetUserById();

            Console.WriteLine(result);
        }
    }
}
