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
    /// Interaction logic for GeneralSetupPage.xaml
    /// </summary>
    public partial class GeneralSetupPage : Page
    {
        private Frame _mainFrame;
        private GeneralSetupViewModel _generalSetupViewModel;
        private List<Label> _lblCapitols;
        private List<Label> _lblMapTiles;
        private List<SolidColorBrush> _brushes;
        private List<Label> _lblVictoryCards;
        private MainWindow _mainWindow;
        public GeneralSetupPage(MainWindow mainWindow, Frame mainFrame, CivANewDawnGameMaster gameMaster)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainFrame = mainFrame;
            _generalSetupViewModel = new GeneralSetupViewModel(gameMaster);

            _brushes = new List<SolidColorBrush>()
            {
                Brushes.Red,
                Brushes.Orange,
                Brushes.Lime,
                Brushes.Cyan,
                Brushes.Purple
            };

            _lblCapitols = new List<Label>()
            {
                lbl_player1CapitolTile,
                lbl_player2CapitolTile,
                lbl_player3CapitolTile,
                lbl_player4CapitolTile,
                lbl_player5CapitolTile
            };

            _lblVictoryCards = new List<Label>()
            {
                lbl_victoryCard1,
                lbl_victoryCard2,
                lbl_victoryCard3,
                lbl_victoryCard4
            };

            _lblMapTiles = new List<Label>()
            {
                lbl_mapTile1,
                lbl_mapTile2,
                lbl_mapTile3,
                lbl_mapTile4
            };

            for(int i = 0; i < _generalSetupViewModel.Players.Count; i++)
            {
                var player = _generalSetupViewModel.Players[i];
                _lblCapitols[i].Foreground = _brushes[(int)player.PlayerColor];
                int capitolTile = player.StartingTile.Num;
                _lblCapitols[i].Content = player.Name + ": " + capitolTile.ToString();
            }

            var random = new Random();
            int choice = random.Next(2);
            string tileSide = choice == 0 ? "A" : "B";

            for(int i = 0; i < _generalSetupViewModel.BaseTiles.Count; i++)
            {
                _lblMapTiles[i].Content = _generalSetupViewModel.BaseTiles[i].Num.ToString() + tileSide;
            }


            for(int i = 0; i < _lblVictoryCards.Count; i++)
            {
                _lblVictoryCards[i].Content = _generalSetupViewModel.VictoryCards[i].ToString();
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new CivSelectionPage(_mainWindow, _mainFrame, _generalSetupViewModel.Players));
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to start Civ A New Dawn?";
            string title = "";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);

            if(result == MessageBoxResult.Yes)
            {
                _mainFrame.Navigate(new GameplayPage(_mainWindow, _mainFrame, _generalSetupViewModel.GameMaster));
            }
        }
    }
}
