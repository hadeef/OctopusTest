using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain;
using Newtonsoft.Json;

namespace Repository
{
    //This class implements Singleton pattern.
    public sealed class ReleaseEnvironmentRepository : GenericRepository<ReleaseEnvironment, string, string>
    {
        private static readonly object lockObj = new object();
        private static ReleaseEnvironmentRepository _instance;

        private ReleaseEnvironmentRepository(string dataFilePath)
        {
            using (StreamReader r = new StreamReader(dataFilePath))
            {
                string json = r.ReadToEnd();
                _entities = JsonConvert.DeserializeObject<IList<ReleaseEnvironment>>(json);
            }
        }

        private ReleaseEnvironmentRepository(IList<ReleaseEnvironment> environments)
        {
            _entities = environments;
        }

        public static ReleaseEnvironmentRepository GetInstanceByJsonFile(string dataFilePath)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ReleaseEnvironmentRepository(dataFilePath);
                    }
                }
            }

            return _instance;
        }

        public static ReleaseEnvironmentRepository GetInstanceByObjectList(IList<ReleaseEnvironment> environments)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ReleaseEnvironmentRepository(environments);
                    }
                }
            }

            return _instance;
        }

        public override ReleaseEnvironment GetById(string id)
        {
            return _entities.FirstOrDefault(pr => pr.Id == id);
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
