using System;
using System.Collections.Generic;
using System.IO;

namespace Casino8
{
    public class Player
    {
        public delegate void GehtBleite(DateTime dateTime);
        public event GehtBleite IstBleiteGegangen;

        public Player(string fileName)
        {
            _fileName = fileName;
            using (var reader = new StreamReader(fileName))
            {
                Namen = reader.ReadLine();
                var guthaben = reader.ReadLine();
                if (guthaben != null) Guthaben = int.Parse(guthaben.Split(' ')[1]);
                var steps = new Queue<(char, int)>();

                string line;
                while ((line = reader.ReadLine()) != "ENDE")
                {
                    if (line != null) steps.Enqueue((line.Split(' ')[0][0], int.Parse(line.Split(' ')[1])));
                }

                Steps = steps;
            }
            
            IstBleiteGegangen+= OnEsIstEtwasPassiert;
        }

        private void OnEsIstEtwasPassiert(DateTime dateTime)
        {
            using (var writer = new StreamWriter(_fileName, true))
            {
                writer.WriteLine();
                writer.WriteLine($"{dateTime.ToShortDateString()} {dateTime.ToShortTimeString()}Uhr {ToString()}");
            }
        }

        public int Guthaben { get; private set; }
        public string Namen { get; }
        public Queue<(char, int)> Steps { get; }
        private readonly string _fileName;

        public bool IsBleite { get; private set; }

        public (Player, int) Setzen(int betrag)
        {
            return IsBleite ? (null, 0) : (this, Guthaben -= betrag);
        }

        public void Update(int betragToAdd)
        {
            Guthaben += betragToAdd;

            if (Guthaben == 0)
            {
                IsBleite = true;
                IstBleiteGegangen?.Invoke(DateTime.Now);
                return;
            }

            if (Steps.Count == 0)
            {
                OnEsIstEtwasPassiert(DateTime.Now);
            }
        }

        public override string ToString()
        {
            return IsBleite ? $"Spieler {Namen} pleite" : $"Guthaben {this.Guthaben} Euro";
        }
    }
}