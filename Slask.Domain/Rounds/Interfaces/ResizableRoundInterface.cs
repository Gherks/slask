namespace Slask.Domain.Rounds.Interfaces
{
    public interface ResizableRoundInterface : RoundInterface
    {
        public bool SetPlayersPerGroupCount(int count);
    }
}
