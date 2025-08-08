public interface IMiniGame
{
    void StartMiniGame(int difficultyLevel);
    void UpdateMiniGame();
    void CloseMiniGame();
    bool IsFinished { get; }
    bool IsSuccess { get; }
}
