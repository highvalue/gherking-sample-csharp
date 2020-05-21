using System;
using System.Threading.Tasks;

namespace Gherkin.BuildingBlocks.Utils.OptionPattern
{
    public abstract class Option<T>
    {
        public static implicit operator Option<T>(T value) =>
           value.AsOption();

        public static implicit operator Option<T>(None none) =>
            new None<T>();

        /// <summary>
        /// Creates an Option for the result of the given continuation <paramref name="map"/>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public abstract Option<TResult> Map<TResult>(Func<T, TResult> map);
        public abstract Task<Option<TResult>> Map<TResult>(Func<T, Task<TResult>> map);

        public abstract Task<Option<TResult>> MapOptional<TResult>(Func<T, Task<Option<TResult>>> map);

        /// <summary>
        /// Executes the given continuation <paramref name="action"/>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public abstract Option<T> Do(Action<T> action);

        /// <summary>
        /// Creates an Option for the result of the given continuation <paramref name="map"/>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public abstract Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map);

        /// <summary>
        /// Returns the wrapped instance or the default value defined in <paramref name="whenNone"/>
        /// </summary>
        /// <param name="whenNone"></param>
        /// <returns></returns>
        public abstract T Reduce(T whenNone);
        public abstract T Reduce(Func<T> whenNone);

        public abstract TResult Match<TResult>(Func<TResult> none, Func<T, TResult> some);
        public abstract void Match(Action none, Action<T> some);
    }

    public sealed class Some<T> : Option<T>
    {
        public T Content { get; }

        public Some(T value)
        {
            this.Content = value;
        }

        public static implicit operator T(Some<T> some) =>
            some.Content;

        public override Option<TResult> Map<TResult>(Func<T, TResult> map) =>
            map(this.Content);

        public override Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) =>
            map(this.Content);

        public override T Reduce(T whenNone) =>
            this.Content;

        public override T Reduce(Func<T> whenNone) =>
            this.Content;

        public override Option<T> Do(Action<T> map)
        {
            map(this.Content);
            return this;
        }

        public override TResult Match<TResult>(Func<TResult> none, Func<T, TResult> some)
        {
            return some(this.Content);
        }

        public override void Match(Action none, Action<T> some)
        {
            some(this.Content);
        }

        public override async Task<Option<TResult>> MapOptional<TResult>(Func<T, Task<Option<TResult>>> map)
        {
           return await map(this.Content) ;
        }

        public override async Task<Option<TResult>> Map<TResult>(Func<T, Task<TResult>> map)
        {
            return await map(this.Content);
        }
    }

    public sealed class None<T> : Option<T>
    {
        public override Option<TResult> Map<TResult>(Func<T, TResult> map) =>
            None.Value;

        public override Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) =>
            None.Value;

        public override T Reduce(T whenNone) =>
            whenNone;

        public override T Reduce(Func<T> whenNone) =>
            whenNone();

        public override Option<T> Do(Action<T> map)
        {
            return None.Value;
        }

        public override TResult Match<TResult>(Func<TResult> none, Func<T, TResult> some)
        {
            return none();
        }

        public override void Match(Action none, Action<T> some)
        {
            none();
        }

        public override async Task<Option<TResult>> MapOptional<TResult>(Func<T, Task<Option<TResult>>> map) =>
          await Task.FromResult(new None<TResult>());

        public override async Task<Option<TResult>> Map<TResult>(Func<T, Task<TResult>> map) =>        
            await Task.FromResult(new None<TResult>());        
    }

    public sealed class None
    {
        public static None Value { get; } = new None();

        private None() { }
    }
}
