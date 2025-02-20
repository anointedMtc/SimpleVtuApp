using MediatR;

namespace DomainSharedKernel.Interfaces;

public interface IDomainEvent : INotification
{
    // I prefer to use this marker interface instead of a baseclass because of the 
    // advice from Chris Patterson founder of MassTransit... DON'T USE BASE CLASSES
}
// let us also consider this a marker interface sort of... because in real sense
// we could have just passed in the INotification like they did eShopOnContainer
// but let's just say for the sake of readablility we have renamed it to 
// IDomainEvent so we know what we are talking about... but essentially what we
// are doing is similar to  
//
//          UserCreatedDomainEvent : INotification
//
// and then we have a handler for
//
// UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
