using System;
using System.Collections.Generic;

namespace Casino8
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Casino.Initialize(new Random(DateTime.Now.Millisecond));
            
            var player1 = new Player("Player\\Spieler1.conf");
            Casino.Join(player1);
            
            while (!Casino.LobbyIsEmpty)
            {
                Casino.Update();
            }
        }
    }
}