using SharedKernel.Domain.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Domain.Events;


// I AM LEAVING THIS HERE TO SHOW THAT YOU CAN TAKE IN THE WHOLE CUSTOMER AS AN INPUT
//public record StarAchievedByCustomerDomainEvent(
//    Customer Customer,
//    DateTimeOffset CreatedAt,
//    decimal DiscountGiven) : IDomainEvent;


public record StarAchievedByCustomerDomainEvent(
    Guid CustomerId,
    string Email,
    string FirstName,
    decimal DiscountGiven,
    decimal TotalOfTransactionsMade,
    decimal FinalVtuBonusBalance,
    DateTimeOffset CreatedAt) : IDomainEvent;


