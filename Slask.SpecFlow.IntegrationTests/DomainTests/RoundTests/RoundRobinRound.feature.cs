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
namespace Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class RoundRobinRoundFeature : Xunit.IClassFixture<RoundRobinRoundFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "RoundRobinRound.feature"
#line hidden
        
        public RoundRobinRoundFeature(RoundRobinRoundFeature.FixtureData fixtureData, InternalSpecFlow.XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RoundRobinRound", "\tDoes a bunch of tests on Round robin rounds", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Can create round robin round")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinRound")]
        [Xunit.TraitAttribute("Description", "Can create round robin round")]
        [Xunit.TraitAttribute("Category", "RoundRobinRoundTag")]
        public virtual void CanCreateRoundRobinRound()
        {
            string[] tagsOfScenario = new string[] {
                    "RoundRobinRoundTag"};
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can create round robin round", null, new string[] {
                        "RoundRobinRoundTag"});
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
                TechTalk.SpecFlow.Table table42 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table42.AddRow(new string[] {
                            "Round robin",
                            "Round robin round",
                            "3",
                            "1"});
#line 7
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table42, "When ");
#line hidden
                TechTalk.SpecFlow.Table table43 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table43.AddRow(new string[] {
                            "Round robin",
                            "Round robin round",
                            "3",
                            "1"});
#line 10
 testRunner.Then("created rounds in tournament should be valid with values:", ((string)(null)), table43, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create round robin round without name")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinRound")]
        [Xunit.TraitAttribute("Description", "Cannot create round robin round without name")]
        public virtual void CannotCreateRoundRobinRoundWithoutName()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create round robin round without name", null, ((string[])(null)));
#line 14
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
#line 15
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table44 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table44.AddRow(new string[] {
                            "Round robin",
                            "",
                            "3",
                            "1"});
#line 16
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table44, "When ");
#line hidden
#line 19
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create round robin round with zero advancers")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinRound")]
        [Xunit.TraitAttribute("Description", "Cannot create round robin round with zero advancers")]
        public virtual void CannotCreateRoundRobinRoundWithZeroAdvancers()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create round robin round with zero advancers", null, ((string[])(null)));
#line 21
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
#line 22
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table45 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table45.AddRow(new string[] {
                            "Round robin",
                            "Round robin round",
                            "3",
                            "0"});
#line 23
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table45, "When ");
#line hidden
#line 26
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create round robin round with less than zero advancers")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinRound")]
        [Xunit.TraitAttribute("Description", "Cannot create round robin round with less than zero advancers")]
        public virtual void CannotCreateRoundRobinRoundWithLessThanZeroAdvancers()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create round robin round with less than zero advancers", null, ((string[])(null)));
#line 28
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
#line 29
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table46 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table46.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 1",
                            "3",
                            "-1"});
                table46.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 2",
                            "3",
                            "-2"});
                table46.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 3",
                            "3",
                            "-3"});
#line 30
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table46, "When ");
#line hidden
#line 35
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 36
  testRunner.And("created round 1 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
  testRunner.And("created round 2 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create round robin round with even best ofs")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinRound")]
        [Xunit.TraitAttribute("Description", "Cannot create round robin round with even best ofs")]
        public virtual void CannotCreateRoundRobinRoundWithEvenBestOfs()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create round robin round with even best ofs", null, ((string[])(null)));
#line 39
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
#line 40
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table47 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table47.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 1",
                            "0",
                            "1"});
                table47.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 2",
                            "2",
                            "1"});
                table47.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 3",
                            "4",
                            "1"});
#line 41
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table47, "When ");
#line hidden
#line 46
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 47
  testRunner.And("created round 1 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 48
  testRunner.And("created round 2 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create round robin round with best ofs less than zero")]
        [Xunit.TraitAttribute("FeatureTitle", "RoundRobinRound")]
        [Xunit.TraitAttribute("Description", "Cannot create round robin round with best ofs less than zero")]
        public virtual void CannotCreateRoundRobinRoundWithBestOfsLessThanZero()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create round robin round with best ofs less than zero", null, ((string[])(null)));
#line 50
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
#line 51
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table48 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table48.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 1",
                            "-1",
                            "1"});
                table48.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 2",
                            "-2",
                            "1"});
                table48.AddRow(new string[] {
                            "Round robin",
                            "Round robin round 3",
                            "-3",
                            "1"});
#line 52
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table48, "When ");
#line hidden
#line 57
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 58
  testRunner.And("created round 1 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 59
  testRunner.And("created round 2 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
                RoundRobinRoundFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                RoundRobinRoundFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
