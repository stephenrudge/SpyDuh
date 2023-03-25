using Microsoft.Data.SqlClient;
using SpyDuhLakers.Models;
using SpyDuhLakers.Utils;
using System.Diagnostics.Metrics;

namespace SpyDuhLakers.Repositories
{
    public class EnemyRepository : BaseRepository, IEnemyRepository
    {
        public EnemyRepository(IConfiguration configuration) : base(configuration) { }

        public Enemy GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, UserId, enemyId FROM Enemies WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", Id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Enemy enemy = null;
                    if (reader.Read())
                    {
                        enemy = new Enemy()
                        {
                            Id = Id,
                            userId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            enemyId = reader.GetInt32(reader.GetOrdinal("enemyId"))
                        };
                    }
                    reader.Close();
                    return enemy;
                }
            }
        }


        public void Insert(Enemy enemy)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Enemies (userId, enemyId) 
                                OUTPUT INSERTED.Id 
                                VALUES (@userId, @enemyId)";
                    cmd.Parameters.AddWithValue("@userId", enemy.userId);
                    cmd.Parameters.AddWithValue("@enemyId", enemy.enemyId);
                    int id = (int)cmd.ExecuteScalar();

                    id = enemy.Id;
                }
            }
        }

        public void Update(Enemy enemy)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Enemies SET 
                                    userId = @userId,
                                    enemyId = @enemyId
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@userId", enemy.userId);
                    cmd.Parameters.AddWithValue("@enemyId", enemy.enemyId);
                    cmd.Parameters.AddWithValue("@id", enemy.Id);
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
                    cmd.CommandText = @"DELETE FROM Enemies WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }





    }

}
