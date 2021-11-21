using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain;
using Newtonsoft.Json;

namespace Repository
{
    //This class implements Singleton pattern.
    public sealed class DeploymentRepository : GenericRepository<Deployment, string, string>
    {
        private static readonly object lockObj = new object();
        private static  DeploymentRepository _instance;

        private DeploymentRepository(string dataFilePath)
        {
            using (StreamReader r = new StreamReader(dataFilePath))
            {
                string json = r.ReadToEnd();
                _entities = JsonConvert.DeserializeObject<IList<Deployment>>(json);
            }
        }

        private DeploymentRepository(IList<Deployment> deployments)
        {
            _entities = deployments;
        }

        public static DeploymentRepository GetInstanceByJsonFile(string dataFilePath)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new DeploymentRepository(dataFilePath);
                    }
                }
            }

            return _instance;
        }

        public static DeploymentRepository GetInstanceByObjectList(IList<Deployment> deployments)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new DeploymentRepository(deployments);
                    }
                }
            }

            return _instance;
        }

        public override Deployment GetById(string id)
        {
            return _entities.FirstOrDefault(pr => pr.Id == id);
        }

        public IList<Deployment> GetByReleaseId(string releaseId)
        {
            return _entities.Where(pr => pr.ReleaseId == releaseId).ToList();
        }

        //public IList<Deployment> GetAllProjectDeploymentsByReleaseIds(IList<string> releaseIds)
        //{
        //    return _entities.Where(pr => releaseIds.Contains(pr.ReleaseId)).ToList();
        //}

        //public IList<Deployment> GetAllProjectDeploymentsInEnvironmentByReleaseIds(IList<string> releaseIds, string environmentId)
        //{
        //    return _entities.Where(pr => releaseIds.Contains(pr.ReleaseId) && pr.EnvironmentId == environmentId).ToList();
        //}

        public Deployment GetProjectLatestDeploymentInEnvironmentByReleaseIds(IList<string> releaseIds, string environmentId)
        {
            return _entities.Where(pr => releaseIds.Contains(pr.ReleaseId) && pr.EnvironmentId == environmentId)
                .OrderByDescending(pr=>pr.DeployedAt).FirstOrDefault();
        }

        public override bool Remove(string id)
        {
            bool isEntityEncluded = _entities.Where(pr => pr.Id == id).Any();

            if (isEntityEncluded)
            {
                _entities = _entities.Where(pr => pr.Id != id).ToList();
                return true;
            }

            return false;
        }
    }
}
