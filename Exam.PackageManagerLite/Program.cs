using System;

namespace Exam.PackageManagerLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PackageManager packageManager = new PackageManager();
            Package package = new Package("1", "pesho", DateTime.Now, "777");
            Package package2 = new Package("2", "pesho", DateTime.Now, "999");
            Package package3 = new Package("3", "zaro", DateTime.Now, "888");
            Package package4 = new Package("4", "gego", DateTime.Now, "666");
            Package package5 = new Package("5", "chego", DateTime.Now, "555");

            packageManager.RegisterPackage(package);
            packageManager.RegisterPackage(package2);
            packageManager.RegisterPackage(package3);
            packageManager.RegisterPackage(package4);
            packageManager.RegisterPackage(package5);
            packageManager.AddDependency("1", "2");
            packageManager.AddDependency("3", "2");
            packageManager.AddDependency("4", "2");
            
            packageManager.RemovePackage("2");

            Console.WriteLine(string.Join(", ", packageManager.GetOrderedPackagesByReleaseDateThenByVersion()));
        }
    }
}
