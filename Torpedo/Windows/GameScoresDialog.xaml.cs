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
using System.Windows.Shapes;
using Torpedo.Model;
using Torpedo.Repository;

namespace Torpedo.Windows
{
    /// <summary>
    /// Interaction logic for ScoresDialog.xaml
    /// </summary>
    public partial class GameScoresDialog : Window
    {
        public GameScoresDialog()
        {
            InitializeComponent();
            AddItems();
        }

        private void AddItems()
        {
            var matchList = MatchRepository.GetMatches();

            foreach (Match match in matchList)
            {
                scoreGrid.Items.Add(match);
            }
        }
    }
}
