using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Torpedo.GameElement;

namespace Torpedo.Windows
{
    /// <summary>
    /// Interaction logic for NameInputDialog.xaml
    /// </summary>
    public partial class NameInputDialog : Window
    {

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public NameInputDialog()
        {
            InitializeComponent();
        }

        private void OnBotChecked(object sender, RoutedEventArgs e)
        {
            if (playerTwoContainer != null && playerTwoText != null)
            {
                playerTwoContainer.Opacity = 0.5;
                playerTwoText.IsEnabled = false;
            }
        }

        private void OnBotUnChecked(object sender, RoutedEventArgs e)
        {
            playerTwoContainer.Opacity = 1;
            playerTwoText.IsEnabled = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnDone(object sender, RoutedEventArgs e)
        {
            try
            {
                CreatePlayers();
                Close();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }

        private void CreatePlayers()
        {
            var player1Name = playerOneText.Text;

            ValidateName(player1Name);
            Player1 = new RealPlayer(player1Name);

            if (isBotCheckbox.IsChecked == true)
            {
                Player2 = new AIPlayer();
            }
            else
            {
                var player2Name = playerTwoText.Text;
                ValidateName(player2Name);
                Player2 = new RealPlayer(player2Name);
            }
        }

        private void ValidateName(string playerName)
        {
            var specialCharacterFilterRegex = new Regex("^[a-zA-Z0-9 ]*$");

            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new FormatException("Player name cannot be blank or whitespace!");
            }
            else if (!specialCharacterFilterRegex.IsMatch(playerName))
            {
                throw new FormatException($"Player name: \"{playerName}\" contains special characters.");
            }
        }
    }
}
