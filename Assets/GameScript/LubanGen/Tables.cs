//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using SimpleJSON;

namespace cfg
{
   
public sealed partial class Tables
{
    public SLG.TbConst TbConst {get; }
    public SLG.TbLanguage TbLanguage {get; }
    public SLG.Character Character {get; }
    public SLG.Item Item {get; }
    public SLG.Class Class {get; }
    public SLG.TileEffect TileEffect {get; }
    public SLG.Skill Skill {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        var tables = new System.Collections.Generic.Dictionary<string, object>();
        TbConst = new SLG.TbConst(loader("slg_tbconst")); 
        tables.Add("SLG.TbConst", TbConst);
        TbLanguage = new SLG.TbLanguage(loader("slg_tblanguage")); 
        tables.Add("SLG.TbLanguage", TbLanguage);
        Character = new SLG.Character(loader("slg_character")); 
        tables.Add("SLG.Character", Character);
        Item = new SLG.Item(loader("slg_item")); 
        tables.Add("SLG.Item", Item);
        Class = new SLG.Class(loader("slg_class")); 
        tables.Add("SLG.Class", Class);
        TileEffect = new SLG.TileEffect(loader("slg_tileeffect")); 
        tables.Add("SLG.TileEffect", TileEffect);
        Skill = new SLG.Skill(loader("slg_skill")); 
        tables.Add("SLG.Skill", Skill);
        PostInit();

        TbConst.Resolve(tables); 
        TbLanguage.Resolve(tables); 
        Character.Resolve(tables); 
        Item.Resolve(tables); 
        Class.Resolve(tables); 
        TileEffect.Resolve(tables); 
        Skill.Resolve(tables); 
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        TbConst.TranslateText(translator); 
        TbLanguage.TranslateText(translator); 
        Character.TranslateText(translator); 
        Item.TranslateText(translator); 
        Class.TranslateText(translator); 
        TileEffect.TranslateText(translator); 
        Skill.TranslateText(translator); 
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
