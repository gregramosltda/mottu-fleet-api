using MediatR;
using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Features.Motos.Queries.GetAllMotos;

public record GetAllMotosQuery : IRequest<IEnumerable<Moto>>;
