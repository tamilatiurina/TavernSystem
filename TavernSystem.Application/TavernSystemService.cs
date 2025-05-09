using Microsoft.Data.SqlClient;
using TavernSystem.Models;

namespace TavernSystem.Application;

public class TavernSystemService: ITavernSystemService
{
    private string _connectionString;

    public TavernSystemService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Adventurer> GetAllAdventurers()
    {
        List<Adventurer> adventurers = [];
        const string queryString = "SELECT * FROM Adventurer";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var adventurerRow = new Adventurer
                        {
                            Id = reader.GetInt32(0),
                            Nickname = reader.GetString(1),
                        };
                        adventurers.Add(adventurerRow);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return adventurers;
    }

    public bool AddAdventurer(Adventurer adventurer)
    {
        const string insertString = "INSERT INTO Adventurer (Nickname, RaceId, ExperienceId, PersonId) VALUES (@Nickname, @RaceId, @ExperienceId, @PersonId)";

        int countRowsAdded = -1;
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(insertString, connection);
            command.Parameters.AddWithValue("@Nickname", adventurer.Nickname);
            command.Parameters.AddWithValue("@RaceId", adventurer.RaceId);
            command.Parameters.AddWithValue("@ExperienceId", adventurer.ExperienceId);
            command.Parameters.AddWithValue("@PersonId", adventurer.PersonId);
            connection.Open();
            countRowsAdded = command.ExecuteNonQuery();
        }
        
        return countRowsAdded != -1;
    }
}