using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain.Groups
{
    public static class PlayerSwitcher
    {
        public static bool SwitchMatchesOn(Player player1, Player player2)
        {
            if (SwitchIsPossible(player1, player2))
            {
                bool bothPlayersResidesInSameMatch = player1.Match.Id == player2.Match.Id;

                if (bothPlayersResidesInSameMatch)
                {
                    MakeSwitchOnPlayerReferencesInSameMatch(player1.Match);
                }
                else
                {
                    MakeSwitchOnPlayerReferencesInDifferentMatch(player1, player2);
                }

                player1.Match.Group.RemoveAllMatchBetsOnMatch(player1.Match);
                player2.Match.Group.RemoveAllMatchBetsOnMatch(player2.Match);

                return true;
            }

            return false;
        }

        private static bool SwitchIsPossible(Player player1, Player player2)
        {
            if (EitherPlayerIsInvalid(player1, player2))
            {
                // LOG Error: Either player that is attempting to switch match is invalid
                return false;
            }

            if (EitherPlayerHasAnInvalidPlayerReference(player1, player2))
            {
                // LOG Error: Either or both players that are switching matches has invalid player references
                return false;
            }

            if (EitherPlayersGroupHasBegun(player1, player2))
            {
                // LOG Error: Either player that wants to switch matches is residing in a group that has already started, it's too late to switch matches with other players
                return false;
            }

            if (EitherPlayerResidesInGroupThatDisallowSwitching(player1, player2))
            {
                // LOGG Error: Either player that wants to switch matches resides in a group that does not allow switching
                return false;
            }

            return true;
        }

        private static bool EitherPlayerIsInvalid(Player player1, Player player2)
        {
            return player1 == null || player2 == null;
        }

        private static bool EitherPlayerHasAnInvalidPlayerReference(Player player1, Player player2)
        {
            return player1.PlayerReference == null || player2.PlayerReference == null;
        }

        private static bool EitherPlayersGroupHasBegun(Player player1, Player player2)
        {
            bool player1MatchHasBegun = player1.Match.Group.GetPlayState() != PlayState.NotBegun;
            bool player2MatchHasBegun = player2.Match.Group.GetPlayState() != PlayState.NotBegun;

            return player1MatchHasBegun || player2MatchHasBegun;
        }

        private static bool EitherPlayerResidesInGroupThatDisallowSwitching(Player player1, Player player2)
        {
            bool player1GroupDisallowsSwitching = player1.Match.Group is RoundRobinGroup;
            bool player2GroupDisallowsSwitching = player2.Match.Group is RoundRobinGroup;

            return player1GroupDisallowsSwitching && player2GroupDisallowsSwitching;
        }

        private static void MakeSwitchOnPlayerReferencesInSameMatch(Match match)
        {
            PlayerReference firstPlayerReference = match.Player1.PlayerReference;
            PlayerReference secondPlayerReference = match.Player2.PlayerReference;

            match.SetPlayers(secondPlayerReference, firstPlayerReference);
        }

        private static void MakeSwitchOnPlayerReferencesInDifferentMatch(Player player1, Player player2)
        {
            PlayerReference firstPlayerReference = player1.PlayerReference;
            PlayerReference secondPlayerReference = player2.PlayerReference;

            Match player1Match = player1.Match;
            Match player2Match = player2.Match;

            if (player1Match.Player1.Id == player1.Id)
            {
                player1Match.SetPlayers(secondPlayerReference, player1Match.Player2.PlayerReference);
            }
            else
            {
                player1Match.SetPlayers(player1Match.Player1.PlayerReference, secondPlayerReference);
            }

            if (player2Match.Player1.Id == player2.Id)
            {
                player2Match.SetPlayers(firstPlayerReference, player2Match.Player2.PlayerReference);
            }
            else
            {
                player2Match.SetPlayers(player2Match.Player1.PlayerReference, firstPlayerReference);
            }
        }
    }
}
