
using Microsoft.Data.SqlClient;

namespace RoomChatApi.Commands
{
    public static class UserCommands
    {
        public static async Task UpdateUserIsConnected(string userId, int roomId, bool isConnected, string? connectionId)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=VirtualLibrary;Trusted_Connection=True;TrustServerCertificate=True";

            const string query = @"
            UPDATE StudyRoomUsers
            SET IsConnected = @IsConnected, ConnectionId = @ConnectionId
            WHERE UserId = @UserId AND StudyRoomId = @StudyRoomId";

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@StudyRoomId", roomId);
            command.Parameters.AddWithValue("@IsConnected", isConnected);
            command.Parameters.AddWithValue("@ConnectionId", (object?)connectionId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }
    }
}

