using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace HttpServApp.Models
{
    internal class Repository : IRepository
    {
        public string ConnStr { get;  } = Configuration.DBConnStr;
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        private readonly NpgsqlConnection connection = new NpgsqlConnection();
        public Repository() {  }

        public Repository (string connStr)
        {
            this.ConnStr = connStr;
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
        }

        public HttpRequest? GetRequestById (long idRequest)
        {
            return Requests.Find(request => request.IdRequest == idRequest);
        }

        public List<HttpRequest> GetRequestsByPeriod(DateTime dateBeg, DateTime dateEnd)
        {
            return Requests.FindAll(r => r.DateTimeRequest >= dateBeg && r.DateTimeRequest <= dateEnd);
        }

        // Завантаження із БД, формування колекції Requests
        public void LoadFromDb()
        {
            try
            {
                connection.ConnectionString = ConnStr;
                connection.Open();
                NpgsqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT ""Id_Request"", ""DateTime_Request"", ""Version"", ""Method"", ""Ip_Address"", ""Status"", ""Content_Type_Request""" +
	                        @" FROM public.""Http_Request""";
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        HttpRequest request = new HttpRequest(this,
                            Convert.ToDateTime(dataReader["DateTime_Request"]),
                            dataReader["Version"] != DBNull.Value ? Convert.ToString(dataReader["Version"]) : null,
                            dataReader["Method"] != DBNull.Value ? Convert.ToString(dataReader["Method"]) : null,
                            Convert.ToString(dataReader["Ip_Address"]),
                            dataReader["Content_Type_Request"] != DBNull.Value ? Convert.ToString(dataReader["Content_Type_Request"]) : null,
                            Convert.ToInt64(dataReader["Id_Request"]))
                        {
                            Status = (StatusEnum)Convert.ToInt32(dataReader["Status"])
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
                connection.ConnectionString = ConnStr;
                connection.Open();
                NpgsqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string query = string.Empty;
                switch (typeOper)
                {
                    case '+':
                        query = @"INSERT INTO public.""Http_Request""(" +
                            @"""DateTime_Request"", ""Version"", ""Method"", ""Ip_Address"", ""Status"", ""Content_Type_Request"")" +
                            @" VALUES ($1, $2, $3, $4, $5, $6)";
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.DateTimeOffset, Value = httpRequest.DateTimeRequest });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.String, Value = httpRequest.Version == null ? DBNull.Value : httpRequest.Version });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.String, Value = httpRequest.Method == null ? DBNull.Value : httpRequest.Method });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.String, Value = httpRequest.IpAddress });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int32, Value = (int)httpRequest.Status });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.String, Value = httpRequest.ContentTypeRequest == null ? DBNull.Value : httpRequest.ContentTypeRequest });
                        break;

                    case '-':
                        query = @"DELETE FROM public.""Http_Request""" +
                                @" WHERE ""Id_Request"" = $1";
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int64, Value = httpRequest.IdRequest });
                        break;

                    case '=':
                        query = @"UPDATE public.""Http_Request""" +
	                            @" SET ""Status""= $1" +
	                            @" WHERE ""Id_Request""= $2";
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int32, Value = (int)httpRequest.Status });
                        cmd.Parameters.Add(new NpgsqlParameter() { DbType = System.Data.DbType.Int64, Value = httpRequest.IdRequest });
                        break;

                    default:
                        break;
                }
                if (string.IsNullOrEmpty(query))
                    return;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                // Отримання ідентифікатора з бази даних для нового об'єкта
                if (typeOper == '+')
                {
                    cmd.CommandText = @"SELECT currval('""Http_Request_Seq_Id""')  as id";
                    cmd.Parameters.Clear();
                    long newIdRequest = Convert.ToInt64(cmd.ExecuteScalar());
                    httpRequest.IdRequest = newIdRequest;

                }
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
