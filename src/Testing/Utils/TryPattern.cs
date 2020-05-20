using System;
using System.Collections.Generic;

namespace Gherkin.Testing.Utils
{
    public class Try<T>
    {
        public Either<Exception, T> Result { get; set; }

        public Try(Func<T> @try)
        {
            try
            {
                Result = new Success<Exception, T>(@try());
            }
            catch (Exception e)
            {
                Result = new Fail<Exception, T>(e);
            }
        }

        public Try(Exception exception)
        {
            Result = new Fail<Exception, T>(exception);
        }

        public Try(T value)
        {
            Result = new Success<Exception, T>(value);
        }

        public static implicit operator Try<T>(T value) =>
         new Try<T>(value);

        public static implicit operator Try<T>(Exception value) =>
            new Try<T>(value);

        public Try<T1> Select<T1>(Func<T, T1> func)
        {
            return Result.Resolve(
                fail: e => new Try<T1>(e),
                success: result => new Try<T1>(() => func(result)));
        }
        public Try<T1> SelectMany<T1>(Func<T, Try<T1>> func)
        {
            return Result.Resolve(
             fail: e => new Try<T1>(e),
             success: result => func(result));
        }

    }

    public abstract class Either<TFail, TSuccess>
    {
        public abstract void IfFail(Action<TFail> action);
        public abstract void IfSuccess(Action<TSuccess> action);
        public abstract Either<TFail, T1Success> TransformSuccess<T1Success>(Func<TSuccess, T1Success> mapping);
        public abstract TResult Resolve<TResult>(Func<TFail, TResult> fail, Func<TSuccess, TResult> success);
        public abstract TResult Resolve<TResult>(Action<TFail> fail, Func<TSuccess, TResult> success);
        public abstract TResult Resolve<TResult>(Func<TFail, TResult> fail, Action<TSuccess> success);

        public static implicit operator Either<TFail, TSuccess>(TSuccess value) =>
                new Success<TFail, TSuccess>(value);

        public static implicit operator Either<TFail, TSuccess>(TFail value) =>
           new Fail<TFail, TSuccess>(value);
    }
    public class Fail<TFail, TSuccess> : Either<TFail, TSuccess>
    {
        private readonly TFail value;

        public Fail(TFail left)
        {
            this.value = left;
        }
        public override void IfFail(Action<TFail> action)
        {
            action(value);
        }

        public override void IfSuccess(Action<TSuccess> action) { }

        public override TResult Resolve<TResult>(Func<TFail, TResult> fail, Func<TSuccess, TResult> success)
        {
            return fail(value);
        }

        public override Either<TFail, T1Success> TransformSuccess<T1Success>(Func<TSuccess, T1Success> mapping)
        {
            return new Fail<TFail, T1Success>(value);
        }

        public override TResult Resolve<TResult>(Action<TFail> fail, Func<TSuccess, TResult> success)
        {
            fail(value);
            return default(TResult);
        }

        public override TResult Resolve<TResult>(Func<TFail, TResult> fail, Action<TSuccess> success)
        {
            return fail(value);
        }
    }
    public class Success<TFail, TSuccess> : Either<TFail, TSuccess>
    {
        private readonly TSuccess value;

        public Success(TSuccess value)
        {
            this.value = value;
        }
        public override void IfFail(Action<TFail> action) { }

        public override void IfSuccess(Action<TSuccess> action)
        {
            action(value);
        }

        public override Either<TFail, T1Success> TransformSuccess<T1Success>(Func<TSuccess, T1Success> mapping)
        {
            return new Success<TFail, T1Success>(mapping(value));
        }
        public override TResult Resolve<TResult>(Func<TFail, TResult> fail, Func<TSuccess, TResult> success)
        {
            return success(value);
        }

        public override TResult Resolve<TResult>(Action<TFail> fail, Func<TSuccess, TResult> success)
        {
            return success(value);
        }

        public override TResult Resolve<TResult>(Func<TFail, TResult> fail, Action<TSuccess> success)
        {
            success(value);
            return default(TResult);
        }
    }

    public static class TryPatternExtensions
    {

        public static Try<TOut> TryExecute<TIn, TOut>(this TIn item, Func<TIn, TOut> func)
        {
            return new Try<TOut>(() => func(item));
        }

        public static IEnumerable<Try<TOut>> SelectTry<TIn, TOut>(this IEnumerable<TIn> items, Func<TIn, TOut> func)
        {
            foreach (var item in items)
                yield return new Try<TOut>(() => func(item));
        }

        public static IEnumerable<T> SelectSuccess<T>(this IEnumerable<Try<T>> items)
        {
            foreach (var item in items)
            {
                if (item.Result is Success<Exception, T>)
                    yield return item.Result.Resolve(fail: f => default,
                                                     success: s => s);
            }
        }

        public static IEnumerable<Try<T>> OnFail<T>(this IEnumerable<Try<T>> items, Action<Exception> action)
        {
            foreach (var item in items)
            {
                item.Result.IfFail(x => action(x));
            }

            return items;
        }

    }

}
