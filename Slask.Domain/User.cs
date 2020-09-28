using System;

namespace Slask.Domain
{
    public class User
    {
        private User()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public static User Create(string name)
        {
            if (name == null || name == "")
            {
                return null;
            }

            return new User
            {
                Name = name
            };
        }

        public void RenameTo(string name)
        {
            Name = name.Trim();
        }
    }
}
