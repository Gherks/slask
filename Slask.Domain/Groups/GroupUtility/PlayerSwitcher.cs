using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Groups.GroupUtility
{
    public static class PlayerSwitcher
    {
        public static bool SwitchMatchesOn(Match match1, Guid playerReference1Id, Match match2, Guid playerReference2Id)
        {
            if (SwitchIsPossible(match1, playerReference1Id, match2, playerReference2Id))
            {
                bool playersResidesInSameMatch = match1.Id == match2.Id;

                if (playersResidesInSameMatch)
                {
                    MakeSwitchOnPlayerReferencesInSameMatch(match1);
                }
                else
                {
                    MakeSwitchOnPlayerReferencesInDifferentMatch(match1, playerReference1Id, match2, playerReference2Id);
                }

                match1.Group.RemoveAllMatchBetsOnMatch(match1);
                match2.Group.RemoveAllMatchBetsOnMatch(match2);

                return true;
            }

            return false;
        }

        private static bool SwitchIsPossible(Match match1, Guid playerReference1Id, Match match2, Guid playerReference2Id)
        {
            bool eitherMatchIsInvalid = match1 == null || match2 == null;

            if (eitherMatchIsInvalid)
            {
                // LOG Error: Either player that is attempting to switch match is invalid
                return false;
            }

            bool eitherPlayerReferenceIsInvalid = playerReference1Id == Guid.Empty || playerReference2Id == Guid.Empty;

            if (eitherPlayerReferenceIsInvalid)
            {
                // LOG Error: Either or both players that are switching matches has invalid player references
                return false;
            }

            if (EitherMatchGroupHasBegun(match1, match2))
            {
                // LOG Issue?: Either player that wants to switch matches is residing in a group that has already started, it's too late to switch matches with other players
                return false;
            }

            if (EitherPlayerResidesInGroupThatDisallowSwitching(match1, match2))
            {
                // LOG Issue?: Either player that wants to switch matches resides in a group that does not allow switching
                return false;
            }

            return true;
        }

        private static bool EitherMatchGroupHasBegun(Match match1, Match match2)
        {
            bool match1HasBegun = match1.Group.GetPlayState() != PlayStateEnum.NotBegun;
            bool match2HasBegun = match2.Group.GetPlayState() != PlayStateEnum.NotBegun;

            return match1HasBegun || match2HasBegun;
        }

        private static bool EitherPlayerResidesInGroupThatDisallowSwitching(Match match1, Match match2)
        {
            bool match1GroupDisallowsSwitching = match1.Group is RoundRobinGroup;
            bool match2GroupDisallowsSwitching = match2.Group is RoundRobinGroup;

            return match1GroupDisallowsSwitching && match2GroupDisallowsSwitching;
        }

        private static void MakeSwitchOnPlayerReferencesInSameMatch(Match match)
        {
            match.AssignPlayerReferencesToPlayers(match.PlayerReference2Id, match.PlayerReference1Id);
        }

        private static void MakeSwitchOnPlayerReferencesInDifferentMatch(Match match1, Guid playerReference1Id, Match match2, Guid playerReference2Id)
        {
            if (match1.PlayerReference1Id == playerReference1Id)
            {
                match1.AssignPlayerReferencesToPlayers(playerReference2Id, match1.PlayerReference2Id);
            }
            else
            {
                match1.AssignPlayerReferencesToPlayers(match1.PlayerReference1Id, playerReference2Id);
            }

            if (match2.PlayerReference1Id == playerReference2Id)
            {
                match2.AssignPlayerReferencesToPlayers(playerReference1Id, match2.PlayerReference2Id);
            }
            else
            {
                match2.AssignPlayerReferencesToPlayers(match2.PlayerReference1Id, playerReference1Id);
            }
        }
    }
}
