using TavernSystem.Models;

namespace TavernSystem.Application;

public interface ITavernSystemService
{
    IEnumerable<Adventurer> GetAllAdventurers();
}