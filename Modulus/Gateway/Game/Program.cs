using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Game
{
    class Program
    {
        static void Main(string[] args)
        {

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();


            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
