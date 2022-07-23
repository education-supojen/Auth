using Auth.Domain.Aggregates;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Domain.Events.Staffs;

public record StaffLoginEvent(Staff staff) : IRequest<OneOf<bool,Failure>>;
