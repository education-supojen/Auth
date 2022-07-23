using Auth.Application.DTO;
using Auth.Domain.Aggregates;
using MediatR;

namespace Auth.Application.Queries;

public record SearchUserByQrCodeQuery(string QrCode) : IRequest<UserDto>;
