using AutoMapper;
using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserCreatedSagaStateInstance, UserCreatedSagOrchestratorInstanceResponseDto>().ReverseMap();

        CreateMap<VtuAirtimeOrderedSagaStateInstance, VtuAirtimeSagaOrchestratorInstanceResponseDto>().ReverseMap();

        CreateMap<VtuDataOrderedSagaStateInstance, VtuDataSagaOrchestratorInstanceResponseDto>().ReverseMap();
    }
}
