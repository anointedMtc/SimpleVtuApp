﻿using SharedKernel.Common.Constants;
using SharedKernel.Domain;
using SharedKernel.Domain.Exceptions;
using SharedKernel.Domain.Interfaces;
using VtuApp.Domain.Events;
using VtuApp.Domain.Exceptions;
using VtuApp.Shared.Constants;

namespace VtuApp.Domain.Entities.VtuModelAggregate;

public class Customer : BaseEntity, IAggregateRoot
{
    private readonly List<VtuTransaction> _vtuTransactions = [];
    private readonly List<VtuAppTransfer> _vtuBonusTransfers = [];

    public Guid CustomerId { get; private set; } // belongs to this alone...
    public Guid ApplicationUserId { get; private set; } // same as the user created in the identy module and passed around for CorrelationId
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public VtuAmount VtuBonusBalance { get; private set; }
    public IReadOnlyCollection<VtuAppTransfer> VtuBonusTransfers => _vtuBonusTransfers.AsReadOnly();
    public VtuAmount MainBalance { get; private set; }

    public IReadOnlyCollection<VtuTransaction> VtuTransactions => _vtuTransactions.AsReadOnly();
    
    public int NumberOfStars { get; private set; }
    //public int StarCount { get; private set; }  // let it keep track of how many times you achieved NumberOfStars bonus because we reset
    public int TransactionCount { get; private set; }
    public int FiveTransactionCount { get; private set; }

    public DateTimeOffset TimeLastStarWasAchieved { get; set; } 


    public Customer(Guid applicationUserId, string firstName, string lastName,
        string email, string phoneNumber, VtuAmount registrationBonus)
    {
        ApplicationUserId = applicationUserId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        VtuBonusBalance = registrationBonus;
        MainBalance = 0;
        //TimeLastStarWasAchieved = DateTimeOffset.UtcNow;  you should probably add this... migrations took care of it now, but it may hunt you in future when you make a new/clean migrations

        AddDomainEvent(new VtuAppCustomerCreatedDomainEvent(
            ApplicationUserId,
            Email)
        );
    }

    //#pragma warning disable CS8618    // Required by Entity Framework
    private Customer() { }



    public bool CanBuy(VtuAmount amount)
    {
        if (MainBalance > amount)
        {
            return true;
        }

        return false;
    }
     

    public void AddToCustomerBalance(VtuAmount amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException(amount);
        }
        MainBalance += amount;

        // domainEvent??? -- funds are only added as commands from walletModule which also raises an event after that... so no need

