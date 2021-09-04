using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace rab1
{
    public class PlaneManager
    {
        //-----------------------------------------------------------------------------------------------
        //Расстояние между двумя точками
        public static double DistanceBetweenTwoPoints(Point2D pointOne, Point2D pointTwo)
        {
            double subX = pointOne.X - pointTwo.X;
            double subY = pointOne.Y - pointTwo.Y;
            double distance = Math.Sqrt(subX * subX + subY * subY);
            return distance;
        }
        //-----------------------------------------------------------------------------------------------
        //Расстояния между точками массива и заданной точкой
        public static double[] GetDistances(Point2D[] points, Point2D targetPoint)
        {
            double[] distances = new double[points.Length];
            for (int index = 0; index < points.Length; index++)
            {
                Point2D point = points[index];
                double distance = PlaneManager.DistanceBetweenTwoPoints(point, targetPoint);
                distances[index] = distance;
            }
            return distances;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Перемещение точек
        public static Point2D[] DisplacePoints(
            Point2D[] points,
            double displacementX,
            double displacementY
        )
        {
            Point2D[] newPoints = new Point2D[points.Length];
            for (int index = 0; index < points.Length; index++)
            {
                Point2D point = points[index];
                double newX = point.X + displacementX;
                double newY = point.Y + displacementY;
                Point2D newPoint = new Point2D(newX, newY);
                newPoints[index] = newPoint;
            }
            return newPoints;
        }
        //-----------------------------------------------------------------------------------------------
        //Минимальные координаты точек
        public static double[] GetMinimalCoordinates(Point2D[] points)
        {
            double[] coordinatesX = PlaneManager.GetCoordinatesX(points);
            double[] coordinatesY = PlaneManager.GetCoordinatesY(points);

            double minX = coordinatesX.Min();
            double minY = coordinatesY.Min();

            double[] minimalCoordinates = new double[] { minX, minY };
            return minimalCoordinates;
        }
        //-----------------------------------------------------------------------------------------------
        //Координаты X точек
        public static double[] GetCoordinatesX(Point2D[] points)
        {
            double[] coordinatesX = new double[points.Length];
            for (int index = 0; index < points.Length; index++)
            {
                Point2D point = points[index];
                coordinatesX[index] = point.X;
            }
            return coordinatesX;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Координаты Y точек
        public static double[] GetCoordinatesY(Point2D[] points)
        {
            double[] coordinatesY = new double[points.Length];
            for (int index = 0; index < points.Length; index++)
            {
                Point2D point = points[index];
                coordinatesY[index] = point.Y;
            }
            return coordinatesY;
        }
        //-----------------------------------------------------------------------------------------------
        //Перемещение точек в первый квадрант
        public static Point2D[] DisplacePointsToFirstQuadrant(Point2D[] points)
        {
            double[] minimalCoordinates = PlaneManager.GetMinimalCoordinates(points);
            double displacementX = minimalCoordinates[0];
            double displacementY = minimalCoordinates[1];

            Point2D[] newPoints = PlaneManager.DisplacePoints(points, -displacementX, -displacementY);
            return newPoints;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Создание точек
        public static Point2D[] CreatePoints2D(double[] coordinatesX, double[] coordinatesY)
        {
            Point2D[] points = new Point2D[coordinatesX.Length];
            for (int index = 0; index < points.Length; index++)
            {
                double x = coordinatesX[index];
                double y = coordinatesY[index];
                Point2D point = new Point2D(x, y);
                points[index] = point;
            }
            return points;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Выбрать ближайшую к данной точке точку из массива
        public static Point2D GetNearestPoint(Point2D point, Point2D[] points)
        {
            if (points.Length == 1)
            {
                return points[0];
            }
            Point2D nearestPoint = points[0];
            double minDistance = PlaneManager.DistanceBetweenTwoPoints(point, nearestPoint);
            for (int index = 1; index < points.Length; index++)
            {
                Point2D currentPoint = points[index];
                double distance = PlaneManager.DistanceBetweenTwoPoints(point, currentPoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = currentPoint;
                }
            }
            return nearestPoint;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Средняя точка на плоскости
        public static Point2D GetMidPoint(Point2D[] points)
        {
            Point2D accumulatedPoint = new Point2D(0, 0);
            for (int index = 0; index < points.Length; index++)
            {
                Point2D point = points[index];
                accumulatedPoint += point;
            }
            int n = points.Length;
            double resultX = accumulatedPoint.X / n;
            double resultY = accumulatedPoint.Y / n;

            Point2D resultPoint = new Point2D(resultX, resultY);
            return resultPoint;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
    }
}

