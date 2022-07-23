using MediatR;

namespace Auth.Application.Commands.FeatureAuth;

public record LogoutCommand(string token) : IRequest;