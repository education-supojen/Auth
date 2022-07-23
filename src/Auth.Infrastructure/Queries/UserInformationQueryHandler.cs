using Auth.Application.DTO;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Queries;
using Auth.Infrastructure.Persistence.Dapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Queries;

public class UserInformationQueryHandler : IRequestHandler<UserInformationQuery, UserInformationDto>
{
    private IMiniORMDatabase _database;
    private IAesService _aesService;
    private IJwtTokenGenerator _jwtTokenGenerator;

    public UserInformationQueryHandler(IMiniORMDatabase database,IAesService aesService,IJwtTokenGenerator jwtTokenGenerator)
    {
        _database = database;
        _aesService = aesService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<UserInformationDto> Handle(UserInformationQuery request, CancellationToken cancellationToken)
    {
        var (sub, jti)= _jwtTokenGenerator.ReadSubAndJti(request.token);
        var id = long.Parse(_aesService.DecryptCBC(sub));
        return await _database.Connection.QueryFirstOrDefaultAsync<UserInformationDto>(@$"select * from app_user where id = {id}");
    }
}