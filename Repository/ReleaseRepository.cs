using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain;
using Newtonsoft.Json;

namespace Repository
{
    //This class implements Singleton pattern.
    public sealed class ReleaseRepository : GenericRepository<Release, string, string>
    {
        private static readonly object lockObj = new object();
        private static ReleaseRepository _instance;

        private ReleaseRepository(string dataFilePath)
        {
            using (StreamReader r = new StreamReader(dataFilePath))
            {
                string json = r.ReadToEnd();
                _entities = JsonConvert.DeserializeObject<IList<Release>>(json);
            }
        }

        private ReleaseRepository(IList<Release> releases)
        {
            _entities = releases;
        }

        public static ReleaseRepository GetInstanceByJsonFile(string dataFilePath)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ReleaseRepository(dataFilePath);
                    }
                }
            }

            return _instance;
        }

        public static ReleaseRepository GetInstanceByObjectList(IList<Release> releases)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ReleaseRepository(releases);
                    }
                }
            }

            return _instance;
        }

        public override Release GetById(string releaseId)
        {
            return _entities.FirstOrDefault(pr => pr.Id == releaseId);
        }

        public IList<Release> GetByProjectId(string projectId)
        {
            IList<Release> releases = _entities.Where(re => re.ProjectId == projectId).ToList();
            return releases;
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

        /// <summary>This method returns a release and it's project n previous releases. If releases number is less than n+1, it returns all.</summary>
        /// <param name="releaseId"></param>
        /// <param name="n"></param>
        /// <returns>
        /// List of a project releases.</returns>
        public IList<Release> GetReleaseAndNPreviousVersions(string releaseId, int n)
        {
            IList<Release> result = new List<Release>();
            Release release = GetById(releaseId);

            if (release != null)
            {
                result = _entities.Where(re => (re.ProjectId == release.ProjectId) && (re.Created <= release.Created)).OrderByDescending(re => re.Created).ToList();

                if (result.Count >= (n + 1))
                {
                    return result.Take(n + 1).ToList();
                }
            }

            return result;
        }

        /// <summary>This method returns a release's project all next releases.</summary>
        /// <param name="releaseId"></param>
        /// <returns>List of a project releases.</returns>
        public IList<Release> GetReleaseAllNextVersions(string releaseId)
        {
            IList<Release> result = new List<Release>();
            Release release = GetById(releaseId);

            if (release != null)
            {
                result = _entities.Where(re => (re.ProjectId == release.ProjectId) && (re.Created > release.Created)).OrderBy(re => re.Created).ToList();
            }
            return result;
        }

    }
}
