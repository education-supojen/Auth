using Auth.Application.DTO;
using Auth.Application.Queries;
using Auth.Domain.Aggregates;
using Auth.Infrastructure.Persistence.Dapper;
using Dapper;
using MediatR;

namespace Auth.Infrastructure.Queries;

public class SearchUserByQrCodeHandler : IRequestHandler<SearchUserByQrCodeQuery,UserDto>
{
    private readonly IMiniORMDatabase _miniOrmDatabase;

    public SearchUserByQrCodeHandler(IMiniORMDatabase miniOrmDatabase)
    {
        _miniOrmDatabase = miniOrmDatabase;
    }
    
    public async Task<UserDto> Handle(SearchUserByQrCodeQuery request, CancellationToken cancellationToken)
    {
        return await _miniOrmDatabase.Connection.QueryFirstOrDefaultAsync($@"select * from user where id = {request.QrCode}");
    }
}