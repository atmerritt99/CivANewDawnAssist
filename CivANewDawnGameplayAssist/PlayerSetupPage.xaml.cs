using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using CivANewDawnAssistViewModels;

namespace CivANewDawnGameplayAssist
{
    /// <summary>
    /// Interaction logic for PlayerSetupPage.xaml
    /// </summary>
    public partial class PlayerSetupPage : Page
    {
        private List<ComboBox> _comboBoxes;
        private List<TextBox> _textBoxes;
        private Frame _mainFrame;
        private PlayerSetupViewModel _playerSetupViewModel;
        private MainWindow _mainWindow;
        
        public PlayerSetupPage(MainWindow mainWindow, Frame mainFrame)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _playerSetupViewModel = new PlayerSetupViewModel();
            
            _comboBoxes = new List<ComboBox>() 
            {
                cmbbx_player1Color,
                cmbbx_player2Color,
                cmbbx_player3Color,
                cmbbx_player4Color,
                cmbbx_player5Color
            };

            _textBoxes = new List<TextBox>()
            {
                txtbx_player1Name,
                txtbx_player2Name,
                txtbx_player3Name,
                txtbx_player4Name,
                txtbx_player5Name
            };

            _mainFrame = mainFrame;
        }

        private void UpdateComboBoxes(object sender, int idx)
        {
            foreach (var combobox in _comboBoxes)
            {
                if (combobox == sender)
                    continue;

                if (combobox.SelectedIndex == idx)
                    combobox.SelectedIndex = -1;
            }
        }

        private string ValidatePlayer(int playerIdx)
        {
            var sb = new StringBuilder();

            if (_comboBoxes[playerIdx].SelectedIndex < 0)
                sb.AppendLine($"Player {playerIdx + 1} hasn't selected a color.");

            if (_textBoxes[playerIdx].Text == string.Empty)
                sb.AppendLine($"Player {playerIdx + 1} doesn't have a name");

            return sb.ToString();
        }

        private string ValidatePlayers()
        {
            var sb = new StringBuilder();

            sb.Append(ValidatePlayer(0));
            sb.Append(ValidatePlayer(1));

            if((bool)chkbx_player3.IsChecked)
                sb.Append(ValidatePlayer(2));

            if ((bool)chkbx_player4.IsChecked)
                sb.Append(ValidatePlayer(3));

            if ((bool)chkbx_player5.IsChecked)
                sb.Append(ValidatePlayer(4));

            for(int i = 0; i < _textBoxes.Count; i++)
            {
                for(int j = i + 1; j < _textBoxes.Count; j++)
                {
                    if (_textBoxes[j].Text == string.Empty)
                        continue;

                    if (j > 1 && !(bool)chkbx_player3.IsChecked)
                        continue;

                    if (j > 2 && !(bool)chkbx_player4.IsChecked)
                        continue;

                    if (j > 3 && !(bool)chkbx_player5.IsChecked)
                        continue;

                    if (_textBoxes[i].Text == _textBoxes[j].Text)
                        sb.AppendLine($"Players {i + 1} & {j + 1} need different names.");
                }
            }

            return sb.ToString();
        }

        private void chkbx_player3_Checked(object sender, RoutedEventArgs e)
        {
            txtbx_player3Name.IsEnabled = true;
            cmbbx_player3Color.IsEnabled = true;
            chkbx_player4.IsEnabled = true;
        }

        private void chkbx_player3_Unchecked(object sender, RoutedEventArgs e)
        {
            txtbx_player3Name.IsEnabled = false;
            cmbbx_player3Color.IsEnabled = false;
            chkbx_player4.IsChecked = false;
            chkbx_player4.IsEnabled = false;
        }

        private void chkbx_player4_Checked(object sender, RoutedEventArgs e)
        {
            txtbx_player4Name.IsEnabled = true;
            cmbbx_player4Color.IsEnabled = true;
            chkbx_player5.IsEnabled = true;
        }

        private void chkbx_player4_Unchecked(object sender, RoutedEventArgs e)
        {
            txtbx_player4Name.IsEnabled = false;
            cmbbx_player4Color.IsEnabled = false;
            chkbx_player5.IsChecked = false;
            chkbx_player5.IsEnabled = false;
        }

        private void chkbx_player5_Checked(object sender, RoutedEventArgs e)
        {
            txtbx_player5Name.IsEnabled = true;
            cmbbx_player5Color.IsEnabled = true;
        }

        private void chkbx_player5_Unchecked(object sender, RoutedEventArgs e)
        {
            txtbx_player5Name.IsEnabled = false;
            cmbbx_player5Color.IsEnabled = false;
        }

        private void cmbbx_player1Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComboBoxes(sender, cmbbx_player1Color.SelectedIndex);
        }

        private void cmbbx_player2Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComboBoxes(sender, cmbbx_player2Color.SelectedIndex);
        }

        private void cmbbx_player3Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComboBoxes(sender, cmbbx_player3Color.SelectedIndex);
        }

        private void cmbbx_player4Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComboBoxes(sender, cmbbx_player4Color.SelectedIndex);
        }

        private void cmbbx_player5Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComboBoxes(sender, cmbbx_player5Color.SelectedIndex);
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = ValidatePlayers();

            if (errorMessage == string.Empty)
            {
                var playerNames = _textBoxes.Select(x => x.Text).ToList();
                var playerColors = _comboBoxes.Select(x => x.SelectedIndex).ToList();

                if ((bool)!chkbx_player3.IsChecked)
                {
                    playerNames = playerNames.Take(2).ToList();
                    playerColors = playerColors.Take(2).ToList();
                }
                else if ((bool)!chkbx_player4.IsChecked)
                {
                    playerNames = playerNames.Take(3).ToList();
                    playerColors = playerColors.Take(3).ToList();
                }
                else if ((bool)!chkbx_player5.IsChecked)
                {
                    playerNames = playerNames.Take(4).ToList();
                    playerColors = playerColors.Take(4).ToList();
                }

                var players = _playerSetupViewModel.CreatePlayers(playerNames, playerColors);
                _mainFrame.Navigate(new CivSelectionPage(_mainWindow, _mainFrame, players));
            }
            else
                MessageBox.Show(errorMessage, "ERRORS");
        }
    }
}
