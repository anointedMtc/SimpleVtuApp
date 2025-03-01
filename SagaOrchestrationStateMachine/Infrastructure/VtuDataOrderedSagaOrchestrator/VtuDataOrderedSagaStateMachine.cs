using MassTransit;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;
using SharedKernel.Common.Constants;
using VtuApp.Shared.IntegrationEvents;

namespace SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;

public sealed class VtuDataOrderedSagaStateMachine : MassTransitStateMachine<VtuDataOrderedSagaStateInstance>
{
    public VtuDataOrderedSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);


        Event(() => CustomerPurchasedDataVtuNationEvent, x => x.CorrelateById(context => context.Message.VtuTransactionId));
        Event(() => BuyDataForCustomerMessage, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => BuyDataForCustomerSuccessEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => BuyDataForCustomerFirstTryFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Schedule(() => ScheduleForSecondRetryVtuDataOrderSagaEvent, instance => instance.SecondRetryVtuDataScheduleEventTokenId, s =>
        {
            s.Delay = TimeSpan.FromMinutes(5);

            s.Received = r => r.CorrelateById(context => context.Message.VtuTransactionId);

        });
        Event(() => SecondRetryVtuDataOrderEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => BuyDataForCustomerSecondReTryFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => NotifyCustomerOfVtuDataPurchaseFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => RollbackAmountForVtuDataPurchaseFailedMessage, x =>
        {
            x.CorrelateById(context => context.Message.VtuTransactionId);
            x.OnMissingInstance(m => m.Redeliver(r =>
            {
                r.Interval(5, 1000);
                r.OnRedeliveryLimitReached(n => n.Fault());
            }));
        });
        Event(() => NotifyCustomerOfVtuDataPurchaseSuccessEvent, x =>
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

        Event(() => BuyDataForCustomerMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );
        Event(() => SecondRetryVtuDataOrderEventFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );
        Event(() => RollbackAmountForVtuDataPurchaseFailedMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );
        Event(() => DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageFaulted, x => x
            .CorrelateById(m => m.Message.Message.VtuTransactionId)
            .SelectId(m => m.Message.Message.VtuTransactionId)
        );


        Initially(
            When(CustomerPurchasedDataVtuNationEvent)
            .Then(context =>
            {
                context.Saga.ApplicationUserId = context.Message.ApplicationUserId;
                context.Saga.Email = context.Message.Email;
                context.Saga.FirstName = context.Message.FirstName;
                context.Saga.LastName = context.Message.LastName;
                context.Saga.VtuTransactionId = context.Message.VtuTransactionId;
                context.Saga.NetworkProvider = context.Message.NetworkProvider;
                context.Saga.DataPlanPurchased = context.Message.DataPlanPurchased;
                context.Saga.AmountToPurchase = context.Message.AmountPurchased;
                context.Saga.PricePaid = context.Message.PricePaid;
                context.Saga.Receiver = context.Message.Receiver;
                context.Saga.Sender = context.Message.Sender;
                context.Saga.InitialBalance = context.Message.InitialBalance;
                context.Saga.FinalBalance = context.Message.FinalBalance;
                context.Saga.CreatedAt = context.Message.CreatedAt;
            })
            .Send(new Uri($"queue:{QueueConstants.BuyDataForCustomerMessageQueueName}"),
                context => new BuyDataForCustomerMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.DataPlanPurchased,
                    context.Saga.AmountToPurchase,
                    context.Saga.Receiver)
                )
            .TransitionTo(BuyDataForCustomerCommandSentSagaState)
        );

        // SECOND STAGE
        During(BuyDataForCustomerCommandSentSagaState,
            When(BuyDataForCustomerSuccessEvent)
            .TransitionTo(SuccessVtuDataPurchaseSagaState)
        );
        During(BuyDataForCustomerCommandSentSagaState,
            When(BuyDataForCustomerFirstTryFailedEvent)
            .Schedule(ScheduleForSecondRetryVtuDataOrderSagaEvent,
                context => context.Init<PrepareForSecondRetryVtuDataOrderEvent>(new
                {
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.DataPlanPurchased,
                    context.Saga.AmountToPurchase,
                    context.Saga.Receiver
                })
            ),

            When(ScheduleForSecondRetryVtuDataOrderSagaEvent?.Received)
                .Publish(context => new SecondRetryVtuDataOrderEvent(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.DataPlanPurchased,
                    context.Saga.AmountToPurchase,
                    context.Saga.Receiver))
            .TransitionTo(WaitingForSecondVtuDataRetrySagaState)
        );

        // THIRD STAGE IF SECOND WASN'T SUCCESSFUL
        During(WaitingForSecondVtuDataRetrySagaState,
            When(BuyDataForCustomerSuccessEvent)
            .TransitionTo(SuccessVtuDataPurchaseSagaState)
        );
        During(WaitingForSecondVtuDataRetrySagaState,
            When(BuyDataForCustomerSecondReTryFailedEvent)
            .Send(new Uri($"queue:{QueueConstants.NotifyCustomerOfVtuDataPurchaseFailedEventQueueName}"),
                context => new NotifyCustomerOfVtuDataPurchaseFailedEvent(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.LastName,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.DataPlanPurchased,
                    context.Saga.AmountToPurchase,
                    context.Saga.PricePaid,
                    context.Saga.Receiver,
                    context.Saga.Sender,
                    context.Saga.InitialBalance,
                    context.Saga.FinalBalance,
                    context.Saga.CreatedAt)
                )
            .Send(new Uri($"queue:{QueueConstants.RollbackAmountForVtuDataPurchaseFailedMessageQueueName}"),
                context => new RollbackAmountForVtuDataPurchaseFailedMessage(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.DataPlanPurchased,
                    context.Saga.AmountToPurchase,
                    context.Saga.PricePaid,
                    context.Saga.Receiver,
                    context.Saga.InitialBalance,
                    context.Saga.FinalBalance,
                    context.Saga.CreatedAt)
                )
            .TransitionTo(FailedVtuDataPurchaseSagaState)
        );

        // FOURTH STAGE FOR EVERYBODY - WHEN SUCCESSFUL
        During(SuccessVtuDataPurchaseSagaState,
            When(BuyDataForCustomerSuccessEvent)
            .Send(new Uri($"queue:{QueueConstants.NotifyCustomerOfVtuDataPurchaseSuccessEventQueueName}"),
                context => new NotifyCustomerOfVtuDataPurchaseSuccessEvent(
                    context.Saga.ApplicationUserId,
                    context.Saga.Email,
                    context.Saga.FirstName,
                    context.Saga.LastName,
                    context.Saga.VtuTransactionId,
                    context.Saga.NetworkProvider,
                    context.Saga.DataPlanPurchased,
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
            .TransitionTo(VtuDataOrderedCompletedSagaState)
            .Finalize()
        );
    }


    public State BuyDataForCustomerCommandSentSagaState { get; private set; }
    public State SuccessVtuDataPurchaseSagaState { get; private set; }
    public State WaitingForSecondVtuDataRetrySagaState { get; private set; }
    public State FailedVtuDataPurchaseSagaState { get; private set; }
    public State VtuDataOrderedCompletedSagaState { get; private set; }

    public Event<CustomerPurchasedDataVtuNationEvent> CustomerPurchasedDataVtuNationEvent { get; private set; }
    public Event<BuyDataForCustomerMessage> BuyDataForCustomerMessage { get; private set; }
    public Event<BuyDataForCustomerSuccessEvent> BuyDataForCustomerSuccessEvent { get; private set; }
    public Event<BuyDataForCustomerFirstTryFailedEvent> BuyDataForCustomerFirstTryFailedEvent { get; private set; }

    public Schedule<VtuDataOrderedSagaStateInstance, PrepareForSecondRetryVtuDataOrderEvent> ScheduleForSecondRetryVtuDataOrderSagaEvent { get; private set; }
    public Event<SecondRetryVtuDataOrderEvent> SecondRetryVtuDataOrderEvent { get; private set; }

    public Event<BuyDataForCustomerSecondReTryFailedEvent> BuyDataForCustomerSecondReTryFailedEvent { get; private set; }
    public Event<NotifyCustomerOfVtuDataPurchaseFailedEvent> NotifyCustomerOfVtuDataPurchaseFailedEvent { get; private set; }
    public Event<RollbackAmountForVtuDataPurchaseFailedMessage> RollbackAmountForVtuDataPurchaseFailedMessage { get; private set; }
    public Event<NotifyCustomerOfVtuDataPurchaseSuccessEvent> NotifyCustomerOfVtuDataPurchaseSuccessEvent { get; private set; }
    public Event<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage> DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage { get; private set; }


    // FAULTS
    public Event<Fault<BuyDataForCustomerMessage>> BuyDataForCustomerMessageFaulted { get; private set; }
    public Event<Fault<SecondRetryVtuDataOrderEvent>> SecondRetryVtuDataOrderEventFaulted { get; private set; }
    public Event<Fault<RollbackAmountForVtuDataPurchaseFailedMessage>> RollbackAmountForVtuDataPurchaseFailedMessageFaulted { get; private set; }
    public Event<Fault<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage>> DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageFaulted { get; private set; }


}
