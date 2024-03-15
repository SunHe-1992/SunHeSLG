
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
public sealed partial class BasicStats : Luban.BeanBase
{
    public BasicStats(JSONNode _buf) 
    {
        { if(!_buf["HPMax"].IsNumber) { throw new SerializationException(); }  HPMax = _buf["HPMax"]; }
        { if(!_buf["Str"].IsNumber) { throw new SerializationException(); }  Str = _buf["Str"]; }
        { if(!_buf["Mag"].IsNumber) { throw new SerializationException(); }  Mag = _buf["Mag"]; }
        { if(!_buf["Dex"].IsNumber) { throw new SerializationException(); }  Dex = _buf["Dex"]; }
        { if(!_buf["Spd"].IsNumber) { throw new SerializationException(); }  Spd = _buf["Spd"]; }
        { if(!_buf["Def"].IsNumber) { throw new SerializationException(); }  Def = _buf["Def"]; }
        { if(!_buf["Res"].IsNumber) { throw new SerializationException(); }  Res = _buf["Res"]; }
        { if(!_buf["Luk"].IsNumber) { throw new SerializationException(); }  Luk = _buf["Luk"]; }
        { if(!_buf["Bld"].IsNumber) { throw new SerializationException(); }  Bld = _buf["Bld"]; }
    }

    public static BasicStats DeserializeBasicStats(JSONNode _buf)
    {
        return new SLG.BasicStats(_buf);
    }

    /// <summary>
    /// HP
    /// </summary>
    public readonly int HPMax;
    /// <summary>
    /// Strength
    /// </summary>
    public readonly int Str;
    /// <summary>
    /// Magic
    /// </summary>
    public readonly int Mag;
    /// <summary>
    /// Dexterity
    /// </summary>
    public readonly int Dex;
    /// <summary>
    /// Speed
    /// </summary>
    public readonly int Spd;
    /// <summary>
    /// Defence
    /// </summary>
    public readonly int Def;
    /// <summary>
    /// Resistance
    /// </summary>
    public readonly int Res;
    /// <summary>
    /// Luck
    /// </summary>
    public readonly int Luk;
    /// <summary>
    /// Build
    /// </summary>
    public readonly int Bld;
   
    public const int __ID__ = 542478865;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
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
}

}