using MediatR;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Features.Motos.Commands.UpdateMotoStatus;

public record UpdateMotoStatusCommand(int Id, UpdateStatusRequest Request) : IRequest<Moto>;
