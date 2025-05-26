using Microsoft.Data.SqlClient;

namespace RoomChatApi.Queries
{
    public static class UserQueries
    {
        public static async Task<(string? UserId, string? UserName, int? RoomId)> GetUserByConnectionId(string connectionId, string connectionString)
        {

            const string query = @"
            SELECT UserId, UserName, StudyRoomId 
            FROM StudyRoomUsers
            INNER JOIN AspNetUsers ON StudyRoomUsers.UserId = AspNetUsers.Id 
            WHERE ConnectionId = @ConnectionId";

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ConnectionId", connectionId);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                string userId = reader["UserId"].ToString()!;
                string userName = reader["UserName"].ToString()!;
                int roomId = (int)reader["StudyRoomId"];
                return (userId, userName, roomId);
            }

            return (null, null, null);
        }

    }
}
