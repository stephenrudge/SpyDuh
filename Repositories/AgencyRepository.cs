using Microsoft.Data.SqlClient;
using SpyDuhLakers.Models;
using SpyDuhLakers.Utils;

namespace SpyDuhLakers.Repositories
{
    public class AgencyRepository : BaseRepository, IAgencyRepository
    {
        public AgencyRepository(IConfiguration configuration) : base(configuration) { }

        public List<Agency> GetAllAgencies()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select 
                                        id AS 'AgencyId'
                                        ,[name] AS 'AgencyName'
                                        From Agency";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Agency> agencies = new List<Agency>();

                    while (reader.Read())
                    {
                        agencies.Add(new Agency()
                        {
                            Id = DbUtils.GetInt(reader, "AgencyId"),
                            Name = DbUtils.GetString(reader, "AgencyName"),
                        });
                    }

                    reader.Close();
                    return agencies;
                }
            }
        }



        public Agency GetAgencyById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select id
                                        ,Name
                                        From Agency
                                        Where id = @id";
                                            
                    cmd.Parameters.AddWithValue("@id", Id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Agency agency = null;
                    if (reader.Read())
                    {
                        agency = new Agency()
                        {
                            Id = Id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),


                        };
                    }
                    reader.Close();
                    return agency;
                }
            }
        }


        public void Insert(Agency agency)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Agency 
                                        ([Name])
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue(@"name", agency.Name);
                    agency.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Agency agency)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Agency SET 
                                    Name = @Name
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", agency.Id);
                    cmd.Parameters.AddWithValue("@Name", agency.Name);
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
                    cmd.CommandText = @"DELETE FROM Agency WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }


        public List<User> GetUserByAgency(string agency)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select u.id, 
		                                u.[name] as 'Spy Name',
		                                a.[name] as 'Agency Name'	
                                        From Users u
                                        join Agency A on u.id = a.id
                                        Where a.name = @name";
                    cmd.Parameters.AddWithValue("@name", agency);
                    List<User> matchedAgency = new List<User>();
                    SqlDataReader reader = cmd.ExecuteReader();
                    User matchedUser = null;

                    while (reader.Read())
                    {
                        matchedUser = new User()
                        {
                            Id = DbUtils.GetInt(reader, "id"),
                            Name = DbUtils.GetString(reader, "name")
                        };
                        matchedAgency.Add(matchedUser);
                    }

                    reader.Close();
                    return matchedAgency;
                }
            }
        }
    }
}