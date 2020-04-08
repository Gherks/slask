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
            if (player1 == null || player2 == null)
            {
                // LOG Error: One of given players was null when trying to switch places
                return false;
            }

            bool bothPlayersHasPlayerReferences = player1.PlayerReference != null && player2.PlayerReference != null;
            bool noMatchHasStartedInGroup = player1.Match.Group.GetPlayState() == PlayState.NotBegun;

            if (bothPlayersHasPlayerReferences && noMatchHasStartedInGroup)
            {
                bool bothPlayersResidesInSameMatch = player1.Match.Id == player2.Match.Id;

                if (bothPlayersResidesInSameMatch)
                {
                    MakeSwitchOnPlayerReferencesInSameMatch(player1.Match);
                    return true;
                }

                bool bothPlayersResidesInSameGroup = player1.Match.Group.Id == player2.Match.Group.Id;

                if (bothPlayersResidesInSameGroup)
                {
                    MakeSwitchOnPlayerReferencesInDifferentMatch(player1, player2);
                    return true;
                }
            }

            return false;
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
