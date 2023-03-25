using Microsoft.Data.SqlClient;
using SpyDuhLakers.Models;
using SpyDuhLakers.Utils;

namespace SpyDuhLakers.Repositories
{
    public class SkillRepository : BaseRepository, ISkillRepository
    {
        public SkillRepository(IConfiguration configuration) : base(configuration) { }

        public List<Skill> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, name, userId FROM [Skills]";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Skill> skills = new List<Skill>();

                    while (reader.Read())
                    {
                        skills.Add(new Skill()
                        {
                            Id = DbUtils.GetInt(reader, "id"),
                            Name = DbUtils.GetString(reader, "name"),
                            UserId = DbUtils.GetInt(reader, "userId"),

                        });
                    }

                    reader.Close();
                    return skills;
                }
            }
        }


        //===============================================================

        public Skill GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                          Name,
                                          UserId 
                                          FROM Skills 
                                          WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", Id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Skill skill = null;
                    if (reader.Read())
                    {
                        skill = new Skill()
                        {
                            Id = Id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),


                        };
                    }
                    reader.Close();
                    return skill;
                }
            }
        }
        //=====================================================================

        public void Insert(Skill skill)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Skills
                                            ([Name], userId)
                                        OUTPUT
                                        INSERTED.Id
                                        VALUES
                                            (@name, @userId)";
                    cmd.Parameters.AddWithValue("@name", skill.Name);
                    cmd.Parameters.AddWithValue("userId", skill.UserId);

                    skill.Id = (int)cmd.ExecuteScalar();
                    
                }
                conn.Close();
            }
        }

        //=================================================


        public void Update(Skill skill)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Skills SET 
                                    userId = @userId
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@userId", skill.UserId);
                    ;
                    cmd.Parameters.AddWithValue("@id", skill.Id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        //=====================================================

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Skills WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }






    }
}