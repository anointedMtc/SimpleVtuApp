namespace Identity.Shared.IntegrationEvents;

public record ApplicationUserEmailConfirmedEvent(
    Guid ApplicationUserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    decimal RegisterationBonus
    );


/*
1. we create new owner - it would raise a domain event that would create
    Wallet for the owner

2. A new customer should also be created in the vtuApp module... 

2. send email
*/