using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void ReportingStructureReturnsCorrectValue()
        {
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            int expectedResult = 4;

            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            HttpResponseMessage response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedResult, reportingStructure.NumberOfReports);

            employeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            expectedResult = 2;

            getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            response = getRequestTask.Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedResult, reportingStructure.NumberOfReports);

            employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            expectedResult = 0;

            getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            response = getRequestTask.Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedResult, reportingStructure.NumberOfReports);
        }

        [TestMethod]
        public void ReportingStructureReturnsNotFound()
        {
            var employeeId = "Invalid Id";

            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            HttpResponseMessage response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
