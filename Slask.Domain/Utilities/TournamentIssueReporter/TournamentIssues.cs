namespace Slask.Domain.Utilities
{
    public enum TournamentIssues
    {
        NotFillingAllGroupsWithPlayers,
        RoundDoesNotSynergizeWithPreviousRound,
        AdvancersCountInRoundIsGreaterThanParticipantCount,
        LastRoundContainsMoreThanOneGroup,
        LastRoundHasMoreThanOneAdvancers,
        StartDateTimeIsInThePast,
        StartDateTimeIncompatibleWithPreviousRound,
        StartDateTimeIncompatibleWithGroupRules
    }
}
