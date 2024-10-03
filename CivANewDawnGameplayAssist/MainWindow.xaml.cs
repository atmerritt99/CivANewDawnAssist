using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer _player;
        private const string MENU_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Truth in the Stones.wav";
		private const string ANCIENT_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Ibn Al-Noor.wav";
		private const string MEDIEVAL_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Medieval Song Village Consort [No Copyright Music].wav";
		private const string INDUSTRIAL_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Industrial Fantasy Music   Industrial Revolution - Kevin MacLeod.wav";
		private const string ATOMIC_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Miami Nights   Main Theme.wav";
		private const string FUTURE_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Equatorial Complex.wav";
		private const string ENDING_MUSIC = "D:\\source\\CivANewDawnGameplayAssist\\CivANewDawnGameplayAssist\\Music\\Arcadia.wav";

		public MainWindow()
        {
            InitializeComponent();
            _mainFrame.Navigate(new PlayerSetupPage(this, _mainFrame));
            _player = new SoundPlayer(MENU_MUSIC);
            _player.LoadAsync();
            _player.PlayLooping();
        }

        public void SwitchToEndingMusic()
        {
            if (_player.SoundLocation == ENDING_MUSIC)
                return;

            _player.Stop();
            _player = new SoundPlayer(ENDING_MUSIC);
            _player.Load();
            _player.PlayLooping();
        }

        public void SwitchToAncientMusic()
        {
            if (_player.SoundLocation == ANCIENT_MUSIC)
                return;

            _player.Stop();
            _player = new SoundPlayer(ANCIENT_MUSIC);
            _player.Load();
            _player.PlayLooping();
        }

        public void SwitchToMedievalMusic()
        {
            if (_player.SoundLocation == MEDIEVAL_MUSIC)
                return;

            _player.Stop();
            _player = new SoundPlayer(MEDIEVAL_MUSIC);
            _player.Load();
            _player.PlayLooping();
        }

        public void SwitchToIndustrialMusic()
        {
            if (_player.SoundLocation == INDUSTRIAL_MUSIC)
                return;

            _player.Stop();
            _player = new SoundPlayer(INDUSTRIAL_MUSIC);
            _player.Load();
            _player.PlayLooping();
        }

        public void SwitchToAtomicMusic()
        {
            if (_player.SoundLocation == ATOMIC_MUSIC)
                return;

            _player.Stop();
            _player = new SoundPlayer(ATOMIC_MUSIC);
            _player.Load();
            _player.PlayLooping();
        }

        public void SwitchToFutureMusic()
        {
            if (_player.SoundLocation == FUTURE_MUSIC)
                return;

            _player.Stop();
            _player = new SoundPlayer(FUTURE_MUSIC);
            _player.Load();
            _player.PlayLooping();
        }
    }
}
