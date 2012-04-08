using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolfAndWarg.Game
{
    class Map
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public Tile[] Tiles { get; set; }
    }

}
