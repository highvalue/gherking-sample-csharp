using System;
using System.Collections.Generic;
using System.Linq;

namespace Gherkin.BuildingBlocks.Utils.OptionPattern
{
   public static class OptionExtensions
    {
        /// <summary>
        /// Returns an Option for object <paramref name="obj"/>. Option is None if <paramref name="condition"/> is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Option<T> When<T>(this T obj, bool condition) =>
        condition ? (Option<T>)new Some<T>(obj) : None.Value;

        /// <summary>
        /// Returns an Option for object <paramref name="obj"/>. Option is None if <paramref name="predicate"/> is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Option<T> When<T>(this T obj, Func<T, bool> predicate) =>
            obj.When(predicate(obj));

        /// <summary>
        /// Returns an Option for object <paramref name="obj"/>. Option is None if <paramref name="predicate"/> is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Option<T> IfNone<T>(this Option<T> obj, Func<T> continuation)
        {
            if (obj is None<T>)
            {
                return continuation().AsOption();
            }
            else
            {
                return obj;
            } 
        }

        /// <summary>
        /// Executes <paramref name="continuation"/> if Option is None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Option<T> IfNone<T>(this Option<T> obj, Action continuation)
        {
            if (obj is None<T>)
            {
                continuation();
            }       
            
            return obj;
        }

        public static Option<T> WhenOptional<T>(this Option<T> obj, bool condition) =>
       condition ? obj : None.Value;

        /// <summary>
        /// Returns an Option for object <paramref name="obj"/>. Option is None if <paramref name="predicate"/> is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Option<T> WhenOptional<T>(this Option<T> obj, Func<T, bool> predicate)
        {
            if (obj is Some<T> s)
            {
                return obj.WhenOptional(predicate(s.Content));
            }
            else
            {
               return obj;
            }           
        }

        /// <summary>
        /// Wraps the given <paramref name="obj"/> in an Option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Option<T> AsOption<T>(this T obj) =>
            obj.When(!object.ReferenceEquals(obj, null));

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence) =>
            sequence.Select(x => (Option<T>)new Some<T>(x))
                .DefaultIfEmpty(None.Value)
                .First();

        public static Option<T> FirstOrNone<T>(
            this IEnumerable<T> sequence, Func<T, bool> predicate) =>
            sequence.Where(predicate).FirstOrNone();

        public static IEnumerable<TResult> SelectOptional<T, TResult>(
            this IEnumerable<T> sequence, Func<T, Option<TResult>> map) =>
            sequence.Select(map)
                .OfType<Some<TResult>>()
                .Select(some => some.Content);

    }
}
