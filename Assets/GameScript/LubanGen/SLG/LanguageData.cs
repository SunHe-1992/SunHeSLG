//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg.SLG
{

public sealed partial class LanguageData :  Bright.Config.BeanBase 
{
    public LanguageData(JSONNode _json) 
    {
        { if(!_json["ID"].IsString) { throw new SerializationException(); }  ID = _json["ID"]; }
        { var __json0 = _json["Lans"]; if(!__json0.IsArray) { throw new SerializationException(); } Lans = new System.Collections.Generic.Dictionary<string, string>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { string _k0;  { if(!__e0[0].IsString) { throw new SerializationException(); }  _k0 = __e0[0]; } string _v0;  { if(!__e0[1].IsString) { throw new SerializationException(); }  _v0 = __e0[1]; }  Lans.Add(_k0, _v0); }   }
        PostInit();
    }

    public LanguageData(string ID, System.Collections.Generic.Dictionary<string, string> Lans ) 
    {
        this.ID = ID;
        this.Lans = Lans;
        PostInit();
    }

    public static LanguageData DeserializeLanguageData(JSONNode _json)
    {
        return new SLG.LanguageData(_json);
    }

    public string ID { get; private set; }
    public System.Collections.Generic.Dictionary<string, string> Lans { get; private set; }

    public const int __ID__ = -594448350;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "ID:" + ID + ","
        + "Lans:" + Bright.Common.StringUtil.CollectionToString(Lans) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
