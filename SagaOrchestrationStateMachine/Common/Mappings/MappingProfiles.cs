using AutoMapper;
using SagaOrchestrationStateMachines.Common.DTO;
using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator;

namespace SagaOrchestrationStateMachines.Common.Mappings;

internal sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserCreatedSagaStateInstance, UserCreatedSagOrchestratorInstanceResponseDto>().ReverseMap();

        CreateMap<VtuAirtimeOrderedSagaStateInstance, VtuAirtimeSagaOrchestratorInstanceResponseDto>().ReverseMap();

        CreateMap<VtuDataOrderedSagaStateInstance, VtuDataSagaOrchestratorInstanceResponseDto>().ReverseMap();
    }
}
