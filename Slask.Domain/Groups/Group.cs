using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class Group
    {
        private Group()
        {
            ParticipatingPlayers = new List<PlayerReference>();
            Matches = new List<Match>();
        }

        public Guid Id { get; private set; }
        public List<PlayerReference> ParticipatingPlayers { get; private set; }
        public List<Match> Matches { get; private set; }
        public Guid RoundId { get; private set; }
        public Round Round { get; private set; }

        public static Group Create(Round parentRound)
        {
            if(parentRound == null)
            {
                return null;
            }

            return new Group
            {
                Id = Guid.NewGuid(),
                Round = parentRound
            };
        }

        public void AddPlayerReference(string name)
        {
            PlayerReference playerReference = ParticipatingPlayers.Where(reference => reference.Name == name).FirstOrDefault();

            if (playerReference == null)
            {
                playerReference = RegisterPlayerToTournamentWithName(name);
                ParticipatingPlayers.Add(playerReference);
            }
        }

        public Match AddMatch(DateTime startDateTime)
        {
            bool anyNameIsEmpty = player1Name == "" || player2Name == "";
            bool namesAreIdentical = player1Name.ToLower() == player2Name.ToLower();
            bool dateTimeIsInThePast = startDateTime < DateTimeHelper.Now;

            if (anyNameIsEmpty || namesAreIdentical || dateTimeIsInThePast)
            {
                // LOG ISSUE HERE
                return null;
            }

            PlayerReference playerReference1 = GetPlayerReferenceWithName(player1Name);
            PlayerReference playerReference2 = GetPlayerReferenceWithName(player2Name);

            Match match = Match.Create(playerReference1, playerReference2, startDateTime);

            if (match != null)
            {
                Matches.Add(match);
            }

            return match;
        }

        public Match AddMatch(PlayerReference playerReference1, PlayerReference playerReference2, DateTime startDateTime)
        {
            return AddMatch(playerReference1.Name, playerReference2.Name, startDateTime);
        }

        private PlayerReference GetPlayerReferenceWithName(string name)
        {
            PlayerReference playerReference = ParticipatingPlayers.Where(reference => reference.Name == name).FirstOrDefault();

            if(playerReference == null)
            {
                playerReference = RegisterPlayerToTournamentWithName(name);
                ParticipatingPlayers.Add(playerReference);
            }

            return playerReference;
        }

        private PlayerReference RegisterPlayerToTournamentWithName(string name)
        {
            Tournament tournament = Round.Tournament;
            PlayerReference playerReference = tournament.GetPlayerReferenceByPlayerName(name);

            if(playerReference == null)
            {
                playerReference = PlayerReference.Create(name, tournament);
                tournament.PlayerReferences.Add(playerReference);
            }

            return playerReference;
        }
    }
}
