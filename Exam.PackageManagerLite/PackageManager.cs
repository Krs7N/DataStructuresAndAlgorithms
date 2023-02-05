using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.PackageManagerLite
{
    public class PackageManager : IPackageManager
    {
        private HashSet<Package> packages = new HashSet<Package>();
        private Dictionary<string, Package> packageById = new Dictionary<string, Package>();

        public void AddDependency(string packageId, string dependencyId)
        {
            var mainPackage = packageById[packageId];
            var dependencyPackage = packageById[dependencyId];

            if (mainPackage == null || dependencyPackage == null)
            {
                throw new ArgumentException();
            }

            mainPackage.Dependencies.Add(dependencyPackage);
        }

        public bool Contains(Package package) => packageById.ContainsKey(package.Id);

        public int Count() => packageById.Count;

        public IEnumerable<Package> GetDependants(Package package) =>
            packages.Where(p => p.Dependencies.Contains(package));

        public IEnumerable<Package> GetIndependentPackages() =>
            packages.Where(p => p.Dependencies.Count == 0).OrderByDescending(p => p.ReleaseDate).ThenBy(p => p.Version);

        public IEnumerable<Package> GetOrderedPackagesByReleaseDateThenByVersion() =>
            packages.OrderByDescending(p => p.ReleaseDate).ThenBy(p => p.Version).ThenByDescending(p => p.Version);

        public void RegisterPackage(Package package)
        {
            var samePackage = packages.FirstOrDefault(p => p.Name == package.Name && p.Version == package.Version);

            if (samePackage != null)
            {
                throw new ArgumentException();
            }

            packages.Add(package);
            packageById.Add(package.Id, package);
        }

        public void RemovePackage(string packageId)
        {
            if (!packageById.ContainsKey(packageId))
            {
                throw new ArgumentException();
            }

            var packageToRemove = packageById[packageId];

            packages.Remove(packageToRemove);
            packageById.Remove(packageId);

            foreach (var package in packages)
            {
                package.Dependencies.Remove(packageToRemove);
            }
        }
    }
}