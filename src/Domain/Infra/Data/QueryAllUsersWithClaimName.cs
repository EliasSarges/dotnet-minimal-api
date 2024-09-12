using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Domain.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);

        var query = @"select U.Email,
                     UC.ClaimValue as Name
            from AspNetUsers U
                     inner join AspNetUserClaims UC on UC.UserId = U.Id
            where UC.ClaimType = 'Name'
            order by 1
            offset (@page - 1) * @rows rows fetch next @rows rows only";

        return db.Query<EmployeeResponse>(query, new { page, rows });
    }
}
