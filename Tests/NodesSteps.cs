﻿using System.Threading.Tasks;
using Client.Domain;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public sealed class NodesSteps
    {
        private readonly ClientServerPair _host = new ClientServerPair();
        private Task<Graph> _result;
 
        [Given(@"I call CreateNode\((.*), (.*)\)")]
        public void CreateNode(double latitude, double longitude)
        {
            var node = _host.Client.CreateNode(new Coordinates(latitude, longitude));
            node.Wait();
        }
        [Given(@"I call UpdateNode")]
        public void GivenICallUpdateNode(string multilineText)
        {
            _host.Client.UpdateNode(
                JsonConvert.DeserializeObject<Node>(multilineText))
                .Wait();
        }

        [When(@"I call GetGraph\(\)")]
        public void GetGraph()
        {
            _result = _host.Client.GetGraph();
        }

        [Then(@"the return value should be")]
        public void CheckResult(string expected)
        {
            var a = JsonConvert.SerializeObject(_result.Result, Formatting.Indented);
            var e = JToken.Parse(expected).ToString(Formatting.Indented);
            a.Should().Be(e);
        }
    }
}
