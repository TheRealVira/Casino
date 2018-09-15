using System;
using System.Collections.Generic;

namespace Casino8
{
    public static class Casino
    {
        public static void Initialize(Random rand)
        {
            _rand = rand;
        }

        private static Random _rand;

        private static readonly List<Player> Lobby = new List<Player>();

        public static void Join(Player player)
        {
            if (player == null) return;
            Lobby.Add(player);
        }

        public static int Update()
        {
            var hasNoMovesMoreLeft = new List<Player>();
            foreach (var player in Lobby)
            {
                player.Steps.Dequeue().Play(player).UpdatePlayer();
                if (player.Steps.Count == 0)
                {
                    hasNoMovesMoreLeft.Add(player);
                }
            }

            Lobby.RemoveAll(x => x.IsBleite || hasNoMovesMoreLeft.Contains(x));

            return Lobby.Count;
        }

        public static bool LobbyIsEmpty=> Lobby.Count == 0;

        private static void UpdatePlayer(this (Player, int) move)
        {
            move.Item1.Update(move.Item2);
        }

        private static (Player, int) Play(this (char, int) move, Player player)
        {
            try
            {
                return int.Parse(move.Item1.ToString()) == _rand.Next(8) ? (player, move.Item2 * 7) : (player, 0);
            }
            catch
            {
                return move.Item1.ToString().ToUpper().Equals("G") && _rand.Next(8) % 2 == 0
                    ? (player, move.Item2 * 2)
                    : (player, 0);
            }
        }
    }
}