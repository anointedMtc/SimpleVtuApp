/*
 


1.  A user registers and Only when he/she confirms their email address, we raise
    UserVerifiedEvent this is an external event to Wallet Module

2.  UserverifiedEvent should be handled by creating a copy of the user in the
    wallet Domain called Owner. This owner has the same Id as the user... once
    an owner is created, we raise an OwnerAddedDomainEvent... this is an internal
    event to us here in Wallet Module

3   OwnerAddedDomainEvent is handled by creating a wallet for him/her using the
    owner/user Id and after that we raise an External/IntergrationEvent to tell
    others that WalletAddedDomainEvent






 
*/