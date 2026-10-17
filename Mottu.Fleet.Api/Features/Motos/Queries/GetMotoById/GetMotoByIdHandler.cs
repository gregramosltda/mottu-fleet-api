using MediatR;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;

namespace Mottu.Fleet.Api.Features.Motos.Queries.GetMotoById;

public class GetMotoByIdHandler : IRequestHandler<GetMotoByIdQuery, Moto>
{
    private readonly IMotoRepository _repository;

    public GetMotoByIdHandler(IMotoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Moto> Handle(GetMotoByIdQuery request, CancellationToken cancellationToken)
    {
        var moto = await _repository.GetByIdAsync(request.Id);
        if (moto is null)
            throw new Mottu.Fleet.Api.Exceptions.NotFoundException($"Moto com id {request.Id} não encontrada");

        return moto;
    }
}
