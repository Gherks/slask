using System;
using System.ComponentModel.DataAnnotations;

namespace Slask.Domain
{
    public class User
    {
        private User()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}
