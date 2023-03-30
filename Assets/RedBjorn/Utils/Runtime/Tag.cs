namespace RedBjorn.Utils
{
    public enum MapTileEnum : int
    {
        Ground,
        Woods,
        FlyOnly,
        Impassable,
    }
    public class Tag : ScriptableObjectExtended
    {
        public MapTileEnum TileType;
        public string TagName;
    }
}
