using Microsoft.Data.SqlClient;
using SpyDuhLakers.Models;
using SpyDuhLakers.Utils;
using System;
using System.Reflection.Metadata.Ecma335;

namespace SpyDuhLakers.Repositories
{

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public List<User> GetAllUsers()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT
                        u.id AS SpyId,
                        u.name AS SpyName,
                        u.agencyId AS AgencyId,
                        f.id AS FriendTableId,
                        f.friendId AS FriendUserId,
                        e.enemyId AS EnemyUserId,
                        e.id AS EnemyTableId,
                        sk.id AS SkillTableId,
                        sk.name AS SkillName,
                        sk.userId AS SkillUserId,
                        sv.id AS ServiceTableId,
                        sv.name AS ServiceName,
                        sv.userId AS ServiceUserId
                    FROM Users u
                        LEFT JOIN Friends f on u.id = f.userId
                        LEFT JOIN Enemies e on u.id = e.userId
                        LEFT JOIN Skills sk on u.id = sk.userId
                        LEFT JOIN Services sv on u.id = sv.userId
                        LEFT JOIN Users friend on friend.id = f.friendId
                        LEFT JOIN Users enemy on enemy.id = e.enemyId";

                    var reader = cmd.ExecuteReader();

                    var users = new List<User>();

                    while (reader.Read())
                    {
                        var userId = DbUtils.GetInt(reader, "SpyId");
                        var user = users.Where(u => u.Id == userId).FirstOrDefault();

                        if (user == null)
                        {
                            user = new User()
                            {
                                Id = DbUtils.GetInt(reader, "SpyId"),
                                Name = DbUtils.GetString(reader, "SpyName"),
                                AgencyId = DbUtils.GetInt(reader, "AgencyId"),
                                Enemies = new List<Enemy>(),
                                Friends = new List<Friend>(),
                                Skills = new List<Skill>(),
                                Services = new List<Service>(),
                                // Agency = new List<Agency>()
                            };
                            users.Add(user);
                        }

                        if (DbUtils.IsNotDbNull(reader, "EnemyTableId"))
                        {
                            var enemyTableId = DbUtils.GetInt(reader, "EnemyTableId");
                            var existingEnemy = user.Enemies.FirstOrDefault(e => e.Id == enemyTableId);

                            if (existingEnemy == null)
                            {
                                user.Enemies.Add(new Enemy()
                                {
                                    Id = DbUtils.GetInt(reader, "EnemyTableId"),
                                    userId = DbUtils.GetInt(reader, "SpyId"),
                                    enemyId = DbUtils.GetInt(reader, "EnemyUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "FriendTableId"))
                        {
                            var friendTableId = DbUtils.GetInt(reader, "FriendTableId");
                            var existingFriend = user.Friends.FirstOrDefault(f => f.Id == friendTableId);

                            if (existingFriend == null)
                            {
                                user.Friends.Add(new Friend()
                                {
                                    Id = friendTableId,
                                    userId = DbUtils.GetInt(reader, "SpyId"),
                                    friendId = DbUtils.GetInt(reader, "FriendUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "SkillTableId"))
                        {
                            var skillTableId = DbUtils.GetInt(reader, "SkillTableId");
                            var existingSkill = user.Skills.FirstOrDefault(s => s.Id == skillTableId);

                            if (existingSkill == null)
                            {
                                user.Skills.Add(new Skill()
                                {
                                    Id = skillTableId,
                                    Name = DbUtils.GetString(reader, "SkillName"),
                                    UserId = DbUtils.GetInt(reader, "SkillUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "ServiceTableId"))
                        {
                            var serviceTableId = DbUtils.GetInt(reader, "ServiceTableId");
                            var existingService = user.Services.FirstOrDefault(sv => sv.Id == serviceTableId);

                            if (existingService == null)
                            {
                                user.Services.Add(new Service()
                                {
                                    Id = serviceTableId,
                                    Name = DbUtils.GetString(reader, "ServiceName"),
                                    UserId = DbUtils.GetInt(reader, "ServiceUserId")
                                });
                            }
                        }
                    }

                    reader.Close();

                    return users;
                }
            }
        }

        public User GetUserbyId(int Id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                        u.id AS SpyId,
                        u.name AS SpyName,
                        u.agencyId AS AgencyId,
                        f.id AS FriendTableId,
                        f.friendId AS FriendUserId,
                        e.enemyId AS EnemyUserId,
                        e.id AS EnemyTableId,
                        sk.id AS SkillTableId,
                        sk.name AS SkillName,
                        sk.userId AS SkillUserId,
                        sv.id AS ServiceTableId,
                        sv.name AS ServiceName,
                        sv.userId AS ServiceUserId
                    FROM Users u
                        LEFT JOIN Friends f on u.id = f.userId
                        LEFT JOIN Enemies e on u.id = e.userId
                        LEFT JOIN Skills sk on u.id = sk.userId
                        LEFT JOIN Services sv on u.id = sv.userId
                        LEFT JOIN Users friend on friend.id = f.friendId
                        LEFT JOIN Users enemy on enemy.id = e.enemyId
                    WHERE u.id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", Id);

                    var reader = cmd.ExecuteReader();

                    User user = null;

                    if (reader.Read())
                    {
                        user = new User()
                        {
                            Id = DbUtils.GetInt(reader, "SpyId"),
                            Name = DbUtils.GetString(reader, "SpyName"),
                            AgencyId = DbUtils.GetInt(reader, "AgencyId"),
                            Enemies = new List<Enemy>(),
                            Friends = new List<Friend>(),
                            Skills = new List<Skill>(),
                            Services = new List<Service>()
                        };

                        if (DbUtils.IsNotDbNull(reader, "EnemyTableId"))
                        {
                            var enemyTableId = DbUtils.GetInt(reader, "EnemyTableId");
                            var existingEnemy = user.Enemies.FirstOrDefault(e => e.Id == enemyTableId);

                            if (existingEnemy == null)
                            {
                                user.Enemies.Add(new Enemy()
                                {
                                    Id = DbUtils.GetInt(reader, "EnemyTableId"),
                                    userId = DbUtils.GetInt(reader, "SpyId"),
                                    enemyId = DbUtils.GetInt(reader, "EnemyUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "FriendTableId"))
                        {
                            var friendTableId = DbUtils.GetInt(reader, "FriendTableId");
                            var existingFriend = user.Friends.FirstOrDefault(f => f.Id == friendTableId);

                            if (existingFriend == null)
                            {
                                user.Friends.Add(new Friend()
                                {
                                    Id = friendTableId,
                                    userId = DbUtils.GetInt(reader, "SpyId"),
                                    friendId = DbUtils.GetInt(reader, "FriendUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "SkillTableId"))
                        {
                            var skillTableId = DbUtils.GetInt(reader, "SkillTableId");
                            var existingSkill = user.Skills.FirstOrDefault(s => s.Id == skillTableId);

                            if (existingSkill == null)
                            {
                                user.Skills.Add(new Skill()
                                {
                                    Id = skillTableId,
                                    Name = DbUtils.GetString(reader, "SkillName"),
                                    UserId = DbUtils.GetInt(reader, "SkillUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "ServiceTableId"))
                        {
                            var serviceTableId = DbUtils.GetInt(reader, "ServiceTableId");
                            var existingService = user.Services.FirstOrDefault(sv => sv.Id == serviceTableId);

                            if (existingService == null)
                            {
                                user.Services.Add(new Service()
                                {
                                    Id = serviceTableId,
                                    Name = DbUtils.GetString(reader, "ServiceName"),
                                    UserId = DbUtils.GetInt(reader, "ServiceUserId")
                                });
                            }
                        }
                    }

                    reader.Close();

                    return user;
                }
            }
        }


        public void Insert(User user)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Users ([Name], agencyId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name, @AgencyId)";
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@AgencyId", user.AgencyId);
                    user.Id = (int)cmd.ExecuteScalar();
                }
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
		                                a.[name] as 'Agency Name',
                                        u.agencyId as 'Agency Id'	
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
                            Name = DbUtils.GetString(reader, "Agency Name"),
                            AgencyId = DbUtils.GetInt(reader, "Agency Id")
                        };
                        matchedAgency.Add(matchedUser);
                    }

                    reader.Close();
                    return matchedAgency;
                }
            }
        }

        public List<User> GetUserBySkill(string skill)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            u.id AS SpyId,
                                            u.name AS SpyName,
                                            u.agencyId AS AgencyId,
                                            f.id AS FriendTableId,
                                            f.friendId AS FriendUserId,
                                            e.enemyId AS EnemyUserId,
                                            e.id AS EnemyTableId,
                                            sk.id AS SkillTableId,
                                            sk.name AS SkillName,
                                            sk.userId AS SkillUserId,
                                            sv.id AS ServiceTableId,
                                            sv.name AS ServiceName,
                                            sv.userId AS ServiceUserId
                                        FROM Users u
                                            LEFT JOIN Friends f on u.id = f.userId
                                            LEFT JOIN Enemies e on u.id = e.userId
                                            LEFT JOIN Skills sk on u.id = sk.userId
                                            LEFT JOIN Services sv on u.id = sv.userId
                                            LEFT JOIN Users friend on friend.id = f.friendId
                                            LEFT JOIN Users enemy on enemy.id = e.enemyId
                                        WHERE sk.name = @name";
                    cmd.Parameters.AddWithValue("@name", skill);
                    List<User> matchedList = new List<User>();
                    SqlDataReader reader = cmd.ExecuteReader();
                    User matchedUser = null;

                    while (reader.Read())
                    {
                        var userId = DbUtils.GetInt(reader, "SpyId");
                        var user = matchedList.Where(u => u.Id == userId).FirstOrDefault();

                        if (user == null)
                        {
                            matchedUser = new User()
                            {
                                Id = DbUtils.GetInt(reader, "SpyId"),
                                Name = DbUtils.GetString(reader, "SpyName"),
                                AgencyId = DbUtils.GetInt(reader, "AgencyId"),
                                Enemies = new List<Enemy>(),
                                Friends = new List<Friend>(),
                                Skills = new List<Skill>(),
                                Services = new List<Service>()
                            };
                            matchedList.Add(matchedUser);
                        }

                        if (DbUtils.IsNotDbNull(reader, "EnemyTableId"))
                        {
                            var enemyTableId = DbUtils.GetInt(reader, "EnemyTableId");
                            var existingEnemy = matchedUser.Enemies.FirstOrDefault(e => e.Id == enemyTableId);

                            if (existingEnemy == null)
                            {
                                matchedUser.Enemies.Add(new Enemy()
                                {
                                    Id = DbUtils.GetInt(reader, "EnemyTableId"),
                                    userId = DbUtils.GetInt(reader, "SpyId"),
                                    enemyId = DbUtils.GetInt(reader, "EnemyUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "FriendTableId"))
                        {
                            var friendTableId = DbUtils.GetInt(reader, "FriendTableId");
                            var existingFriend = matchedUser.Friends.FirstOrDefault(f => f.Id == friendTableId);

                            if (existingFriend == null)
                            {
                                matchedUser.Friends.Add(new Friend()
                                {
                                    Id = friendTableId,
                                    userId = DbUtils.GetInt(reader, "SpyId"),
                                    friendId = DbUtils.GetInt(reader, "FriendUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "SkillTableId"))
                        {
                            var skillTableId = DbUtils.GetInt(reader, "SkillTableId");
                            var existingSkill = matchedUser.Skills.FirstOrDefault(s => s.Id == skillTableId);

                            if (existingSkill == null)
                            {
                                matchedUser.Skills.Add(new Skill()
                                {
                                    Id = skillTableId,
                                    Name = DbUtils.GetString(reader, "SkillName"),
                                    UserId = DbUtils.GetInt(reader, "SkillUserId")
                                });
                            }
                        }

                        if (DbUtils.IsNotDbNull(reader, "ServiceTableId"))
                        {
                            var serviceTableId = DbUtils.GetInt(reader, "ServiceTableId");
                            var existingService = matchedUser.Services.FirstOrDefault(sv => sv.Id == serviceTableId);

                            if (existingService == null)
                            {
                                matchedUser.Services.Add(new Service()
                                {
                                    Id = serviceTableId,
                                    Name = DbUtils.GetString(reader, "ServiceName"),
                                    UserId = DbUtils.GetInt(reader, "ServiceUserId")
                                });
                            }
                        }
                    }

                    reader.Close();
                    return matchedList;
                }
            }
        }


    }
}
