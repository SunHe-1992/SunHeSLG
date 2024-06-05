
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.SLG
{
public partial class TbPawn
{
    private readonly System.Collections.Generic.Dictionary<int, PawnData> _dataMap;
    private readonly System.Collections.Generic.List<PawnData> _dataList;
    
    public TbPawn(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, PawnData>();
        _dataList = new System.Collections.Generic.List<PawnData>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            PawnData _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = PawnData.DeserializePawnData(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.ID, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, PawnData> DataMap => _dataMap;
    public System.Collections.Generic.List<PawnData> DataList => _dataList;

    public PawnData GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public PawnData Get(int key) => _dataMap[key];
    public PawnData this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

