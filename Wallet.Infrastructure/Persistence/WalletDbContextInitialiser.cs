using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Entities;
using System.Text.Json;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Shared.DTO;

namespace Wallet.Infrastructure.Persistence;

public class WalletDbContextInitialiser
{
    private readonly ILogger<WalletDbContextInitialiser> _logger;
    private readonly WalletDbContext _walletDbContext;

    public WalletDbContextInitialiser(ILogger<WalletDbContextInitialiser> logger, 
        WalletDbContext walletDbContext)
    {
        _logger = logger;
        _walletDbContext = walletDbContext;
    }

    public async void SeedDataBase()
    {
        if (!_walletDbContext.Owners.Any())
        {
            //var owners = new Owner[]
            //{
            //    //new Owner { Email = $""}
            //};

            var owners = await CreateDefaultOwners();

            await _walletDbContext.Owners.AddRangeAsync(owners);
            _walletDbContext.SaveChanges();
        }

        if (!_walletDbContext.Transfers.Any())
        {
            //var transfers = new Transfer[]
            //{
            //    //new Transfer { Amount = 34}
            //};

            var transfers = await CreateDefaultTransfers();

            await _walletDbContext.Transfers.AddRangeAsync(transfers);
            _walletDbContext.SaveChanges();
        }

        if (!_walletDbContext.WalletDomainEntities.Any())
        {
            //var walletDomainEntity = new WalletDomainEntity[]
            //{
            //    //new WalletDomainEntity { Transfers = }
            //};

            var walletDomainEntity = await CreateDefaultWallets();

            await _walletDbContext.WalletDomainEntities.AddRangeAsync(walletDomainEntity);
            _walletDbContext.SaveChanges();
        }

        if (_walletDbContext.ChangeTracker.HasChanges()) await _walletDbContext.SaveChangesAsync();

    }



