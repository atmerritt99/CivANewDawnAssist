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
    /// Interaction logic for CivSelectionPage.xaml
    /// </summary>
    public partial class CivSelectionPage : Page
    {
        private Frame _mainFrame;
        private MainWindow _mainWindow;
        private CivSelectionViewModel _civSelectionViewModel;
        private List<SolidColorBrush> _brushes;
        private List<Label> _labels;
        private List<RadioButton> _radioButtons;
        private List<TextBlock> _civDesc;

        public CivSelectionPage(MainWindow mainWindow, Frame mainFrame, List<Player> players)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _brushes = new List<SolidColorBrush>()
            {
                Brushes.Red,
                Brushes.Orange,
                Brushes.Lime,
                Brushes.Cyan,
                Brushes.Purple
            };

            _mainFrame = mainFrame;
            _civSelectionViewModel = new CivSelectionViewModel(players);

            _labels = new List<Label>()
            {
                lbl_player1,
                lbl_player2,
                lbl_player3,
                lbl_player4,
                lbl_player5
            };

            _radioButtons = new List<RadioButton>()
            {
                rdbtn_plyr1Sel1,
                rdbtn_plyr1Sel2,
                rdbtn_plyr2Sel1,
                rdbtn_plyr2Sel2,
                rdbtn_plyr3Sel1,
                rdbtn_plyr3Sel2,
                rdbtn_plyr4Sel1,
                rdbtn_plyr4Sel2,
                rdbtn_plyr5Sel1,
                rdbtn_plyr5Sel2
            };

            _civDesc = new List<TextBlock>()
            {
                txtblck_plyr1Sel1,
                txtblck_plyr1Sel2,
                txtblck_plyr2Sel1,
                txtblck_plyr2Sel2,
                txtblck_plyr3Sel1,
                txtblck_plyr3Sel2,
                txtblck_plyr4Sel1,
                txtblck_plyr4Sel2,
                txtblck_plyr5Sel1,
                txtblck_plyr5Sel2
            };

            for(int i = 0; i < players.Count; i++)
            {
                _labels[i].Visibility = Visibility.Visible;
                _labels[i].Foreground = _brushes[(int)players[i].PlayerColor];
                _labels[i].Content = _labels[i].Content.ToString() + " " + players[i].Name;
                _radioButtons[i * 2].Visibility = Visibility.Visible;
                _radioButtons[(i * 2) + 1].Visibility = Visibility.Visible;
                _civDesc[i * 2].Text = $"     {_civSelectionViewModel.Players[i].Civ1}";
                _civDesc[(i * 2) + 1].Text = $"     {_civSelectionViewModel.Players[i].Civ2}";
                _radioButtons[i * 2].IsChecked = true;
            }
        }

        private void rdbtn_plyr1Sel1_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection1(0);
        }

        private void rdbtn_plyr1Sel2_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection2(0);
        }

        private void rdbtn_plyr2Sel1_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection1(1);
        }

        private void rdbtn_plyr2Sel2_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection2(1);
        }

        private void rdbtn_plyr3Sel1_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection1(2);
        }

        private void rdbtn_plyr3Sel2_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection2(2);
        }

        private void rdbtn_plyr4Sel1_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection1(3);
        }

        private void rdbtn_plyr4Sel2_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection2(3);
        }

        private void rdbtn_plyr5Sel1_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection1(4);
        }

        private void rdbtn_plyr5Sel2_Checked(object sender, RoutedEventArgs e)
        {
            _civSelectionViewModel.PlayerChoosesSelection2(4);
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new PlayerSetupPage(_mainWindow, _mainFrame));
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new GeneralSetupPage(_mainWindow, _mainFrame, _civSelectionViewModel.GameMaster));
        }
    }
}
