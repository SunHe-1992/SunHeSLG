
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
public sealed partial class MapConfig : Luban.BeanBase
{
    public MapConfig(JSONNode _buf) 
    {
        { if(!_buf["ID"].IsNumber) { throw new SerializationException(); }  ID = _buf["ID"]; }
        { if(!_buf["isCorner"].IsBoolean) { throw new SerializationException(); }  IsCorner = _buf["isCorner"]; }
        { if(!_buf["triggerOnPass"].IsBoolean) { throw new SerializationException(); }  TriggerOnPass = _buf["triggerOnPass"]; }
        { if(!_buf["eventId"].IsNumber) { throw new SerializationException(); }  EventId = _buf["eventId"]; }
        { if(!_buf["eventParam1"].IsNumber) { throw new SerializationException(); }  EventParam1 = _buf["eventParam1"]; }
        { if(!_buf["eventParam2"].IsNumber) { throw new SerializationException(); }  EventParam2 = _buf["eventParam2"]; }
        { if(!_buf["tileTitle"].IsString) { throw new SerializationException(); }  TileTitle = _buf["tileTitle"]; }
    }

    public static MapConfig DeserializeMapConfig(JSONNode _buf)
    {
        return new MapConfig(_buf);
    }

    public readonly int ID;
    public readonly bool IsCorner;
    public readonly bool TriggerOnPass;
    public readonly int EventId;
    public readonly float EventParam1;
    public readonly int EventParam2;
    public readonly string TileTitle;
   
    public const int __ID__ = -1840922722;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "ID:" + ID + ","
        + "isCorner:" + IsCorner + ","
        + "triggerOnPass:" + TriggerOnPass + ","
        + "eventId:" + EventId + ","
        + "eventParam1:" + EventParam1 + ","
        + "eventParam2:" + EventParam2 + ","
        + "tileTitle:" + TileTitle + ","
        + "}";
    }
}

}
