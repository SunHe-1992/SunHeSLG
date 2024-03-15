
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
public partial class Class
{
    private readonly System.Collections.Generic.Dictionary<int, ClassData> _dataMap;
    private readonly System.Collections.Generic.List<ClassData> _dataList;
    
    public Class(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, ClassData>();
        _dataList = new System.Collections.Generic.List<ClassData>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            ClassData _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = ClassData.DeserializeClassData(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.ID, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, ClassData> DataMap => _dataMap;
    public System.Collections.Generic.List<ClassData> DataList => _dataList;

    public ClassData GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public ClassData Get(int key) => _dataMap[key];
    public ClassData this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
