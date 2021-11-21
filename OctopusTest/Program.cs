using BusinessServices;
using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.StringConstants;

namespace OctopusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectDataFilePath = DataFilePath(DataFiles.projectFileName);
            ProjectRepository projects = ProjectRepository.GetInstanceByJsonFile(projectDataFilePath);

            string environmentDataFilePath = DataFilePath(DataFiles.releaseEnvironmentFileName);
            ReleaseEnvironmentRepository environments = ReleaseEnvironmentRepository.GetInstanceByJsonFile(environmentDataFilePath);

            string releasesDataFilePath = DataFilePath(DataFiles.releaseFileName);
            ReleaseRepository releases = ReleaseRepository.GetInstanceByJsonFile(releasesDataFilePath);

            string deploymentDataFilePath = DataFilePath(DataFiles.deploymentFileName);
            DeploymentRepository deployments = DeploymentRepository.GetInstanceByJsonFile(deploymentDataFilePath);

            var s3 = new ReleaseRetentionService(projects, environments, releases, deployments);

            var s4 = s3.GetRetentionReleasesListByNPreviousVersions(0);

            var ss3 = 0;
        }
    }
}
