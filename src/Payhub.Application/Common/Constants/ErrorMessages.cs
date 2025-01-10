namespace Payhub.Application.Common.Constants;

public static class ErrorMessages
{
    #region Users
    public const string User_NotFound = "Kullanıcı bulunamadı";
    public const string User_InvalidLoginCredentials = "Giriş bilgileri doğrulanamadı";
    #endregion
    
    #region Roles
    public const string Role_NotFound = "Rol bulunamadı";
    #endregion
    
    #region Accounts
    public const string Account_NotFound = "Hesap bulunamadı";
    #endregion
    
    #region Infrastructures
    public const string Infrastructure_NotFound = "Altyapı bulunamadı";
    #endregion
    
    #region Sites
    public const string Site_NotFound = "Site bulunamadı";
    #endregion
    
    #region Blacklists
    public const string Blacklist_NotFound = "Kullanıcı karalistede bulunamadı";
    #endregion
    
    #region Deposits
    public const string Deposits_NotFound = "Yatırıma bulunamadı";
    public const string Deposits_SiteNotFound = "Yatırıma ait site bulunamadı";
    public const string Deposits_AccountNotFound = "Yatırıma ait uygun hesap bulunamadı";
    public const string Deposits_CustomerNotFound = "Yatırıma ait kullanıcı site bulunamadı";
    public const string Deposits_PaymentWayInactive = "Ödeme yöntemi aktif değil";
    public const string Deposits_InvalidSecurityKey = "Geçersiz güvenlik anahtarı";
    public const string Deposits_HasAlreadyActiveDeposit = "Aktif talep bulunmaktadır";
    public const string Deposits_Blacklisted = "Kullanıcı engellenmiştir";
    public const string Deposits_AlreadyConfirmed = "Bu talep zaten onaylanmıştır";
    public const string Deposits_AlreadyDeclined = "Bu talep zaten reddedilmiştir";
    public const string Deposits_AlreadyTransfered = "Bu talep zaten bekleyenlere taşınmıştır";
    public const string Deposits_AlreadyTimeOut = "Bu talep zaten timeout olmuştur";
    public const string Deposits_StatusMustBePendingConfirmation = "Talebin durumu 'Onay Bekliyor' olmalıdır";
    public const string Deposits_CannotTransferedForConfirmed = "Onaylanmış talep bekleyenlere alınamaz";
    public const string Deposits_CannotTransferedForTimeout = "Reddedilmiş talep bekleyenlere alınamaz";
    #endregion
    
    #region Withdraws
    public const string Withdraws_NotFound = "Çekim bulunamadı";
    public const string Withdraws_SiteNotFound = "Çekime ait site bulunamadı";
    public const string Withdraws_CustomerNotFound = "Çekime ait kullanıcı site bulunamadı";
    public const string Withdraws_AccountNotFound = "Çekime ait hesap bulunamadı";
    public const string Withdraws_PaymentWayInactive = "Ödeme yöntemi aktif değil";
    public const string Withdraws_InvalidSecurityKey = "Geçersiz güvenlik anahtarı";
    public const string Withdraws_HasAlreadyActiveWithdraw = "Aktif talep bulunmaktadır";
    public const string Withdraws_Blacklisted = "Kullanıcı engellenmiştir";
    public const string Withdraws_AlreadyConfirmed = "Bu talep zaten onaylanmıştır";
    public const string Withdraws_AlreadyDeclined = "Bu talep zaten reddedilmiştir";
    public const string Withdraws_AlreadyTransfered = "Bu talep zaten bekleyenlere taşınmıştır";
    public const string Withdraws_AlreadyTimeOut = "Bu talep zaten timeout olmuştur";
    #endregion
    
    #region Affiliates
    public const string Affiliate_NotFound = "Bayi bulunamadı";
    #endregion

    #region SiteSafeMoves
    public const string SiteSafeMoves_NotFound = "Site kasa hareketi bulunamadı";
    #endregion
    #region AffiliateSafeMoves
    public const string AffiliateSafeMoves_NotFound = "Bayi kasa hareketi bulunamadı";
    #endregion
    #region Devices
    public const string Device_NotFound = "Cihaz bulunamadı";
    #endregion
    
    #region WithdrawOrders
    public const string WithdrawOrder_NotFound = "Çıkış emri bulunamadı";
    #endregion
}