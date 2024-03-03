
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.MONO
{
public partial class TbMapConfig
{
    private readonly System.Collections.Generic.Dictionary<int, MapConfig> _dataMap;
    private readonly System.Collections.Generic.List<MapConfig> _dataList;
    
    public TbMapConfig(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, MapConfig>();
        _dataList = new System.Collections.Generic.List<MapConfig>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            MapConfig _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = MapConfig.DeserializeMapConfig(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.ID, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, MapConfig> DataMap => _dataMap;
    public System.Collections.Generic.List<MapConfig> DataList => _dataList;

    public MapConfig GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public MapConfig Get(int key) => _dataMap[key];
    public MapConfig this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

