﻿using System;

namespace Slask.Domain
{
    public class Settings
    {
        private Settings()
        {
        }

        public Guid Id { get; private set; }
        public string Type { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static Settings Create(string type)
        {
            return new Settings
            {
                Id = Guid.NewGuid(),
                Type = type
            };
        }
    }
}
