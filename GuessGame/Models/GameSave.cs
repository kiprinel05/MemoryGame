using System;
using System.Collections.Generic;

namespace GuessGame.Models
{
    public class GameSave
    {
        public string UserName { get; set; }
        public string Category { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<string> TileImages { get; set; }
        public List<bool> RevealedTiles { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public DateTime SaveTime { get; set; }
    }
}