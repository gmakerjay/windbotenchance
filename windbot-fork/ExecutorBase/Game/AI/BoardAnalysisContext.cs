using WindBot.Game;

namespace WindBot.Game.AI
{
    public class BoardAnalysisContext
    {
        public Executor Executor { get; private set; }
        public ClientField Bot { get; private set; }
        public ClientField Enemy { get; private set; }
        public BoardScorer Scorer { get; private set; }
        public Duel Duel { get; private set; }

        public BoardAnalysisContext(Executor executor)
        {
            Executor = executor;
            Bot = executor.Bot;
            Enemy = executor.Enemy;
            Scorer = executor.Scorer;
            Duel = executor.Duel;
        }
    }
}
