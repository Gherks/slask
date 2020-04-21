﻿using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using System;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "RoundRobinGroup")]
    public class RoundRobinGroupSteps : RoundRobinGroupStepDefinitions
    {

    }

    public class RoundRobinGroupStepDefinitions : GroupStepDefinitions
    {

    }
}