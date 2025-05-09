using System.Text.RegularExpressions;
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
        int countRowsAdded = -1;
        if (IsValidPersonId(adventurer.PersonId))
        {

            const string insertString =
                "INSERT INTO Adventurer (Nickname, RaceId, ExperienceId, PersonId) VALUES (@Nickname, @RaceId, @ExperienceId, @PersonId)";
            

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
        }

        return countRowsAdded != -1;
    }
    
    public static bool IsValidPersonId(string personId)
    {
        if (personId == null || personId.Length != 16)
            return false;
        
        var regex = new Regex(@"^[A-Z]{2}(\d{4})(\d{2})(\d{2})(\d{4})[A-Z]{2}$");
        var match = regex.Match(personId);

        if (!match.Success)
            return false;

        int year = int.Parse(match.Groups[1].Value);
        int month = int.Parse(match.Groups[2].Value);
        int day = int.Parse(match.Groups[3].Value);
        int noise = int.Parse(match.Groups[4].Value);

        return year >= 1 && year <= 9999 &&
               month >= 1 && month <= 11 &&
               day >= 1 && day <= 28 &&
               noise >= 0 && noise <= 9999;
    }
}