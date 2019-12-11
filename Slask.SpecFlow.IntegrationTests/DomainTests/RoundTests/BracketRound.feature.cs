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
    public partial class BracketRoundFeature : Xunit.IClassFixture<BracketRoundFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "BracketRound.feature"
#line hidden
        
        public BracketRoundFeature(BracketRoundFeature.FixtureData fixtureData, InternalSpecFlow.XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BracketRound", "\tDoes a bunch of tests on Bracket rounds", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Can create bracket round")]
        [Xunit.TraitAttribute("FeatureTitle", "BracketRound")]
        [Xunit.TraitAttribute("Description", "Can create bracket round")]
        [Xunit.TraitAttribute("Category", "BracketRoundTag")]
        public virtual void CanCreateBracketRound()
        {
            string[] tagsOfScenario = new string[] {
                    "BracketRoundTag"};
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can create bracket round", null, new string[] {
                        "BracketRoundTag"});
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
                TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of"});
                table24.AddRow(new string[] {
                            "Bracket",
                            "Bracket round",
                            "3"});
#line 7
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table24, "When ");
#line hidden
                TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table25.AddRow(new string[] {
                            "Bracket",
                            "Bracket round",
                            "3",
                            "1"});
#line 10
 testRunner.Then("created rounds in tournament should be valid with values:", ((string)(null)), table25, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Advancing amount in bracket rounds cannot be anything other than two")]
        [Xunit.TraitAttribute("FeatureTitle", "BracketRound")]
        [Xunit.TraitAttribute("Description", "Advancing amount in bracket rounds cannot be anything other than two")]
        public virtual void AdvancingAmountInBracketRoundsCannotBeAnythingOtherThanTwo()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Advancing amount in bracket rounds cannot be anything other than two", null, ((string[])(null)));
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
                TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table26.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 1",
                            "3",
                            "0"});
                table26.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 2",
                            "3",
                            "2"});
                table26.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 3",
                            "3",
                            "3"});
#line 16
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table26, "When ");
#line hidden
                TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});
                table27.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 1",
                            "3",
                            "1"});
                table27.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 2",
                            "3",
                            "1"});
                table27.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 3",
                            "3",
                            "1"});
#line 21
 testRunner.Then("created rounds in tournament should be valid with values:", ((string)(null)), table27, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create bracket round without name")]
        [Xunit.TraitAttribute("FeatureTitle", "BracketRound")]
        [Xunit.TraitAttribute("Description", "Cannot create bracket round without name")]
        public virtual void CannotCreateBracketRoundWithoutName()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create bracket round without name", null, ((string[])(null)));
#line 27
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
#line 28
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of"});
                table28.AddRow(new string[] {
                            "Bracket",
                            "",
                            "3"});
#line 29
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table28, "When ");
#line hidden
#line 32
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create bracket round with even best ofs")]
        [Xunit.TraitAttribute("FeatureTitle", "BracketRound")]
        [Xunit.TraitAttribute("Description", "Cannot create bracket round with even best ofs")]
        public virtual void CannotCreateBracketRoundWithEvenBestOfs()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create bracket round with even best ofs", null, ((string[])(null)));
#line 34
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
#line 35
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of"});
                table29.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 1",
                            "0"});
                table29.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 2",
                            "2"});
                table29.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 3",
                            "4"});
#line 36
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table29, "When ");
#line hidden
#line 41
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 42
  testRunner.And("created round 1 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 43
  testRunner.And("created round 2 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Cannot create bracket round with best ofs less than zero")]
        [Xunit.TraitAttribute("FeatureTitle", "BracketRound")]
        [Xunit.TraitAttribute("Description", "Cannot create bracket round with best ofs less than zero")]
        public virtual void CannotCreateBracketRoundWithBestOfsLessThanZero()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot create bracket round with best ofs less than zero", null, ((string[])(null)));
#line 45
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
#line 46
 testRunner.Given("a tournament named \"GSL 2019\" has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of"});
                table30.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 1",
                            "-1"});
                table30.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 2",
                            "-2"});
                table30.AddRow(new string[] {
                            "Bracket",
                            "Bracket round 3",
                            "-3"});
#line 47
 testRunner.When("created tournament 0 adds rounds", ((string)(null)), table30, "When ");
#line hidden
#line 52
 testRunner.Then("created round 0 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 53
  testRunner.And("created round 1 in tournament should be invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 54
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
                BracketRoundFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                BracketRoundFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