        return;
    }

    public void DeductFromCustomerBalance(VtuAmount amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException(amount);
        }

        if (MainBalance < amount)
        {
            throw new InsufficientCustomerFundsException(CustomerId);
        }

        MainBalance -= amount;

        // domainEvent??? -- funds are only deducted as commands from walletModule which also raises an event after that... so no need

        return;
    }


    public VtuTransaction AddVtuTransaction(TypeOfTransaction typeOfTransaction,
        NetworkProvider netWorkProvider, VtuAmount vtuAmount,
        DateTimeOffset createdAt, Status status, VtuAmount discount)
    {
        var vtuTransaction = new VtuTransaction(typeOfTransaction,
            netWorkProvider, vtuAmount, createdAt, status, this.CustomerId, discount);

        _vtuTransactions.Add(vtuTransaction);

        // NO NEED TO ADD THIS EVENT HERE... BECAUSE WHATEVER COMMAND IS PRODUCIING THIS VTU-TRANSACTION WOULD ALSO RAISE SOME KINDA EVENT... BUT IF NEEDED, THEN....
        //AddDomainEvent(new VtuAppTransactionDomainEvent(
        //     this.CustomerId,
        //        typeOfTransaction.ToString(),
        //        netWorkProvider.ToString(),
        //        vtuAmount,
        //        createdAt,
        //        status.ToString())
        //);

        // for every five transactions you buy, we would give you 10 Naira bonus
        TransactionCount++;
        if (TransactionCount == 5)
        {
            // reset count
            TransactionCount = 0;
            FiveTransactionCount++;

            var timeOfDiscount = DateTimeOffset.UtcNow;
            var reasonWhy = $"Five Transactions at {timeOfDiscount}";
            VtuAmount bonusForFiveTransactions = 10M;
            AddToBonusBalance(bonusForFiveTransactions, reasonWhy);

            AddDomainEvent(new FiveTransactionsVtuAppDomainEvent(
                this.CustomerId,
                Email,
                FirstName,
                bonusForFiveTransactions,
                VtuBonusBalance,
                timeOfDiscount));
        }

        // use the method here
        CheckForStar();

        return vtuTransaction;
    }

    public void UpdateVtuTransactionStatus(Guid vtuTransactionId, Status status)
    {
        var transactionToUpdate = _vtuTransactions.SingleOrDefault(e => e.Id == vtuTransactionId);

        transactionToUpdate?.UpdateStatus(status);
    }

    // if a customer adds up to 3 new transactions within the span of one hour, then he gets a star and 10% bonus of the 3 transactions made
    private void CheckForStar()
    {
        var elapsedTime = (DateTimeOffset.UtcNow - TimeLastStarWasAchieved).TotalHours; // The TotalHours property represents whole and fractional hours, whereas the Hours property represents whole hours. --- simply means it would return 1.2 hours, 1.6 etc... so the fractions (minutes, seconds, even ticks) are taken into consideration with this method
        if (elapsedTime > 1 && _vtuTransactions.Count >= 3)
        {
            var chosenTransactions = _vtuTransactions.Take(3);
            var currentTime = DateTimeOffset.UtcNow;
            var oneHourAgo = currentTime - TimeSpan.FromHours(1); // use .Add(-1)  we can add negative hours
            VtuAmount totalTransactionsMade = 0;

            foreach (var transaction in chosenTransactions)
            {
                // add all the time intevals
                currentTime.Subtract(transaction.CreatedAt);
            }

            if (currentTime < oneHourAgo)
            {
                // you do not qualify because they do not fall within one-hour range
                return;
            }
            else
            {
                // you qualified

                // we need to now add a timestamp that will help us sort from the last time you earned a star
                // add one hour to it so that you won't be qulified for another one hour
                //TimeLastStarWasAchieved += TimeSpan.FromHours(1);
                //TimeLastStarWasAchieved = new TimeSpan(1, 00, 00);
                // DateTime newDate = currentDateAndTime.AddHours(3);
                /*
                 // Add -1 hour to simulate timespan being one hour in past
                    DateTime d1 = DateTime.Now.AddHours(-1);
                    DateTime d2 = DateTime.Now;
                    TimeSpan diff = d2 - d1;
                    bool diffIsGreaterThan1Hour = diff.TotalHours > 1;
                 */
                TimeLastStarWasAchieved = DateTimeOffset.UtcNow;

                NumberOfStars++;

                foreach (var transaction in chosenTransactions)
                {
                    totalTransactionsMade += transaction.Amount;
                }
                var discountGiven = totalTransactionsMade * 0.1M;

                //AddToCustomerBalance(discountGiven);
                var createdAt = DateTimeOffset.UtcNow;
                var reasonWhy = $"Star Achieved at {createdAt}";
                AddToBonusBalance(discountGiven, reasonWhy); // also check for updated vtuTransaction

                // I AM LEAVING THIS HERE TO SHOW THAT YOU CAN TAKE IN THE WHOLE CUSTOMER AS AN INPUT
                //AddDomainEvent(new StarAchievedByCustomerDomainEvent(
                //    this, createdAt, discountGiven));

                AddDomainEvent(new StarAchievedByCustomerDomainEvent(
                    CustomerId,
                    Email,
                    FirstName,
                    discountGiven,
                    totalTransactionsMade,
                    VtuBonusBalance,
                    createdAt));

            }
        }

        return;
    }

    public void DeductNumberOfStars()
    {
        NumberOfStars--;
    }

    public void AddNumberOfStars()
    {
        NumberOfStars++;
    }

    // HANDLING BONUSES
    public VtuAppTransfer AddToBonusBalance(VtuAmount amountTransfered, string reasonWhy)
    {
        if (amountTransfered <= 0)
        {
            throw new InvalidAmountException(amountTransfered);
        }

        var vtuBonusTransfer = new VtuAppTransfer( // transferDirection reasonWhy
            amountTransfered, 
            DateTimeOffset.UtcNow,
            //VtuBonusBalance,  // The same entity is being tracked as different entity types 'VtuBonusTransfer.InitialBalance#VtuAmount' and 'Customer.VtuBonusBalance#VtuAmount' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
            VtuBonusBalance,
            VtuBonusBalance + amountTransfered,
            TransferDirection.In,
            reasonWhy,
            this.CustomerId);

        _vtuBonusTransfers.Add(vtuBonusTransfer);

        VtuBonusBalance += amountTransfered;

        return vtuBonusTransfer;
    }


    public VtuAppTransfer DeductFromBonusBalance(VtuAmount amountTransfered, string reasonWhy)
    {
        if (amountTransfered <= 0)
        {
            throw new InvalidAmountException(amountTransfered);
        }

        if (VtuBonusBalance < amountTransfered)
        {
            throw new InsufficientCustomerFundsException(CustomerId);
        }

        var vtuBonusTransfer = new VtuAppTransfer( // transferDirection reasonWhy
            amountTransfered,
            DateTimeOffset.UtcNow,
            //VtuBonusBalance,  // The same entity is being tracked as different entity types 'VtuBonusTransfer.InitialBalance#VtuAmount' and 'Customer.VtuBonusBalance#VtuAmount' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
            VtuBonusBalance,
            VtuBonusBalance - amountTransfered,
            TransferDirection.Out,
            reasonWhy,
            this.CustomerId);

        _vtuBonusTransfers.Add(vtuBonusTransfer);

        VtuBonusBalance -= amountTransfered;

        return vtuBonusTransfer;
    }

}
