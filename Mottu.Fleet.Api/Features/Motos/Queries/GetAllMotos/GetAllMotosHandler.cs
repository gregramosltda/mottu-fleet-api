using MediatR;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;

namespace Mottu.Fleet.Api.Features.Motos.Queries.GetAllMotos;

public class GetAllMotosHandler : IRequestHandler<GetAllMotosQuery, IEnumerable<Moto>>
{
    private readonly IMotoRepository _repository;

    public GetAllMotosHandler(IMotoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Moto>> Handle(GetAllMotosQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}
