using MassTransit;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;
using SharedKernel.Common.Constants;
using VtuApp.Shared.IntegrationEvents;

namespace SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;

public sealed class VtuAirtimeOrderedSagaStateMachine : MassTransitStateMachine<VtuAirtimeOrderedSagaStateInstance>
{
    public VtuAirtimeOrderedSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CustomerPurchasedAirtimeVtuNationEvent, x => x.CorrelateById(context => context.Message.VtuTransactionId));
        Event(() => BuyAirtimeForCustomerMessage, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => BuyAirtimeForCustomerSuccessEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => BuyAirtimeForCustomerFirstTryFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Schedule(() => ScheduleForSecondRetryVtuAirtimeOrderSagaEvent, instance => instance.SecondRetryVtuAirtimeScheduleEventTokenId, s =>
        {
            s.Delay = TimeSpan.FromMinutes(5);

            s.Received = r => r.CorrelateById(context => context.Message.VtuTransactionId);

        });
        Event(() => SecondRetryVtuAirtimeOrderEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => BuyAirtimeForCustomerSecondReTryFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => NotifyCustomerOfVtuAirtimePurchaseFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => RollbackAmountForVtuAirtimePurchaseFailedMessage, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => NotifyCustomerOfVtuAirtimePurchaseSuccessEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });

        Event(() => BuyAirtimeForCustomerMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );
        Event(() => SecondRetryVtuAirtimeOrderEventFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );
        Event(() => RollbackAmountForVtuAirtimePurchaseFailedMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );
        Event(() => DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );


        Initially(
            When(CustomerPurchasedAirtimeVtuNationEvent)
            .Then(context =>
            {
                context.Saga.ApplicationUserId = context.Message.ApplicationUserId;
                context.Saga.Email = context.Message.Email;
                context.Saga.FirstName = context.Message.FirstName;
                context.Saga.LastName = context.Message.LastName;
                context.Saga.VtuTransactionId = context.Message.VtuTransactionId;
                context.Saga.NetworkProvider = context.Message.NetworkProvider;
                context.Saga.AmountToPurchase = context.Message.AmountPurchased;
                context.Saga.PricePaid = context.Message.PricePaid;
                context.Saga.Receiver = context.Message.Receiver;
                context.Saga.Sender = context.Message.Sender;
                context.Saga.InitialBalance = context.Message.InitialBalance;
                context.Saga.FinalBalance = context.Message.FinalBalance;
                context.Saga.CreatedAt = context.Message.CreatedAt;
            })
            .Send(new Uri($"queue:{QueueConstants.BuyAirtimeForCustomerMessageQueueName}"),
                context => new BuyAirtimeForCustomerMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.AmountToPurchase,
                    context.Saga.Receiver)
                )
            .TransitionTo(BuyAirtimeForCustomerCommandSentSagaState)
        );

        // SECOND STAGE
        During(BuyAirtimeForCustomerCommandSentSagaState,
            When(BuyAirtimeForCustomerSuccessEvent)
            .TransitionTo(SuccessVtuAirtimePurchaseSagaState)
        );
        During(BuyAirtimeForCustomerCommandSentSagaState,
            When(BuyAirtimeForCustomerFirstTryFailedEvent)
            .Schedule(ScheduleForSecondRetryVtuAirtimeOrderSagaEvent,
                context => context.Init<PrepareForSecondRetryVtuAirtimeOrderEvent>(new
                {
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.AmountToPurchase,
                    context.Saga.Receiver
                })
            ),

            When(ScheduleForSecondRetryVtuAirtimeOrderSagaEvent?.Received)
                .Publish(context => new SecondRetryVtuAirtimeOrderEvent(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.AmountToPurchase,
                    context.Saga.Receiver))
            .TransitionTo(WaitingForSecondVtuAirtimeRetrySagaState)
        );

        // THIRD STAGE IF SECOND WASN'T SUCCESSFUL
        During(WaitingForSecondVtuAirtimeRetrySagaState,
            When(BuyAirtimeForCustomerSuccessEvent)
            .TransitionTo(SuccessVtuAirtimePurchaseSagaState)
        );
        During(WaitingForSecondVtuAirtimeRetrySagaState,
            When(BuyAirtimeForCustomerSecondReTryFailedEvent)
            .Send(new Uri($"queue:{QueueConstants.NotifyCustomerOfVtuAirtimePurchaseFailedEventQueueName}"),
                context => new NotifyCustomerOfVtuAirtimePurchaseFailedEvent(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.LastName,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.AmountToPurchase,
                    context.Saga.PricePaid,
                    context.Saga.Receiver,
                    context.Saga.Sender,
                    context.Saga.InitialBalance,
                    context.Saga.FinalBalance,
                    context.Saga.CreatedAt)
                )
            .Send(new Uri($"queue:{QueueConstants.RollbackAmountForVtuAirtimePurchaseFailedMessageQueueName}"),
                context => new RollbackAmountForVtuAirtimePurchaseFailedMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.AmountToPurchase,
                    context.Saga.PricePaid,
                    context.Saga.Receiver,
                    context.Saga.InitialBalance,
                    context.Saga.FinalBalance,
                    context.Saga.CreatedAt)
                )
            .TransitionTo(FailedVtuAirtimePurchaseSagaState)
        );

        // FOURTH STAGE FOR EVERYBODY - WHEN SUCCESSFUL
        During(SuccessVtuAirtimePurchaseSagaState,
            When(BuyAirtimeForCustomerSuccessEvent)
            .Send(new Uri($"queue:{QueueConstants.NotifyCustomerOfVtuAirtimePurchaseSuccessEventQueueName}"),
                context => new NotifyCustomerOfVtuAirtimePurchaseSuccessEvent(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.LastName,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.AmountToPurchase,
                    context.Saga.PricePaid,
                    context.Saga.Receiver,
                    context.Saga.Sender,
                    context.Saga.InitialBalance,
                    context.Saga.FinalBalance,
                    context.Saga.CreatedAt)
                )
            .Send(new Uri($"queue:{QueueConstants.DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageQueueName}"),
                context => new DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.PricePaid)
                )
            .TransitionTo(VtuAirtimeOrderedCompletedSagaState)
            .Finalize()
        );


    }


    public State BuyAirtimeForCustomerCommandSentSagaState { get; private set; }
    public State SuccessVtuAirtimePurchaseSagaState { get; private set; }
    public State WaitingForSecondVtuAirtimeRetrySagaState { get; private set; }
    public State FailedVtuAirtimePurchaseSagaState { get; private set; }
    public State VtuAirtimeOrderedCompletedSagaState { get; private set; }

    public Event<CustomerPurchasedAirtimeVtuNationEvent> CustomerPurchasedAirtimeVtuNationEvent { get; private set; }
    public Event<BuyAirtimeForCustomerMessage> BuyAirtimeForCustomerMessage { get; private set; }
    public Event<BuyAirtimeForCustomerSuccessEvent> BuyAirtimeForCustomerSuccessEvent { get; private set; }
    public Event<BuyAirtimeForCustomerFirstTryFailedEvent> BuyAirtimeForCustomerFirstTryFailedEvent { get; private set; }

    public Schedule<VtuAirtimeOrderedSagaStateInstance, PrepareForSecondRetryVtuAirtimeOrderEvent> ScheduleForSecondRetryVtuAirtimeOrderSagaEvent { get; private set; }
    public Event<SecondRetryVtuAirtimeOrderEvent> SecondRetryVtuAirtimeOrderEvent { get; private set; }

    public Event<BuyAirtimeForCustomerSecondReTryFailedEvent> BuyAirtimeForCustomerSecondReTryFailedEvent { get; private set; }
    public Event<NotifyCustomerOfVtuAirtimePurchaseFailedEvent> NotifyCustomerOfVtuAirtimePurchaseFailedEvent { get; private set; }
    public Event<RollbackAmountForVtuAirtimePurchaseFailedMessage> RollbackAmountForVtuAirtimePurchaseFailedMessage { get; private set; }
    public Event<NotifyCustomerOfVtuAirtimePurchaseSuccessEvent> NotifyCustomerOfVtuAirtimePurchaseSuccessEvent { get; private set; }
    public Event<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage> DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage { get; private set; }



    // FAULTS
    public Event<Fault<BuyAirtimeForCustomerMessage>> BuyAirtimeForCustomerMessageFaulted {  get; private set; }
    public Event<Fault<SecondRetryVtuAirtimeOrderEvent>> SecondRetryVtuAirtimeOrderEventFaulted {  get; private set; }
    public Event<Fault<RollbackAmountForVtuAirtimePurchaseFailedMessage>> RollbackAmountForVtuAirtimePurchaseFailedMessageFaulted {  get; private set; }
    public Event<Fault<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage>> DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageFaulted {  get; private set; }


}