    // OWNER
    private async Task<List<Owner>> CreateDefaultOwners()
    {
        string fileName = "owner.json";
        if (!File.Exists(fileName))
        {
            _logger.LogInformation($"Creating {fileName}");
            using Stream writer = new FileStream(fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(writer, GetDefaultOwners());
        }

        _logger.LogInformation($"Reading owner types from file {fileName}");
        using Stream reader = new FileStream(fileName, FileMode.Open);
        var apptTypes = await JsonSerializer.DeserializeAsync<List<OwnerDto>>(reader);

        return apptTypes.Select(dto => new Owner(dto.OwnerId, dto.Email, dto.FirstName, dto.LastName)).ToList();
    }
    

    private static readonly Guid OwnerIdOne = Guid.NewGuid();
    private static readonly Guid OwnerIdTwo = Guid.NewGuid();
    private static readonly Guid OwnerIdThree = Guid.NewGuid();

    private static readonly Guid WalletIdOne = Guid.NewGuid();
    private static readonly Guid WalletIdTwo = Guid.NewGuid();
    private static readonly Guid WalletIdThree = Guid.NewGuid();

    private static List<OwnerDto> GetDefaultOwners()
    {
        var result = new List<OwnerDto>
            {
                new OwnerDto {
                  OwnerId = OwnerIdOne,
                  Email = "owner.one@gmail.com",
                  FirstName = "Owner One",
                  LastName = "Owner One LastName"
                },
                new OwnerDto {
                  OwnerId = OwnerIdTwo,
                  Email = "owner.two@gmail.com",
                  FirstName = "Owner Two",
                  LastName = "Owner Two LastName"
                },
                new OwnerDto{
                  OwnerId = OwnerIdThree,
                  Email = "owner.three@gmail.com",
                  FirstName = "Owner Three",
                  LastName = "Owner Three LastName"
                }
            };

        return result;
    }



    // TRANSFER
    private async Task<List<Transfer>> CreateDefaultTransfers()
    {
        string fileName = "transfer.json";
        if (!File.Exists(fileName))
        {
            _logger.LogInformation($"Creating {fileName}");
            using Stream writer = new FileStream(fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(writer, GetDefaultTransfers());
        }

        _logger.LogInformation($"Reading transfer types from file {fileName}");
        using Stream reader = new FileStream(fileName, FileMode.Open);
        var apptTypes = await JsonSerializer.DeserializeAsync<List<TransferDto>>(reader);

        return apptTypes.Select(dto => new Transfer(dto.WalletId, dto.Amount, (TransferDirection)dto.Direction, dto.ReasonWhy, dto.CreatedAt)).ToList();
    }

    private static List<TransferDto> GetDefaultTransfers()
    {
        var result = new List<TransferDto>
            {
                new TransferDto {
                  TransferId = Guid.NewGuid(),
                  WalletId = WalletIdOne,
                  Amount = 34.5M,
                  Direction = 0,
                  CreatedAt = DateTimeOffset.UtcNow,
                },
                new TransferDto {
                  TransferId = Guid.NewGuid(),
                  WalletId = WalletIdTwo,
                  Amount = 41.5M,
                  Direction = 1,
                  CreatedAt = DateTimeOffset.UtcNow,
                },
                new TransferDto{
                  TransferId = Guid.NewGuid(),
                  WalletId = WalletIdThree,
                  Amount = 59.5M,
                  Direction = 0,
                  CreatedAt = DateTimeOffset.UtcNow,
                },
                new TransferDto{
                  TransferId = Guid.NewGuid(),
                  WalletId = WalletIdOne,
                  Amount = 82.5M,
                  Direction = 1,
                  CreatedAt = DateTimeOffset.UtcNow,
                }
            };

        return result;
    }



    // WALLET-DOMAIN-ENTITY
    private async Task<List<WalletDomainEntity>> CreateDefaultWallets()
    {
        string fileName = "wallet.json";
        if (!File.Exists(fileName))
        {
            _logger.LogInformation($"Creating {fileName}");
            using Stream writer = new FileStream(fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(writer, GetDefaultWallets());
        }

        _logger.LogInformation($"Reading wallet types from file {fileName}");
        using Stream reader = new FileStream(fileName, FileMode.Open);
        var apptTypes = await JsonSerializer.DeserializeAsync<List<WalletDto>>(reader);

        //return apptTypes.Select(dto => new WalletDomainEntity(dto.WalletId, dto.OwnerId, dto.CreatedAt, dto.Amount, dto.Transfers)).ToList();
        return apptTypes.Select(dto => new WalletDomainEntity(dto.OwnerId, dto.ApplicationUserId, dto.UserEmail)).ToList();
    }


    // FUNNY THING IS THAT WE ONLY NEEDED THE OWNER ID TO CREATE A NEW WALLET
    private static List<WalletDto> GetDefaultWallets()
    {
        var result = new List<WalletDto>
            {
                new WalletDto {
                  WalletId = WalletIdOne,
                  OwnerId = OwnerIdOne,
                  CreatedAt = DateTimeOffset.UtcNow,
                  Amount = 324M,
                  Transfers = new List<TransferDto>
                  {
                      new TransferDto
                      {
                          TransferId = Guid.NewGuid(),
                          WalletId = WalletIdOne,
                          Amount = 44.5M,
                          Direction = 1,
                          CreatedAt = DateTimeOffset.UtcNow,
                      }
                  }

                },
                new WalletDto {
                  WalletId = WalletIdTwo,
                  OwnerId = OwnerIdTwo,
                  CreatedAt = DateTimeOffset.UtcNow,
                  Amount = 324M,
                  Transfers = new List<TransferDto>
                  {
                      
                  }
                },
                new WalletDto{
                  WalletId = WalletIdThree,
                  OwnerId = OwnerIdThree,
                  CreatedAt = DateTimeOffset.UtcNow,
                  Amount = 324M,
                  Transfers = new List<TransferDto>
                  {

                  }
                }
            };

        return result;
    }
}
