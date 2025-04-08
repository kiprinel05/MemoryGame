using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GuessGame.Models;
using GuessGame.Services;

namespace GuessGame.Views
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var stats = StatisticsService.LoadStatistics();

            var displayStats = stats.Select(s => new
            {
                s.UserName,
                s.GamesPlayed,
                s.GamesWon,
                WinRate = s.GamesPlayed > 0
                    ? string.Format("{0:P1}", (double)s.GamesWon / s.GamesPlayed)
                    : "0%"
            }).ToList();

            StatsGrid.ItemsSource = displayStats;
        }
    }
}