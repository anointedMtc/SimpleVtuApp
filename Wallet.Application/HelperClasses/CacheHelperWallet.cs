using SharedKernel.Domain.HelperClasses;

namespace Wallet.Application.HelperClasses;

public static class CacheHelperWallet
{
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _getAllWalletsKeyTemplate = "wallets-{0}-{1}-{2}-{3}";
    private static readonly string _getAllOwnersKeyTemplate = "owners-{0}-{1}-{2}-{3}";
    private static readonly string _getOwnerAndWalletByEmailKeyTemplate = "ownerAndWallet-{0}-{1}-{2}-{3}";
    private static readonly string _getWalletAndTransfersByIdKeyTemplate = "walletAndTransfers-{0}-{1}-{2}-{3}";
    private static readonly string _getWalletByIdKeyTemplate = "walletById-{0}-{1}-{2}-{3}";



    public static string GenerateGetAllWalletsCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllWalletsKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

    public static string GenerateGetAllOwnersCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllOwnersKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

    public static string GenerateGetOwnerAndWalletByEmailCacheKey(string email)
    {
        return string.Format(_getOwnerAndWalletByEmailKeyTemplate, " ", " ", " ", email);
    }

    public static string GenerateGetWalletAndTransfersByIdCacheKey(Guid id)
    {
        return string.Format(_getWalletAndTransfersByIdKeyTemplate, " ", " ", " ", id);
    }
    public static string GenerateGetWalletByIdCacheKey(Guid id)
    {
        return string.Format(_getWalletByIdKeyTemplate, " ", " ", " ", id);
    }


}
