using System.Runtime.CompilerServices;
using Data;
[assembly: InternalsVisibleTo("UnitTests")]
namespace Services.Interfaces
{
    public interface ILevelsService
    {
        int GetNextLevelToPlay();
        void FinishLevel(bool levelCompleted);
        LevelDefinitionData GetLevelDefinition(int level);
    }
}
