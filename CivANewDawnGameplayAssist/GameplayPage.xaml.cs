using CivANewDawnAssistModels;
using CivANewDawnAssistViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CivANewDawnGameplayAssist
{
    /// <summary>
    /// Interaction logic for GameplayPage.xaml
    /// </summary>
    public partial class GameplayPage : Page
    {
        private Frame _mainFrame;
        private GameplayViewModel _gameplayViewModel;
        private List<TextBlock> _eventTxtBlck;
        private MainWindow _mainWindow;
        public GameplayPage(MainWindow mainWindow, Frame mainFrame, CivANewDawnGameMaster gameMaster)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainWindow.SwitchToAncientMusic();
            _mainFrame = mainFrame;
            
            _gameplayViewModel = new GameplayViewModel(gameMaster);

            _eventTxtBlck = new List<TextBlock>()
            {
                txtblck_event1,
                txtblck_event2,
                txtblck_event3,
                txtblck_event4,
                txtblck_event5
            };

            txtblck_currentTurn.Text = _gameplayViewModel.CurrentTurn;
            UpdateGame();
            CheckAstronomy();
        }

        private void CheckAstronomy()
        {
            //TODO: Remove Magic Strings
            btn_astronomy.IsEnabled = _gameplayViewModel.IsAstronomyPossible;
        }

        private void UpdateGame()
        {
            if(_gameplayViewModel.EndOfEra)
            {
                if (_gameplayViewModel.CurrentEra == EventCardEra.ANCIENT)
                {
                    _mainWindow.SwitchToMedievalMusic();
                }

                if (_gameplayViewModel.CurrentEra == EventCardEra.MEDIEVAL)
                {
                    _mainWindow.SwitchToIndustrialMusic();
                }

                if (_gameplayViewModel.CurrentEra == EventCardEra.INDUSTRIAL)
                {
                    _mainWindow.SwitchToAtomicMusic();
                }

                if (_gameplayViewModel.CurrentEra == EventCardEra.ATOMIC)
                {
                    _mainWindow.SwitchToFutureMusic();
                }

                if (_gameplayViewModel.CurrentEra == EventCardEra.FUTURE)
                {
                    _mainWindow.SwitchToEndingMusic();
                }
            }
            UpdateEventCards();
        }

        private void UpdateEventCards()
        {
            for (int i = 0; i < _eventTxtBlck.Count; i++)
            {
                if (i < _gameplayViewModel.NextEventCards.Count)
                    _eventTxtBlck[i].Text = _gameplayViewModel.NextEventCards[i].Name;
                else
                    _eventTxtBlck[i].Text = string.Empty;
            }
        }

        private void btn_nextTurn_Click(object sender, RoutedEventArgs e)
        {
            txtblck_currentTurn.Text = _gameplayViewModel.NextTurn;
            UpdateGame();
            CheckAstronomy();
        }

        //private void btn_buyCultureWonder_Click(object sender, RoutedEventArgs e)
        //{
        //    string message = "Are you sure you want to purchase this culture wonder?";
        //    string title = "";
        //    MessageBoxButton buttons = MessageBoxButton.YesNo;
        //    MessageBoxResult result = MessageBox.Show(message, title, buttons);

        //    if(result == MessageBoxResult.Yes)
        //    {
        //        _gameplayViewModel.PurchaseCultureWonder();
        //        UpdateGame();
        //    }
        //}

        //private void btn_buyEconomyWonder_Click(object sender, RoutedEventArgs e)
        //{
        //    string message = "Are you sure you want to purchase this economy wonder?";
        //    string title = "";
        //    MessageBoxButton buttons = MessageBoxButton.YesNo;
        //    MessageBoxResult result = MessageBox.Show(message, title, buttons);

        //    if(result == MessageBoxResult.Yes)
        //    {
        //        _gameplayViewModel.PurchaseEconomyWonder();
        //        UpdateGame();
        //    }
        //}

        //private void btn_buyMilitaryWonder_Click(object sender, RoutedEventArgs e)
        //{
        //    string message = "Are you sure you want to purchase this military wonder?";
        //    string title = "";
        //    MessageBoxButton buttons = MessageBoxButton.YesNo;
        //    MessageBoxResult result = MessageBox.Show(message, title, buttons);

        //    if(result == MessageBoxResult.Yes)
        //    {
        //        _gameplayViewModel.PurchaseMilitaryWonder();
        //        UpdateGame();
        //    }
        //}

        //private void btn_buyScienceWonder_Click(object sender, RoutedEventArgs e)
        //{
        //    string message = "Are you sure you want to purchase this science wonder?";
        //    string title = "";
        //    MessageBoxButton buttons = MessageBoxButton.YesNo;
        //    MessageBoxResult result = MessageBox.Show(message, title, buttons);

        //    if(result == MessageBoxResult.Yes)
        //    {
        //        _gameplayViewModel.PurchaseScienceWonder();
        //        UpdateGame();
        //    }
        //}

        private void btn_drawMapTile_Click(object sender, RoutedEventArgs e)
        {
            lbl_mapTile.Content = _gameplayViewModel.GetNextMapTile();
            btn_drawMapTile.IsEnabled = !_gameplayViewModel.AreMapTilesEmpty;
            CheckAstronomy();
        }

        private void btn_astronomy_Click(object sender, RoutedEventArgs e)
        {
            var x = _gameplayViewModel.GetNextMapTile();
            var y = _gameplayViewModel.GetNextMapTile();
            btn_drawMapTile.IsEnabled = !_gameplayViewModel.AreMapTilesEmpty;
            CheckAstronomy();
            lbl_mapTile.Content = y != null ? $"{x},{y}" : x.ToString();
            btn_returnFirstMapTile.IsEnabled = y != null;
            btn_returnSecondMapTile.IsEnabled = y != null;
            btn_drawMapTile.IsEnabled = y != null;
            btn_nextTurn.IsEnabled = y != null;
        }

        private void btn_returnFirstMapTile_Click(object sender, RoutedEventArgs e)
        {
            var tokens = lbl_mapTile.Content.ToString().Split(',');
            int mapTileNum = int.Parse(tokens[0]);
            _gameplayViewModel.ReturnMapTile(mapTileNum);
            lbl_mapTile.Content = tokens[1];

            btn_returnFirstMapTile.IsEnabled = false;
            btn_returnSecondMapTile.IsEnabled = false;
            btn_drawMapTile.IsEnabled = true;
            btn_nextTurn.IsEnabled = true;
        }

        private void btn_returnSecondMapTile_Click(object sender, RoutedEventArgs e)
        {
            var tokens = lbl_mapTile.Content.ToString().Split(',');
            int mapTileNum = int.Parse(tokens[1]);
            _gameplayViewModel.ReturnMapTile(mapTileNum);
            lbl_mapTile.Content = tokens[0];

            btn_returnFirstMapTile.IsEnabled = false;
            btn_returnSecondMapTile.IsEnabled = false;
            btn_drawMapTile.IsEnabled = true;
            btn_nextTurn.IsEnabled = true;
        }
    }
}
