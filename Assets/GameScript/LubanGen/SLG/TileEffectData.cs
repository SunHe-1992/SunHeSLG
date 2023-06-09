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

public sealed partial class TileEffectData :  Bright.Config.BeanBase 
{
    public TileEffectData(JSONNode _json) 
    {
        { if(!_json["ID"].IsNumber) { throw new SerializationException(); }  ID = _json["ID"]; }
        { if(!_json["Name"].IsString) { throw new SerializationException(); }  Name = _json["Name"]; }
        { if(!_json["Avoid"].IsNumber) { throw new SerializationException(); }  Avoid = _json["Avoid"]; }
        { if(!_json["Heal"].IsNumber) { throw new SerializationException(); }  Heal = _json["Heal"]; }
        { if(!_json["UnBreakable"].IsBoolean) { throw new SerializationException(); }  UnBreakable = _json["UnBreakable"]; }
        { if(!_json["MoveCost"].IsNumber) { throw new SerializationException(); }  MoveCost = _json["MoveCost"]; }
        { if(!_json["Mov"].IsNumber) { throw new SerializationException(); }  Mov = _json["Mov"]; }
        { if(!_json["Def_foe"].IsNumber) { throw new SerializationException(); }  DefFoe = _json["Def_foe"]; }
        { if(!_json["Res_foe"].IsNumber) { throw new SerializationException(); }  ResFoe = _json["Res_foe"]; }
        { if(!_json["Def_ally"].IsNumber) { throw new SerializationException(); }  DefAlly = _json["Def_ally"]; }
        { if(!_json["Res_ally"].IsNumber) { throw new SerializationException(); }  ResAlly = _json["Res_ally"]; }
        { if(!_json["Damage"].IsNumber) { throw new SerializationException(); }  Damage = _json["Damage"]; }
        PostInit();
    }

    public TileEffectData(int ID, string Name, int Avoid, int Heal, bool UnBreakable, int MoveCost, int Mov, int Def_foe, int Res_foe, int Def_ally, int Res_ally, int Damage ) 
    {
        this.ID = ID;
        this.Name = Name;
        this.Avoid = Avoid;
        this.Heal = Heal;
        this.UnBreakable = UnBreakable;
        this.MoveCost = MoveCost;
        this.Mov = Mov;
        this.DefFoe = Def_foe;
        this.ResFoe = Res_foe;
        this.DefAlly = Def_ally;
        this.ResAlly = Res_ally;
        this.Damage = Damage;
        PostInit();
    }

    public static TileEffectData DeserializeTileEffectData(JSONNode _json)
    {
        return new SLG.TileEffectData(_json);
    }

    public int ID { get; private set; }
    public string Name { get; private set; }
    public int Avoid { get; private set; }
    /// <summary>
    /// on turn start
    /// </summary>
    public int Heal { get; private set; }
    public bool UnBreakable { get; private set; }
    public int MoveCost { get; private set; }
    /// <summary>
    /// on turn start
    /// </summary>
    public int Mov { get; private set; }
    public int DefFoe { get; private set; }
    public int ResFoe { get; private set; }
    public int DefAlly { get; private set; }
    public int ResAlly { get; private set; }
    /// <summary>
    /// on turn start
    /// </summary>
    public int Damage { get; private set; }

    public const int __ID__ = 760987433;
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
        + "Name:" + Name + ","
        + "Avoid:" + Avoid + ","
        + "Heal:" + Heal + ","
        + "UnBreakable:" + UnBreakable + ","
        + "MoveCost:" + MoveCost + ","
        + "Mov:" + Mov + ","
        + "DefFoe:" + DefFoe + ","
        + "ResFoe:" + ResFoe + ","
        + "DefAlly:" + DefAlly + ","
        + "ResAlly:" + ResAlly + ","
        + "Damage:" + Damage + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
