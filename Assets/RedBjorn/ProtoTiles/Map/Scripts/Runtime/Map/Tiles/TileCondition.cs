using RedBjorn.Utils;

namespace RedBjorn.ProtoTiles.Tiles
{
    public abstract class TileCondition : ScriptableObjectExtended
    {
        public abstract bool IsMet(TileEntity tile);
    }
}
