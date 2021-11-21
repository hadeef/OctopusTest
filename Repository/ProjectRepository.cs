using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain;
using Newtonsoft.Json;

namespace Repository
{
    //This class implements Singleton pattern.
    public sealed class ProjectRepository : GenericRepository<Project, string, string>
    {
        private static readonly object lockObj = new object();

        private static ProjectRepository _instance;

        private ProjectRepository(string dataFilePath)
        {            
            using (StreamReader r = new StreamReader(dataFilePath))
            {
                string json = r.ReadToEnd();
                _entities = JsonConvert.DeserializeObject<IList<Project>>(json);
            }
        }

        private ProjectRepository(IList<Project> projects)
        {
            _entities = projects;
        }

        public static ProjectRepository GetInstanceByJsonFile(string dataFilePath)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ProjectRepository(dataFilePath);
                    }
                }
            }

            return _instance;
        }

        public static ProjectRepository GetInstanceByObjectList(IList<Project> projects)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ProjectRepository(projects);
                    }
                }
            }

            return _instance;
        }

        public override Project GetById(string id)
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
