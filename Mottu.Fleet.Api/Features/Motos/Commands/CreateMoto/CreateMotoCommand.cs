using MediatR;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Features.Motos.Commands.CreateMoto;

public record CreateMotoCommand(CreateMotoRequest Request) : IRequest<Moto>;
