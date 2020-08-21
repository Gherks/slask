﻿namespace Slask.Domain.Utilities
{
    public enum ContestTypeEnum
    {
        None = 0,
        Bracket = 1,
        DualTournament = 2,
        RoundRobin = 3
    }

    public enum BetTypeEnum
    {
        None = 0,
        MatchBet = 1,
        MiscellaneousBet = 2
    }

    public enum PlayStateEnum
    {
        NotBegun = 0,
        Ongoing = 1,
        Finished = 2
    }
}
