// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.1.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class RoundRobinGroupFeature : Xunit.IClassFixture<RoundRobinGroupFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "RoundRobinGroup.feature"
#line hidden
        
        public RoundRobinGroupFeature(RoundRobinGroupFeature.FixtureData fixtureData, InternalSpecFlow.XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RoundRobinGroup", "\tDoes a bunch of tests on Round robin group", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Adding group to round robin round creates bracket group")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinGroup")]
        [Xunit.TraitAttribute("Description", "Adding group to round robin round creates bracket group")]
        [Xunit.TraitAttribute("Category", "RoundRobinGroupTag")]
        public virtual void AddingGroupToRoundRobinRoundCreatesBracketGroup()
        {
            string[] tagsOfScenario = new string[] {
                    "RoundRobinGroupTag"};
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Adding group to round robin round creates bracket group", null, new string[] {
                        "RoundRobinGroupTag"});
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table11.AddRow(new string[] {
                            "Round robin",
                            "Round robin round",
                            "3",
                            "1"});
#line 7
  testRunner.And("created tournament 0 adds rounds", ((string)(null)), table11, "And ");
#line hidden
#line 10
 testRunner.When("group is added to created round 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 11
 testRunner.Then("group 0 should be valid of type \"Round robin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Start time in matches in round robin groups is spaced with one hour upon creation" +
            "")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinGroup")]
        [Xunit.TraitAttribute("Description", "Start time in matches in round robin groups is spaced with one hour upon creation" +
            "")]
        public virtual void StartTimeInMatchesInRoundRobinGroupsIsSpacedWithOneHourUponCreation()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Start time in matches in round robin groups is spaced with one hour upon creation" +
                    "", null, ((string[])(null)));
#line 13
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 14
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table12.AddRow(new string[] {
                            "Bracket",
                            "Bracket round",
                            "3",
                            "1"});
#line 15
  testRunner.And("created tournament 0 adds rounds", ((string)(null)), table12, "And ");
#line hidden
#line 18
  testRunner.And("group is added to created round 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 19
 testRunner.When("players \"Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain\" is added " +
                        "to created group 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 20
 testRunner.Then("minutes between matches in created group 0 should be 60", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Round robin tournament progression goes as expected")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinGroup")]
        [Xunit.TraitAttribute("Description", "Round robin tournament progression goes as expected")]
        public virtual void RoundRobinTournamentProgressionGoesAsExpected()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Round robin tournament progression goes as expected", null, ((string[])(null)));
#line 22
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 25
 testRunner.Given("a tournament named \"GSL 2019\" with users \"Stålberto, Bönis, Guggelito\" added to i" +
                        "t", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table13.AddRow(new string[] {
                            "Round robin tournament",
                            "Round robin round",
                            "3",
                            "3"});
#line 26
  testRunner.And("created tournament 0 adds rounds", ((string)(null)), table13, "And ");
#line hidden
#line 29
  testRunner.And("group is added to created round 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 30
  testRunner.And("players \"Maru, Stork, Taeja, Rain, Bomber\" is added to created group 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                            "Created tournament index",
                            "Round index",
                            "Group index"});
                table14.AddRow(new string[] {
                            "0",
                            "0",
                            "0"});
#line 31
  testRunner.And("groups within created tournament is played out and betted on", ((string)(null)), table14, "And ");
#line hidden
#line 34
 testRunner.Then("advancing players in created group 0 is exactly \"Bomber, Taeja, Stork\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                RoundRobinGroupFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                RoundRobinGroupFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
