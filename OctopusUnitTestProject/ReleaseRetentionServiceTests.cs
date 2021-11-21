using BusinessServices;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using System;
using System.Collections.Generic;

namespace OctopusTest
{
    [TestClass]
    public class ReleaseRetentionServiceTests
    {
        [TestMethod]
        public void GetLatestDeploymentOfProjectInEnvironment_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            ReleaseRetentionService service = ReleaseRetentionServiceProvider.GetReleaseRetentionServiceInstance();
            string projectId = "TestProject-1";
            string environmentId = "Environment-1";

            // Act
            var result = service.GetLatestDeploymentOfProjectInEnvironment(projectId, environmentId);

            // Assert
            Assert.AreEqual(result.Id, "TestDeployment-2");
        }

        [TestMethod]
        public void GetLatestDeploymentsByProjectAndEnvironment_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            ReleaseRetentionService service = ReleaseRetentionServiceProvider.GetReleaseRetentionServiceInstance();

            // Act
            var result = service.GetLatestDeploymentsByProjectAndEnvironment();

            // Assert
            Assert.AreEqual(result[0].Id, "TestDeployment-2");
            Assert.AreEqual(result[0].ReleaseId, "TestRelease-2");

            Assert.AreEqual(result[1].Id, "TestDeployment-3");
            Assert.AreEqual(result[1].ReleaseId, "TestRelease-3");

            Assert.AreEqual(result[2].Id, "TestDeployment-4");
            Assert.AreEqual(result[2].ReleaseId, "TestRelease-4");

            Assert.AreEqual(result[3].Id, "TestDeployment-6");
            Assert.AreEqual(result[3].ReleaseId, "TestRelease-6");
        }

        [TestMethod]
        public void GetRetentionReleasesListByNPreviousVersions_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            ReleaseRetentionService service = ReleaseRetentionServiceProvider.GetReleaseRetentionServiceInstance();
            int n = 1;

            // Act
            var result = service.GetRetentionReleasesListByNPreviousVersions(n);

            // Assert
            Assert.AreEqual(result[0].Id, "TestRelease-1");
            Assert.AreEqual(result[1].Id, "TestRelease-2");
            Assert.AreEqual(result[2].Id, "TestRelease-3");

            Assert.AreEqual(result[3].Id, "TestRelease-4");
            Assert.AreEqual(result[4].Id, "TestRelease-5");
            Assert.AreEqual(result[5].Id, "TestRelease-6");
        }

        private  static class ReleaseRetentionServiceProvider
        {
            public static ReleaseRetentionService GetReleaseRetentionServiceInstance()
            {
                List<Project> projects = new List<Project>();
                List<ReleaseEnvironment> environments = new List<ReleaseEnvironment>();
                List<Release> releases = new List<Release>();
                List<Deployment> deployments = new List<Deployment>();

                projects.Add(new Project("TestProject-1", "TestProject-1"));
                projects.Add(new Project("TestProject-2", "TestProject-2"));
                ProjectRepository proRepository = ProjectRepository.GetInstanceByObjectList(projects);

                environments.Add(new ReleaseEnvironment("Environment-1", "TestStaging-1"));
                environments.Add(new ReleaseEnvironment("Environment-2", "TestStaging-2"));
                ReleaseEnvironmentRepository envRepository = ReleaseEnvironmentRepository.GetInstanceByObjectList(environments);

                releases.Add(new Release("TestRelease-1", "TestProject-1", "1.0.1", DateTime.Now.AddDays(-10)));
                releases.Add(new Release("TestRelease-2", "TestProject-1", "1.0.2", DateTime.Now.AddDays(-9)));
                releases.Add(new Release("TestRelease-3", "TestProject-1", "1.0.3", DateTime.Now.AddDays(-8)));
                releases.Add(new Release("TestRelease-4", "TestProject-2", "1.0.1", DateTime.Now.AddDays(-7)));
                releases.Add(new Release("TestRelease-5", "TestProject-2", "1.0.2", DateTime.Now.AddDays(-6)));
                releases.Add(new Release("TestRelease-6", "TestProject-2", "1.0.7", DateTime.Now.AddDays(-5)));
                ReleaseRepository relRepository = ReleaseRepository.GetInstanceByObjectList(releases);

                deployments.Add(new Deployment("TestDeployment-1", "TestRelease-1", "Environment-1", DateTime.Now.AddDays(-5)));
                deployments.Add(new Deployment("TestDeployment-2", "TestRelease-2", "Environment-1", DateTime.Now.AddDays(-4)));
                deployments.Add(new Deployment("TestDeployment-3", "TestRelease-3", "Environment-2", DateTime.Now.AddDays(-4)));
                deployments.Add(new Deployment("TestDeployment-4", "TestRelease-4", "Environment-1", DateTime.Now.AddDays(-3)));
                deployments.Add(new Deployment("TestDeployment-5", "TestRelease-5", "Environment-2", DateTime.Now.AddDays(-3)));
                deployments.Add(new Deployment("TestDeployment-6", "TestRelease-6", "Environment-2", DateTime.Now.AddDays(-2)));
                DeploymentRepository depRepository = DeploymentRepository.GetInstanceByObjectList(deployments);

                ReleaseRetentionService releaseRetentionService = new ReleaseRetentionService(proRepository, envRepository, relRepository, depRepository);
                return releaseRetentionService;
            }
        }
    }
}

