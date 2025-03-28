using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : ObservableObject
    {
        private GameModel _gameModel;
        private ObservableCollection<Card> _cards;
        private Card _firstCard;
        private Card _secondCard;
        private bool _isGameInProgress;

        public ObservableCollection<Card> Cards
        {
            get => _cards;
            set => SetProperty(ref _cards, value);
        }

        public string TimeLeft => _gameModel.TimeLeft.ToString(@"mm\:ss");

        public bool IsGameInProgress
        {
            get => _isGameInProgress;
            set => SetProperty(ref _isGameInProgress, value);
        }

        public GameViewModel()
        {
            _gameModel = new GameModel(4, 4); // Joc 4x4
            _cards = new ObservableCollection<Card>(_gameModel.Cards);
            StartNewGameCommand = new RelayCommand(StartNewGame);
            FlipCardCommand = new RelayCommand<int>(FlipCard);
        }

        public IRelayCommand StartNewGameCommand { get; }
        public IRelayCommand<int> FlipCardCommand { get; }

        private void StartNewGame()
        {
            _gameModel.StartNewGame();
            Cards = new ObservableCollection<Card>(_gameModel.Cards);
            _firstCard = null;
            _secondCard = null;
            _isGameInProgress = true;
        }

        private void FlipCard(int index)
        {
            if (!_isGameInProgress || Cards[index].IsFlipped)
                return;

            Cards[index].IsFlipped = true;

            if (_firstCard == null)
            {
                _firstCard = Cards[index];
                return;
            }

            _secondCard = Cards[index];

            if (_gameModel.CheckMatch(Cards.IndexOf(_firstCard), Cards.IndexOf(_secondCard)))
            {
                if (_gameModel.MatchedPairs == _gameModel.TotalPairs)
                {
                    // Jocul s-a terminat cu succes
                    _isGameInProgress = false;
                }
            }
            else
            {
                // Nu se potrivesc, întoarcem cărțile
                var tempFirstCard = _firstCard;
                var tempSecondCard = _secondCard;
                _firstCard = null;
                _secondCard = null;

                // Întoarcerea cărților la câteva secunde
                System.Threading.Tasks.Task.Delay(500).ContinueWith(t =>
                {
                    tempFirstCard.IsFlipped = false;
                    tempSecondCard.IsFlipped = false;
                });
            }
        }
    }
}
