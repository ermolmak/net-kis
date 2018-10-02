﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVector
{
    public enum VectorRelation
    {
        General,
        Parallel,
        Orthogonal
    }

    // Для создания методов расширения нужен статический класс, в котором в статических методах у первого аргумента 
    // добавляется ключевое слово this, далее можно вызывать данный метод у объектов класса такого аргумента 
    // Ниже нужно реализовать методы-расширения для нашего вектора
    // И не забыть про документацию и тесты
    public static class VectorExtensions
    {
        /// <summary>
        /// Нормализация вектора
        /// TODO: Length=0?
        /// </summary>
        /// <param name="v">Вектор</param>
        /// <returns>Вектор длины 1, сонаправленный вектору v</returns>
        public static Vector Normalize(this Vector v) => v / v.Length();

        /// <summary>
        /// Получить угол между векторами в радианах
        /// TODO: Length=0?
        /// </summary>
        /// <param name="v">Первый вектор</param>
        /// <param name="u">Второй вектор</param>
        /// <returns>Угол между векторами v и u</returns>
        public static double GetAngleBetween(this Vector v, Vector u) =>
            Math.Acos(v.DotProduct(u) / (v.Length() * u.Length()));

        /// <summary>
        /// Получить отношение векторов: параллельны, перпендикулярны, остальное
        /// TODO: Length=0?
        /// </summary>
        /// <param name="v">Первый вектор</param>
        /// <param name="u">Второй вектор</param>
        /// <returns>
        /// VectorRelation:
        /// Parallel, если параллельны
        /// Orthogonal, если перпендикулярны
        /// General, иначе
        /// </returns>
        public static VectorRelation GetRelation(this Vector v, Vector u)
        {
            if (Math.Abs(v.CrossProduct(u)) < 1e-4) // TODO: think about it
            {
                return VectorRelation.Parallel;
            }

            return Math.Abs(v.DotProduct(u)) < 1e-4 // TODO: and here
                ? VectorRelation.Orthogonal
                : VectorRelation.General;
        }

        /// <summary>
        /// Еденичный вектор, ортогональный данному, полученный поворотом на 90
        /// градусов против часовой стрелки
        /// TODO: Length=0?
        /// </summary>
        /// <param name="v">Вектор</param>
        /// <returns>
        /// Единичный вектор, повёрнутый относительно v на 90 градусов
        /// против часовой стрелки
        /// </returns>
        public static Vector GetOrthogonal(this Vector v) =>
            new Vector(-v.Y, v.X).Normalize();

        /// <summary>
        /// Повернуть вектор на заданный угол
        /// </summary>
        /// <param name="v">Вектор</param>
        /// <param name="angle">
        /// Величина угла в радианах, на который осуществляется поворот
        /// против часовой стрелки
        /// </param>
        /// <returns>Результат поворота вектора v на угол angle</returns>
        public static Vector Rotate(this Vector v, double angle) =>
            new Vector(v.X * Math.Cos(angle) - v.Y * Math.Sin(angle),
                v.X * Math.Sin(angle) + v.Y * Math.Cos(angle));
    }
}