using System.Collections.Generic;
public class FUIDef
{
    /// <summary>
    /// UI内容
    /// </summary>
    public enum FWindow
    {
        /// <summary>
        /// 登录
        /// </summary>
        LoginPage,
        /// <summary>
        /// 加载界面
        /// </summary>
        LoginAssets,
        /// <summary>
        /// 匹配和对局开始界面
        /// </summary>
        FindMatchUI,
        /// <summary>
        /// 匹配房间界面
        /// </summary>
        HallRoomPrepare,
        /// <summary>
        /// 改名
        /// </summary>
        UserChangeName,
        /// <summary>
        /// 麻将局操作界面
        /// </summary>
        UIMahjong,
        /// <summary>
        /// 大厅
        /// </summary>
        HallPage,
        /// <summary>
        /// 匹配完成显示信息界面
        /// </summary>
        MatchingUI,
        /// <summary>
        /// 匹配完成显示信息界面
        /// </summary>
        HallPlayerChoose,
        /// <summary>
        /// 结算界面
        /// </summary>
        UIClearing,
        /// <summary>
        /// 对局流水
        /// </summary>
        BillRecord,
        /// <summary>
        /// 托管界面
        /// </summary>
        AIManage,
        /// <summary>
        /// tips界面
        /// </summary>
        TipsPage,
        /// <summary>
        /// 雀士界面
        /// </summary>
        SparrowsArchives, //主界面
        sparrowsFriendLevelUp, //好感度升级界面
        sparrowsConclusionSuccess, //契约升级界面
        /// <summary>
        /// 背包
        /// </summary>
        BagPage,
        /// <summary>
        /// 背包使用小弹窗
        /// </summary>
        BagUseTips,
        /// <summary>
        /// GM窗口
        /// </summary>
        GMPage,
        /// <summary>
        /// 商城
        /// </summary>
        ShopPage,
        /// <summary>
        /// 获得奖励弹窗
        /// </summary>
        GetRewardPage,
        /// <summary>
        /// 玩家设置界面
        /// </summary>
        PlayerInfoPage,
        /// <summary>
        /// 邮件界面
        /// </summary>
        MailPage,
        /// <summary>
        /// 好友界面
        /// </summary>
        FriendPage,
        /// <summary>
        /// 商城购买
        /// </summary>
        ShopBuyPage,
        /// <summary>
        /// yes-no界面
        /// </summary>
        ConfirmWindow,
        /// <summary>
        /// 规则说明
        /// </summary>
        RuleDescPage,
        /// <summary>
        /// 茶楼
        /// </summary>
        TeahousePage,
        /// <summary>
        /// 茶楼的奖品一览
        /// </summary>
        TeahousePreview,
        /// <summary>
        /// 物品tips窗口
        /// </summary>
        TipConsumePage,
        /// <summary>
        /// 设置界面
        /// </summary>
        SetPage,
        SetKefu,
        SetExchange,
        /// <summary>
        /// VIP界面
        /// </summary>
        VipPage,
        /// <summary>
        /// 首充界面
        /// </summary>
        FirstChargeUI,
        /// <summary>
        ///引导的界面
        /// </summary>
        GuidePage,
        /// <summary>
        /// 战令界面
        /// </summary>
        BattlePassUI,
        /// 通用规则界面
        Rulepage,
    }
    /// <summary>
    /// 包名
    /// </summary>
    public enum FPackage
    {
        CommonPackage,
        AudioPackage,
        CommonButton,
        UILogin,
        MahjongPackage,
        Hall,
        PlayerInfo,
        HallRoom,
        Sparrows,
        Bag,
        GM,
        Shop,
        GetReward,
        Mail,
        Friend,
        RuleDesc,
        Teahouse,
        TipConsume,
        Set,
        Vip,
        Guide,
        Finance,
        Rule
    }
    /// <summary>
    /// 界面变动
    /// </summary>
    public static Dictionary<FWindow, FPackage> windowUIpair = new Dictionary<FWindow, FPackage>()
    {
        {FWindow.LoginPage, FPackage.UILogin},
        {FWindow.LoginAssets, FPackage.UILogin},
        {FWindow.FindMatchUI, FPackage.MahjongPackage},
        {FWindow.HallRoomPrepare, FPackage.HallRoom},
        {FWindow.MatchingUI, FPackage.MahjongPackage},
        {FWindow.UIMahjong, FPackage.MahjongPackage},
        {FWindow.HallPage, FPackage.Hall},
        {FWindow.UserChangeName, FPackage.PlayerInfo},
        {FWindow.HallPlayerChoose, FPackage.Hall},
        {FWindow.UIClearing, FPackage.MahjongPackage},
        {FWindow.BillRecord, FPackage.MahjongPackage},
        {FWindow.AIManage, FPackage.MahjongPackage},
        {FWindow.TipsPage, FPackage.CommonPackage},
        {FWindow.SparrowsArchives, FPackage.Sparrows},
        {FWindow.sparrowsFriendLevelUp, FPackage.Sparrows},
        {FWindow.sparrowsConclusionSuccess, FPackage.Sparrows},
        {FWindow.BagPage, FPackage.Bag},
        {FWindow.BagUseTips, FPackage.Bag},
        {FWindow.GMPage, FPackage.GM},
        {FWindow.ShopPage, FPackage.Shop},
        {FWindow.ShopBuyPage, FPackage.Shop},
        {FWindow.RuleDescPage, FPackage.RuleDesc},
        {FWindow.GetRewardPage, FPackage.GetReward},
        {FWindow.PlayerInfoPage, FPackage.PlayerInfo},
        {FWindow.MailPage, FPackage.Mail},
        {FWindow.FriendPage, FPackage.Friend},
        {FWindow.ConfirmWindow, FPackage.CommonPackage},
        {FWindow.TeahousePage, FPackage.Teahouse},
        {FWindow.TeahousePreview, FPackage.Teahouse},
        {FWindow.TipConsumePage, FPackage.TipConsume},
        {FWindow.SetKefu, FPackage.Set},
        {FWindow.SetExchange, FPackage.Set},
        {FWindow.SetPage, FPackage.Set},
        {FWindow.VipPage, FPackage.Vip},
        {FWindow.FirstChargeUI, FPackage.Finance},
        {FWindow.GuidePage, FPackage.Guide},
        {FWindow.BattlePassUI, FPackage.Finance},
        {FWindow.Rulepage, FPackage.Rule},
    };
}