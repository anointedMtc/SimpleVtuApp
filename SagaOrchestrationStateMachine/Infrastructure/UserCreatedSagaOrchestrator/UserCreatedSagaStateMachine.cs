using Identity.Shared.IntegrationEvents;
using MassTransit;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.UserCreatedSaga;
using SharedKernel.Application.Constants;
using VtuApp.Shared.IntegrationEvents;
using Wallet.Shared.IntegrationEvents;

namespace SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;

public sealed class UserCreatedSagaStateMachine : MassTransitStateMachine<UserCreatedSagaStateInstance>
{
    public UserCreatedSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);


        Event(() => ApplicationUserEmailConfirmedEvent, x => x.CorrelateById(context => context.Message.ApplicationUserId));
        Event(() => CreateNewWalletOwnerMessage, x =>
        {
            x.CorrelateById(context => context.Message.ApplicationUserId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => CreateNewVtuAppCustomerMessage, x =>
        {
            x.CorrelateById(context => context.Message.ApplicationUserId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => WalletAddedIntegrationEvent, x =>
        {
            x.CorrelateById(context => context.Message.ApplicationUserId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => VtuAppCustomerCreatedIntegrationEvent, x =>
        {
            x.CorrelateById(context => context.Message.ApplicationUserId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Schedule(() => ScheduleForNotifyingApplicationUserSagaEvent, instance => instance.NotifyApplicationUserScheduleEventTokenId, s =>
        {
            s.Delay = TimeSpan.FromSeconds(30);

            s.Received = r => r.CorrelateById(context => context.Message.ApplicationUserId);

            //s.Received = r => r.OnMissingInstance(m => m.Redeliver(r =>
            //{
            //    r.Interval(5, 1000);
            //    r.OnRedeliveryLimitReached(n => n.Fault());
            //}));
        });

        Event(() => CreateNewWalletOwnerMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.ApplicationUserId)
            .SelectId(m => m.Message.Message.ApplicationUserId)
        );
        Event(() => CreateNewVtuAppCustomerMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.ApplicationUserId)
            .SelectId(m => m.Message.Message.ApplicationUserId)
        );



        Initially(
            When(ApplicationUserEmailConfirmedEvent)
            .Then(context =>
            {
                context.Saga.ApplicationUserId = context.Message.ApplicationUserId;
                context.Saga.FirstName = context.Message.FirstName;
                context.Saga.LastName = context.Message.LastName;
                context.Saga.Email = context.Message.Email;
                context.Saga.PhoneNumber = context.Message.PhoneNumber;
                context.Saga.RegisterationBonus = context.Message.RegisterationBonus;
                context.Saga.CreatedAt = DateTimeOffset.UtcNow;
            })
            .Send(new Uri($"queue:{QueueConstants.CreateNewWalletOwnerMessageQueueName}"),
                context => new CreateNewWalletOwnerMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.LastName,
                    context.Saga.RegisterationBonus))
            .Send(new Uri($"queue:{QueueConstants.CreateNewVtuAppCustomerMessageQueueName}"),
                context => new CreateNewVtuAppCustomerMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.LastName,
                    context.Saga.PhoneNumber,
                    context.Saga.RegisterationBonus))
            .TransitionTo(WalletOwnerAndVtuAppCustomerCreatedSagaState)
        );


        CompositeEvent(() => UserCreatedInAllModulesEvent, x => x.UserCreatedInAllModulesEventStatus,
            WalletAddedIntegrationEvent, VtuAppCustomerCreatedIntegrationEvent);

        DuringAny(
            When(UserCreatedInAllModulesEvent)
               .Schedule(ScheduleForNotifyingApplicationUserSagaEvent,
               context => context.Init<NotifyApplicationUserOfWalletCreatedEvent>(new
               {
                   context.Saga.ApplicationUserId,
                   context.Saga.FirstName,
                   context.Saga.LastName,
                   context.Saga.Email,
                   context.Saga.RegisterationBonus
               }))
               .Finalize()
        );

        DuringAny(
            When(CreateNewWalletOwnerMessageFaulted)
            .TransitionTo(FailedUserCreatedSagaState)
        );
        DuringAny(
            When(CreateNewVtuAppCustomerMessageFaulted)
            .TransitionTo(FailedUserCreatedSagaState)
        );
    }


    public State ApplicationUserConfirmedSagaState { get; private set; }
    public State WalletOwnerAndVtuAppCustomerCreatedSagaState { get; private set; }
    public State FailedUserCreatedSagaState { get; private set; }

    public Event<ApplicationUserEmailConfirmedEvent> ApplicationUserEmailConfirmedEvent { get; private set; }
    public Event<CreateNewWalletOwnerMessage> CreateNewWalletOwnerMessage { get; private set; }
    public Event<CreateNewVtuAppCustomerMessage> CreateNewVtuAppCustomerMessage { get; private set; }

    // COMPOSITE EVENT
    public Event<WalletAddedIntegrationEvent> WalletAddedIntegrationEvent { get; private set; }
    public Event<VtuAppCustomerCreatedIntegrationEvent> VtuAppCustomerCreatedIntegrationEvent { get; private set; }
    public Event UserCreatedInAllModulesEvent { get; private set; }

    // SCHEDULE EVENT
    public Schedule<UserCreatedSagaStateInstance, NotifyApplicationUserOfWalletCreatedEvent> ScheduleForNotifyingApplicationUserSagaEvent { get; private set; }

    // FAULTS
    public Event<Fault<CreateNewWalletOwnerMessage>> CreateNewWalletOwnerMessageFaulted { get; private set; }
    public Event<Fault<CreateNewVtuAppCustomerMessage>> CreateNewVtuAppCustomerMessageFaulted { get; private set; }

}
