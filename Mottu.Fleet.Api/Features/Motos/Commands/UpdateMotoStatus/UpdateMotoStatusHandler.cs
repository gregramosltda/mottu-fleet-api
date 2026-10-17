using FluentValidation;
using MediatR;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;

namespace Mottu.Fleet.Api.Features.Motos.Commands.UpdateMotoStatus;

public class UpdateMotoStatusHandler : IRequestHandler<UpdateMotoStatusCommand, Moto>
{
    private readonly IMotoRepository _repository;
    private readonly IValidator<UpdateStatusRequest> _validator;

    public UpdateMotoStatusHandler(IMotoRepository repository, IValidator<UpdateStatusRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Moto> Handle(UpdateMotoStatusCommand command, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validation.IsValid)
            throw new Mottu.Fleet.Api.Exceptions.ValidationException(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var moto = await _repository.GetByIdAsync(command.Id);
        if (moto is null)
            throw new Mottu.Fleet.Api.Exceptions.NotFoundException($"Moto com id {command.Id} não encontrada");

        moto.Status = command.Request.Status;
        moto.UltimaAtualizacao = DateTime.UtcNow;
        return await _repository.UpdateAsync(moto);
    }
}
