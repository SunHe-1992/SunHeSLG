
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class Building : Luban.BeanBase
{
    public Building(JSONNode _buf) 
    {
        { if(!_buf["ID"].IsNumber) { throw new SerializationException(); }  ID = _buf["ID"]; }
        { if(!_buf["Name"].IsString) { throw new SerializationException(); }  Name = _buf["Name"]; }
        { if(!_buf["Level"].IsNumber) { throw new SerializationException(); }  Level = _buf["Level"]; }
        { if(!_buf["price"].IsNumber) { throw new SerializationException(); }  Price = _buf["price"]; }
        { if(!_buf["isMax"].IsBoolean) { throw new SerializationException(); }  IsMax = _buf["isMax"]; }
        { if(!_buf["Image"].IsString) { throw new SerializationException(); }  Image = _buf["Image"]; }
    }

    public static Building DeserializeBuilding(JSONNode _buf)
    {
        return new Building(_buf);
    }

    public readonly int ID;
    public readonly string Name;
    public readonly int Level;
    /// <summary>
    /// upgrading price
    /// </summary>
    public readonly long Price;
    public readonly bool IsMax;
    /// <summary>
    /// icon in chapter select
    /// </summary>
    public readonly string Image;
   
    public const int __ID__ = -1366001964;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "ID:" + ID + ","
        + "Name:" + Name + ","
        + "Level:" + Level + ","
        + "price:" + Price + ","
        + "isMax:" + IsMax + ","
        + "Image:" + Image + ","
        + "}";
    }
}

}
