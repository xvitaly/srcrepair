using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair
{
    public sealed class GameManager
    {
        private List<SourceGame> SourceGames;
        public SourceGame SelectedGame { get; private set; }

        public void Select(string GameName)
        {
            SelectedGame = SourceGames.Find(Item => String.Equals(Item.FullAppName, GameName, StringComparison.CurrentCultureIgnoreCase));
        }

        public GameManager()
        {
        }
    }
}
