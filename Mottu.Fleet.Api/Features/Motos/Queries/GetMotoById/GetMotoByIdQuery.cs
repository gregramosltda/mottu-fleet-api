using MediatR;
using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Features.Motos.Queries.GetMotoById;

public record GetMotoByIdQuery(int Id) : IRequest<Moto>;

