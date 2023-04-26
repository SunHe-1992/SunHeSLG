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

public sealed partial class CharacterData :  Bright.Config.BeanBase 
{
    public CharacterData(JSONNode _json) 
    {
        { if(!_json["ID"].IsNumber) { throw new SerializationException(); }  ID = _json["ID"]; }
        { if(!_json["CharName"].IsString) { throw new SerializationException(); }  CharName = _json["CharName"]; }
        { if(!_json["Description"].IsString) { throw new SerializationException(); }  Description = _json["Description"]; }
        { if(!_json["Gender"].IsNumber) { throw new SerializationException(); }  Gender = (SLG.Gender)_json["Gender"].AsInt; }
        { if(!_json["CharAttr"].IsObject) { throw new SerializationException(); }  CharAttr = SLG.BasicStats.DeserializeBasicStats(_json["CharAttr"]);  }
        { if(!_json["Growth"].IsObject) { throw new SerializationException(); }  Growth = SLG.BasicStats.DeserializeBasicStats(_json["Growth"]);  }
        { if(!_json["CapFix"].IsObject) { throw new SerializationException(); }  CapFix = SLG.BasicStats.DeserializeBasicStats(_json["CapFix"]);  }
        PostInit();
    }

    public CharacterData(int ID, string CharName, string Description, SLG.Gender Gender, SLG.BasicStats CharAttr, SLG.BasicStats Growth, SLG.BasicStats CapFix ) 
    {
        this.ID = ID;
        this.CharName = CharName;
        this.Description = Description;
        this.Gender = Gender;
        this.CharAttr = CharAttr;
        this.Growth = Growth;
        this.CapFix = CapFix;
        PostInit();
    }

    public static CharacterData DeserializeCharacterData(JSONNode _json)
    {
        return new SLG.CharacterData(_json);
    }

    public int ID { get; private set; }
    public string CharName { get; private set; }
    public string Description { get; private set; }
    public SLG.Gender Gender { get; private set; }
    public SLG.BasicStats CharAttr { get; private set; }
    public SLG.BasicStats Growth { get; private set; }
    public SLG.BasicStats CapFix { get; private set; }

    public const int __ID__ = 1694309747;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        CharAttr?.Resolve(_tables);
        Growth?.Resolve(_tables);
        CapFix?.Resolve(_tables);
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        CharAttr?.TranslateText(translator);
        Growth?.TranslateText(translator);
        CapFix?.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "ID:" + ID + ","
        + "CharName:" + CharName + ","
        + "Description:" + Description + ","
        + "Gender:" + Gender + ","
        + "CharAttr:" + CharAttr + ","
        + "Growth:" + Growth + ","
        + "CapFix:" + CapFix + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}