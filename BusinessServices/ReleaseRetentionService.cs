using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.StringConstants;

namespace BusinessServices
{
    //Assumptions for ReleaseRetentionService class: 
    // 1- Retention list includes all future releases of current deployed project which were released but not deployed yet.
    // 2- This implementation assumes there is no incompatibility on any release date and it's deployment date.For example there
    //    will be no release which it's deployment date is before it's release date.

    public class ReleaseRetentionService
    {
        private ProjectRepository _projects;
        private ReleaseEnvironmentRepository _environments;
        private ReleaseRepository _releases;
        private DeploymentRepository _deployments;

        private StringBuilder _sb = new StringBuilder();
        private IList<Release> proReleases = new List<Release>();

        public ReleaseRetentionService(ProjectRepository projects, ReleaseEnvironmentRepository environments, ReleaseRepository releases, DeploymentRepository deployments)
        {
            _projects = projects;
            _environments = environments;
            _releases = releases;
            _deployments = deployments;
        }

        /// <summary>This method returns project latest deployment on an environment.</summary>
        /// <returns>A deployments based on project/environment.</returns>
        public Deployment GetLatestDeploymentOfProjectInEnvironment(string projectId, string environmentId)
        {
            Deployment proLatestDeployment = null;

            //Gets project all releases. It includes a small trick for better performance!.
            if (proReleases.Count == 0 || proReleases[0].ProjectId != projectId)
                proReleases = _releases.GetByProjectId(projectId);

            IList<string> proReleaseIds = proReleases.Select(re => re.Id).ToList();

            if (proReleaseIds.Count > 0)
            {
                //Gets project latest deployment on environment.
                proLatestDeployment = _deployments.GetProjectLatestDeploymentInEnvironmentByReleaseIds(proReleaseIds, environmentId);
            }

            return proLatestDeployment;
        }

        /// <summary>This method returns latest deployments of all projects on all environments.</summary>
        /// <returns>List of deployments based on project/environment.</returns>
        public IList<Deployment> GetLatestDeploymentsByProjectAndEnvironment()
        {
            IList<Deployment> result = new List<Deployment>();

            IList<Project> projectsAll = _projects.GetAll();
            IList<ReleaseEnvironment> environmentsAll = _environments.GetAll();

            if (projectsAll.Count > 0 && environmentsAll.Count > 0)
            {
                for (int i = 0; i < projectsAll.Count; i++)
                {
                    for (int j = 0; j < environmentsAll.Count; j++)
                    {
                        Deployment proDeployment = GetLatestDeploymentOfProjectInEnvironment(projectsAll[i].Id, environmentsAll[j].Id);

                        if (proDeployment != null)
                        {
                            result.Add(proDeployment);
                        }
                    }
                }
            }
            return result;
        }

        //Note: For each project, this method returns n previous releases of project and also adds all next releases of project which haven't yet deployed on any environment.
        public IList<Release> GetRetentionReleasesListByNPreviousVersions(int n)
        {
            //logging variable
            string logText = string.Empty;
            string mainRelId = string.Empty;

            List<Release> result = new List<Release>();
            IList<Deployment> latestDeployments = GetLatestDeploymentsByProjectAndEnvironment();

            if (latestDeployments.Count > 0)
            {
                foreach (Deployment de in latestDeployments)
                {
                    IList<Release> relps = _releases.GetReleaseAndNPreviousVersions(de.ReleaseId, n);

                    if (relps.Count == 0)
                    {
                        logText = de.ReleaseId + ReleaseItme.ReleaseNotFound;
                        _sb = _sb.Append(LogFileItme(logText));
                    }
                    else
                    {
                        mainRelId = relps[0].Id;

                        if (relps.Count < n + 1)
                        {
                            logText = mainRelId + LessPreviousReleases(n, relps.Count - 1);
                            _sb = _sb.Append(LogFileItme(logText));
                        }

                        for (int k = 0; k < relps.Count; k++)
                        {
                            if (k == 0)
                            {
                                logText = relps[k].Id + ReleaseItme.MainRelease + EnvironmentAndProjectString(relps[k].ProjectId, de.EnvironmentId);
                            }
                            else
                            {
                                logText = relps[k].Id + ReleaseItme.PreviousRelease + mainRelId + EnvironmentAndProjectString(relps[k].ProjectId, de.EnvironmentId);
                            }
                            _sb = _sb.Append(LogFileItme(logText));
                        }

                        result.AddRange(relps);

                    }

                    //Also returns all next releases of a project which haven't yet deployed on any environment.
                    IList<Release> relns = _releases.GetReleaseAllNextVersions(de.ReleaseId);

                    if (relns.Count > 0)
                    {
                        for (int k = 0; k < relns.Count; k++)
                        {
                            logText = relns[k].Id + ReleaseItme.NextRelease + mainRelId + EnvironmentAndProjectString(relns[k].ProjectId, de.EnvironmentId);
                            _sb = _sb.Append(LogFileItme(logText));
                        }

                        result.AddRange(relns);
                    }
                    else
                    {
                        logText = mainRelId + ReleaseItme.NoNextReleases;
                        _sb = _sb.Append(LogFileItme(logText));
                    }
                }

                //Removing repetitive releases in list.
                result = result.Distinct().OrderBy(re => re.Created).ToList();

            }
            else
            {
                logText = ReleaseItme.NotFoundAnyDeployment;
                _sb = _sb.Append(LogFileItme(logText));
            }

            File.AppendAllText(AppStrings.startupPath + Path.DirectorySeparatorChar + LogFile.LogFileName, _sb.ToString());
            _sb.Clear();

            return result;
        }
    }
}
