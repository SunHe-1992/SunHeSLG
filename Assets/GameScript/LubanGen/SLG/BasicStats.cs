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

public sealed partial class BasicStats :  Bright.Config.BeanBase 
{
    public BasicStats(JSONNode _json) 
    {
        { if(!_json["HPMax"].IsNumber) { throw new SerializationException(); }  HPMax = _json["HPMax"]; }
        { if(!_json["Str"].IsNumber) { throw new SerializationException(); }  Str = _json["Str"]; }
        { if(!_json["Mag"].IsNumber) { throw new SerializationException(); }  Mag = _json["Mag"]; }
        { if(!_json["Dex"].IsNumber) { throw new SerializationException(); }  Dex = _json["Dex"]; }
        { if(!_json["Spd"].IsNumber) { throw new SerializationException(); }  Spd = _json["Spd"]; }
        { if(!_json["Def"].IsNumber) { throw new SerializationException(); }  Def = _json["Def"]; }
        { if(!_json["Res"].IsNumber) { throw new SerializationException(); }  Res = _json["Res"]; }
        { if(!_json["Luk"].IsNumber) { throw new SerializationException(); }  Luk = _json["Luk"]; }
        { if(!_json["Bld"].IsNumber) { throw new SerializationException(); }  Bld = _json["Bld"]; }
        PostInit();
    }

    public BasicStats(int HPMax, int Str, int Mag, int Dex, int Spd, int Def, int Res, int Luk, int Bld ) 
    {
        this.HPMax = HPMax;
        this.Str = Str;
        this.Mag = Mag;
        this.Dex = Dex;
        this.Spd = Spd;
        this.Def = Def;
        this.Res = Res;
        this.Luk = Luk;
        this.Bld = Bld;
        PostInit();
    }

    public static BasicStats DeserializeBasicStats(JSONNode _json)
    {
        return new SLG.BasicStats(_json);
    }

    /// <summary>
    /// HP
    /// </summary>
    public int HPMax { get; private set; }
    /// <summary>
    /// Strength
    /// </summary>
    public int Str { get; private set; }
    /// <summary>
    /// Magic
    /// </summary>
    public int Mag { get; private set; }
    /// <summary>
    /// Dexterity
    /// </summary>
    public int Dex { get; private set; }
    /// <summary>
    /// Speed
    /// </summary>
    public int Spd { get; private set; }
    /// <summary>
    /// Defence
    /// </summary>
    public int Def { get; private set; }
    /// <summary>
    /// Resistance
    /// </summary>
    public int Res { get; private set; }
    /// <summary>
    /// Luck
    /// </summary>
    public int Luk { get; private set; }
    /// <summary>
    /// Build
    /// </summary>
    public int Bld { get; private set; }

    public const int __ID__ = 542478865;
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
        + "HPMax:" + HPMax + ","
        + "Str:" + Str + ","
        + "Mag:" + Mag + ","
        + "Dex:" + Dex + ","
        + "Spd:" + Spd + ","
        + "Def:" + Def + ","
        + "Res:" + Res + ","
        + "Luk:" + Luk + ","
        + "Bld:" + Bld + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
