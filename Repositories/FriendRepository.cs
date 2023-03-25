using Microsoft.Data.SqlClient;
using SpyDuhLakers.Models;
using SpyDuhLakers.Utils;
using System.Diagnostics.Metrics;

namespace SpyDuhLakers.Repositories
{
    public class FriendRepository : BaseRepository, IFriendRepository
    {
        public FriendRepository(IConfiguration configuration) : base(configuration) { }

        public Friend GetById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, UserId, FriendId FROM Friends WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", Id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Friend friend = null;
                    if (reader.Read())
                    {
                        friend = new Friend()
                        {
                            Id = Id,
                            userId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            friendId = reader.GetInt32(reader.GetOrdinal("FriendId"))
                        };
                    }
                    reader.Close();
                    return friend;
                }
            }
        }


        public void Update(Friend friend)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Friends SET 
                                    userId = @userId,
                                    friendId = @friendId
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@userId", friend.userId);
                    cmd.Parameters.AddWithValue("@enemyId", friend.friendId);
                    cmd.Parameters.AddWithValue("@id", friend.Id);
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
                    cmd.CommandText = @"DELETE FROM FRIENDS WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }


        public void Insert(Friend friend)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Friends (userId, friendId) 
                                OUTPUT INSERTED.Id 
                                VALUES (@userId, @friendId)" +

                    @"INSERT INTO Friends (userId, friendId) 
                                OUTPUT INSERTED.Id 
                                VALUES (@friendId, @userId)";
                    cmd.Parameters.AddWithValue("@userId", friend.userId);
                    cmd.Parameters.AddWithValue("@friendId", friend.friendId);
                    friend.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
