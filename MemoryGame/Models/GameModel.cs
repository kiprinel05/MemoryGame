using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryGame.Models
{
    public class GameModel
    {
        public List<Card> Cards { get; set; }
        public int TotalPairs { get; set; }
        public int MatchedPairs { get; set; }
        public TimeSpan TimeLeft { get; set; }

        public GameModel(int rows, int columns)
        {
            Cards = new List<Card>();
            TotalPairs = (rows * columns) / 2;
            MatchedPairs = 0;
            TimeLeft = TimeSpan.FromMinutes(1); // Timpul rămas, de exemplu 1 minut
        }

        public void StartNewGame()
        {
            Cards.Clear();
            var allCards = new List<Card>();
            var images = new List<string> { "image1.png", "image2.png", "image3.png" }; // Exemplu de imagini, adaugă căi reale

            // Creăm perechi de cărți
            foreach (var image in images)
            {
                allCards.Add(new Card { Image = image });
                allCards.Add(new Card { Image = image });
            }

            // Amestecăm cărțile
            var random = new Random();
            Cards = allCards.OrderBy(x => random.Next()).ToList();
        }

        public bool CheckMatch(int firstIndex, int secondIndex)
        {
            if (Cards[firstIndex].Image == Cards[secondIndex].Image)
            {
                MatchedPairs++;
                return true;
            }
            return false;
        }
    }

    public class Card
    {
        public string Image { get; set; }
        public bool IsFlipped { get; set; }
    }
}
