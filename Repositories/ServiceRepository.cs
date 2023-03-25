using Microsoft.Data.SqlClient;
using SpyDuhLakers.Models;
using SpyDuhLakers.Utils;

namespace SpyDuhLakers.Repositories
{
    public class ServiceRepository : BaseRepository, IServiceRepository
    {
        public ServiceRepository(IConfiguration configuration) : base(configuration) { }
        public List<Service> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select
                                        id
                                        ,[name]
                                        ,userId
                                        From [Services]";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Service> services = new List<Service>();

                    while (reader.Read())
                    {
                        services.Add(new Service()
                            {
                            Id = DbUtils.GetInt(reader, "id"),
                            Name = DbUtils.GetString(reader, "name"),
                            UserId = DbUtils.GetInt(reader, "userId"),
                        });
                    }
                    reader.Close();

                    return services;

                }
            }
        }


        public Service GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id
                                               ,[name]
                                               ,userId 
                                                FROM [Services] 
                                                WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", Id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Service service = null;
                    if (reader.Read())
                    {
                        service = new Service()
                        {
                            Id = Id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            UserId = reader.GetOrdinal("UserId"),
                        };
                    }
                    reader.Close();
                    return service;
                }

            }
        }


        public void Insert(Service service)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Insert INTO Services([Name], userId)
                                        OUTPUT INSERTED.Id 
                                        VALUES (@name, @userId)";
                    cmd.Parameters.AddWithValue("@name", service.Name);
                    cmd.Parameters.AddWithValue("@userId", service.UserId);
                    int id = (int)cmd.ExecuteScalar();

                }
                conn.Close();

            }
        }

        public void Update(Service service)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Services SET 
                                    userId = @userId,
                                    name = @name
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@userId", service.UserId);
                    cmd.Parameters.AddWithValue("@name", service.Name);
                    cmd.Parameters.AddWithValue("@id", service.Id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Services WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

    }
}

