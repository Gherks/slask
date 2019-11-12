using Slask.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.TestCore
{
    public abstract class TestContextBase
    {
        protected SlaskContext SlaskContext { get; }

        protected TestContextBase(SlaskContext slaskContext)
        {
            SlaskContext = slaskContext;
        }

        public void SaveChanges()
        {
            SlaskContext.SaveChanges();
        }
    }
}
