using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class StringConstants
    {
        public static class DataFiles
        {
            public const string dataFilesFolderName = "Data";
            public const string deploymentFileName = "Deployments.json";
            public const string releaseEnvironmentFileName = "Environments.json";
            public const string projectFileName = "Projects.json";
            public const string releaseFileName = "Releases.json";
        }

        public static class LogFile
        {
            public const string LogFileName = "ReleaseRetentionLog.txt";
        }

        public static class AppStrings
        {
            public static readonly string startupPath = Directory.GetCurrentDirectory();
        }

        public static string DataFilePath(string fileName)
        {
            return Path.Combine(AppStrings.startupPath, Path.Combine(DataFiles.dataFilesFolderName, fileName));
        }

        public static class ReleaseItme
        {
            public const string MainRelease = " release added to retention list as current deployment";
            public const string PreviousRelease = " release added to retention list as previous release of ";
            public const string NextRelease = " release added to retention list as next release of ";
            public const string NoPreviousReleases = " release doesn't have any previous release.";
            public const string NoNextReleases = " release doesn't have any next release.";
            public const string LessPreviousReleases = " release doesn't has required previous releases.";
            public const string ReleaseNotFound = " release not found in releases.";
            public const string NotFoundAnyDeployment = "No deployment found in deployments.";
        }

        public static string EnvironmentAndProjectString(string projectId, string environmentId)
        {
            return $" for project : {projectId} on environment: {environmentId}";
        }

        public static string LessPreviousReleases(int requiredNo, int availableNo)
        {
            return $" release only have {availableNo} out of {requiredNo} previous releases.";
        }

        public static StringBuilder LogFileItme(string message)
        {
            StringBuilder str = new StringBuilder(DateTime.Now + message.PadLeft(message.Length + 5) + Environment.NewLine);
            return str;
        }
    }
}
