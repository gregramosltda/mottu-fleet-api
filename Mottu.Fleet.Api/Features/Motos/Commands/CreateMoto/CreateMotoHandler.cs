using FluentValidation;
using MediatR;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;

namespace Mottu.Fleet.Api.Features.Motos.Commands.CreateMoto;

public class CreateMotoHandler : IRequestHandler<CreateMotoCommand, Moto>
{
    private readonly IMotoRepository _repository;
    private readonly IValidator<CreateMotoRequest> _validator;

    public CreateMotoHandler(IMotoRepository repository, IValidator<CreateMotoRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Moto> Handle(CreateMotoCommand command, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validation.IsValid)
            throw new Mottu.Fleet.Api.Exceptions.ValidationException(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var moto = new Moto
        {
            Placa = command.Request.Placa,
            Modelo = command.Request.Modelo,
            Status = command.Request.Status,
            Quilometragem = command.Request.Quilometragem,
            Bateria = command.Request.Bateria,
            UltimaAtualizacao = DateTime.UtcNow
        };

        return await _repository.AddAsync(moto);
    }
}

