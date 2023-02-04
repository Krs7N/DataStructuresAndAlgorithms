using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        private HashSet<Route> routes = new HashSet<Route>();
        private Dictionary<string, Route> routeById = new Dictionary<string, Route>();

        public int Count => routes.Count;

        public void AddRoute(Route route)
        {
            if (routes.Contains(route))
            {
                throw new ArgumentException();
            }

            routeById.Add(route.Id, route);
            routes.Add(route);
        }

        public void ChooseRoute(string routeId)
        {
            if (!routeById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            routeById[routeId].Popularity++;
        }

        public bool Contains(Route route) => routes.Contains(route);

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint) =>
            routes.Where(r => r.IsFavorite && r.LocationPoints.IndexOf(destinationPoint) > 0).OrderBy(r => r.Distance).ThenByDescending(r => r.Popularity);

        public Route GetRoute(string routeId)
        {
            if (!routeById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            return routeById[routeId];
        }

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints() => routes
            .OrderByDescending(r => r.Popularity).ThenBy(r => r.Distance).ThenBy(r => r.LocationPoints.Count).Take(5);

        public void RemoveRoute(string routeId)
        {
            if (!routeById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            var route = routeById[routeId];

            routeById.Remove(routeId);
            routes.Remove(route);
        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint) => routes
            .Where(r => r.LocationPoints.Contains(startPoint)
                        && r.LocationPoints.Contains(endPoint)
                        && r.LocationPoints.IndexOf(startPoint) < r.LocationPoints.IndexOf(endPoint))
            .OrderBy(r => r.IsFavorite)
            .ThenBy(r => r.LocationPoints.IndexOf(endPoint) - r.LocationPoints.IndexOf(startPoint))
            .ThenByDescending(r => r.Popularity);
    }
}
