using Cidco.Middleware.Application.Contracts.Persistance;
using Cidco.Middleware.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Cidco.Middleware.Persistense.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly CidcoMiddlewareDBContext _dbContext;
        private readonly ILogger _logger;
        public BaseRepository(CidcoMiddlewareDBContext dbContext, ILogger<T> logger)
        {
            _dbContext = dbContext; _logger = logger;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            _logger.LogInformation("ListAllAsync Initiated");
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async virtual Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size)
        {
            return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<T>> StoredProcedureQueryAsync(string storedProcedureName, SqlParameter[] parameters = null)
        {
            if (parameters == null)

                return await _dbContext.Set<T>().FromSqlRaw(storedProcedureName).ToListAsync();
            else
            {
                var parameterNames = GetParameterNames(parameters);
                SqlParameter idListParam = new SqlParameter("", parameters[0].Value);
                string sqlCommand = "";
                var result = await _dbContext.Set<T>().FromSqlRaw(sqlCommand, idListParam).ToListAsync();
                return result;
            }
        }

        public async Task<IList<T>> ExecuteStoredProcedure<T>(string storedProcedureName, Dictionary<string, object> parameters) where T : class
        {
            var sqlParameters = new List<SqlParameter>();
            var sqlQuery = $"EXEC {storedProcedureName} ";

            foreach (var param in parameters)
            {
                var sqlParameter = new SqlParameter(param.Key, param.Value ?? DBNull.Value);
                sqlParameters.Add(sqlParameter);
                sqlQuery += $"{param.Key}, ";
            }
            sqlQuery = sqlQuery.TrimEnd(',', ' ');

            return await _dbContext.Set<T>().FromSqlRaw(sqlQuery, sqlParameters.ToArray()).ToListAsync();
        }

        public async Task<int> InsertRecordAsync(string storedProcedureName, Dictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();
            var sqlQuery = $"EXEC {storedProcedureName} ";

            foreach (var param in parameters)
            {
                var sqlParameter = new SqlParameter(param.Key, param.Value ?? DBNull.Value);
                sqlParameters.Add(sqlParameter);
                sqlQuery += $"{param.Key}, ";
            }
            sqlQuery = sqlQuery.TrimEnd(',', ' ');
            var res = await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, sqlParameters);
            return res;
        }
        public async Task<int> InsertRecordAsyncWithOutParam(string storedProcedureName, Dictionary<string, Tuple<SqlDbType, object, ParameterDirection>> parameters)
        {
            var sqlParameters = new List<SqlParameter>();
            foreach (var param in parameters)
            {
                if(param.Value.Item3== ParameterDirection.Input)
                {
                    SqlParameter parm = new SqlParameter(param.Key, param.Value.Item1);
                    parm.Value = param.Value.Item2;
                    parm.Direction = param.Value.Item3;
                    sqlParameters.Add(parm);
                }
            }
            var outputParam = new SqlParameter(parameters.Last().Key, SqlDbType.Int) { Direction = ParameterDirection.Output };
            sqlParameters.Add(outputParam);
            //var res = await _dbContext.Database.ExecuteSqlRaw("EXEC UpdateEmployeeSalary "+ string.Join(",", parameters.Select(x => x.Key).ToArray()),
            //    sqlParameters.ToArray());

            //  var sqlParameters = new List<SqlParameter>();
            var sqlQuery = $"EXEC {storedProcedureName} ";

            
           // sqlQuery = sqlQuery.TrimEnd(',', ' ');
            var res = await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery + string.Join(",", parameters.Select(x => x.Key).ToArray()) +" OUTPUT", sqlParameters);
            int salary = (int)outputParam.Value;

            return salary;
        }

        private string[] GetParameterNames(SqlParameter[] parameters)
        {
            var parameterNames = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterNames[i] = parameters[i].ParameterName + "=" + parameters[i].Value;
            }
            return parameterNames;
        }

    }
}
