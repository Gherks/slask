using System;

namespace Slask.Domain
{
    public class User
    {
        private User()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public static User Create(string name)
        {
            if(name == null || name == "")
            {
                return null;
            }

            return new User
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public void RenameTo(string name)
        {
            Name = name.Trim();
        }
    }
}
