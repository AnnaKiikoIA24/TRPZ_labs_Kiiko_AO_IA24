using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace HttpServApp.Models
{
    internal class DBContext : IDbContext
    {
        private readonly string сonnStr = Configuration.DBConnStr;
        private readonly NpgsqlConnection connection = new NpgsqlConnection();
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        public DBContext() { }

        public DBContext (string connStr)
        {
            this.сonnStr = connStr;
        }

        public void AddRequest (HttpRequest request)
        { 
            Requests.Add (request);
        }

        public void RemoveRequest (HttpRequest request)
        {
            if (Requests.Contains(request))
                Requests.Remove(request);
            else throw new Exception("Object not found in collection");
        }

        public void UpdateRequest (HttpRequest request)
        {
            HttpRequest? existRequest = GetRequestById(request.IdRequest);
            if (existRequest == null)
                throw new Exception("Updatable object not found in collection");
            existRequest.Status = request.Status;
            existRequest.DateResponse = request.DateResponse;
        }

        public HttpRequest? GetRequestById (long idRequest)
        {
            return Requests.Find(request => request.IdRequest == idRequest);
        }

        public List<HttpRequest> GetRequestsByPeriod(DateTime dateBeg, DateTime dateEnd)
        {
            return Requests.FindAll(r => r.DateRequest >= dateBeg && r.DateRequest <= dateEnd);
        }

        // Завантаження із БД, формування колекції Requests
        public void LoadFromDb()
        {
            try
            {
                connection.ConnectionString = сonnStr;
                connection.Open();
                NpgsqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT ""Id_Request"", ""Path"", ""Version"", ""Content_Type"", ""Method"", ""Date_Request"", ""Body"", ""Content_Length"", " +
                            @"""Status"", ""Date_Response""" +
	                        @" FROM public.""Http_Request""";
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        HttpRequest request = new HttpRequest(
                            dataReader["Path"] != DBNull.Value ? Convert.ToString(dataReader["Path"]) : null,
                            dataReader["Version"] != DBNull.Value ? Convert.ToString(dataReader["Version"]) : null,
                            dataReader["Content_Type"] != DBNull.Value ? Convert.ToString(dataReader["Content_Type"]) : null,
                            dataReader["Method"] != DBNull.Value ? Convert.ToString(dataReader["Method"]) : null,
                            dataReader["Content_Length"] != DBNull.Value ? Convert.ToInt32(dataReader["Content_Length"]) : null,
                            Convert.ToDateTime(dataReader["Date_Request"]),
                            dataReader["Body"] != DBNull.Value ? Convert.ToString(dataReader["Body"]) : null,
                            Convert.ToInt64(dataReader["Id_Request"]))
                        {
                            Status = (StatusEnum)Convert.ToInt32(dataReader["Status"]),
                            DateResponse = Convert.ToDateTime(dataReader["Date_Response"])
                        };
                        AddRequest(request);

                    }
                    dataReader.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"SELECT error: {ex.Message}");
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
        }

        public void SaveToDB(HttpRequest httpRequest, char typeOper)
        {
            try
            {
                connection.ConnectionString = сonnStr;
                connection.Open();
                NpgsqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string query = string.Empty;
                switch (typeOper)
                {
                    case '+':
                        query = @"INSERT INTO public.""Http_Request""(" +
	                        @"""Path"", ""Version"", ""Content_Type"", ""Method""," +
                            @"""Date_Request"", ""Body"", ""Content_Length"", ""Status"", ""Date_Response"")" +
                            @" VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)";
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = httpRequest.Path == null ? DBNull.Value : httpRequest.Path });
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = httpRequest.Version == null ? DBNull.Value : httpRequest.Version });
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = httpRequest.ContentType == null ? DBNull.Value : httpRequest.ContentType });
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = httpRequest.Method == null ? DBNull.Value : httpRequest.Method });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Date, Value = httpRequest.DateRequest });
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = httpRequest.Body == null ? DBNull.Value : httpRequest.Body });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int32, Value = httpRequest.ContentLength == null ? DBNull.Value : httpRequest.ContentLength });
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = (int)httpRequest.Status });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Date, Value = httpRequest.DateResponse });
                        break;

                    case '-':
                        query = @"DELETE FROM public.""Http_Request""" +
                                @" WHERE ""Id_Request"" = $1";
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int64, Value = httpRequest.IdRequest });
                        break;

                    case '=':
                        query = @"UPDATE public.""Http_Request""" +
	                            @" SET ""Status""= $1, ""Date_Response""= $2" +
	                            @" WHERE ""Id_Request""= $3";
                        cmd.Parameters.Add(new NpgsqlParameter() { Value = (int)httpRequest.Status });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Date, Value = httpRequest.DateResponse });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int64, Value = httpRequest.IdRequest });
                        break;

                    default:
                        break;
                }
                if (string.IsNullOrEmpty(query))
                    return;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"DML error: {ex.Message}");
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
        }
    }
}
