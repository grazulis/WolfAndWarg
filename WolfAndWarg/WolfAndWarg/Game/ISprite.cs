using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WolfAndWarg.Game
{
    public interface ISprite
    {
        Vector2 Position { get; set; }
        Vector2 OldPosition { get; set; }
        Texture2D Texture { get; set; }
        int Health { get; set; }
        int Defence { get; set; }
    }
}