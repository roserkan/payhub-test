namespace Payhub.Application.Common.Constants;

public static class CacheKeys
{
    public const string User_All = "User_All";
    public static readonly string[] User_Remove = { "User", "Role" };
    
    public const string Role_All = "Role_All";
    public const string Role_All_Select = "Role_All_Select";
    public static readonly string[] Role_Remove = { "Role", "User", "Site", "Account", "Affiliate" };
    
    public const string Account_All = "Account_All";
    public const string Account_All_Select = "Account_All_Select";
    public const string Account_All_Pw = "Account_All_Pw";
    public static readonly string[] Account_Remove = { "Account", "Bank", "Site" };
    
    public const string Bank_All = "Bank_All";
    public const string Bank_All_Select = "Bank_All_Select";
    public static readonly string[] Bank_Remove = { "Bank", "Account" };
    
    public const string PaymentWay_All = "PaymentWay_All";
    public const string PaymentWay_All_Select = "PaymentWay_All_Select";
    public static readonly string[] PaymentWay_Remove = { "PaymentWay", "Account" };
    
    public const string Infrastructure_All = "Infrastructure_All";
    public const string Infrastructure_All_Select = "Infrastructure_All_Select";
    public static readonly string[] Infrastructure_Remove = { "Infrastructure", "Site" };
    
    public const string Site_All = "Site_All";
    public const string Site_All_Select = "Site_All_Select";
    public static readonly string[] Site_Remove = { "Site", "Infrastructure", "Account" };
    
    public const string Blacklist_PanelIds = "Blacklist_PanelIds";  
    public const string Blacklist_PanelIds_Select = "Blacklist_PanelIds_Select";
    public static readonly string[] Blacklist_PanelIds_Remove = { "Blacklist" };
    
    public const string Affiliate_All = "Affiliate_All";
    public const string Affiliate_All_Select = "Affiliate_All_Select";
    public static readonly string[] Affiliate_Remove = { "Affiliate" };
}