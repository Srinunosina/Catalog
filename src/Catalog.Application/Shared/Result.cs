/**
namespace Catalog.Application.Shared;

public interface IResult
{
    bool IsSuccess { get; }
    Error? Error { get; }
}

public abstract class ResultBase : IResult
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected ResultBase(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
}

public sealed class Result : ResultBase
{
    private Result(bool isSuccess, Error? error) : base(isSuccess, error){}
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}

public sealed class Result<T> : ResultBase
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        Value = value;
    }
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error)  => new(false, default, error);
}

public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new("", "", default);
}
public enum ErrorType
{
    Validation,
    Domain,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Infrastructure
}
**/

/**
    /// <summary>
    /// Option A
    ///  Separate Class based no inheritance and limited to two response types
    ///  Not flexible and difficult to extend
    /// </summary>
     public class Result
     {
         public bool IsSuccess { get; }
         public Error Error { get; }

         protected Result(bool isSuccess, Error error)
         {
             IsSuccess = isSuccess;
             Error = error;
         }

         public static Result Success() => new(true, Error.None);
         public static Result Failure(Error error) => new(false, error);
     }
     public sealed class Result<T> : Result
     {
         public T Value { get; }

         private Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
         {
             Value = value;
         }

         public static Result<T> Success(T value) => new(true, value, Error.None);

         public static Result<T> Failure(Error error) => new(false, default!, error);
     }

**/

/**
    /// <summary>
    /// Option B
    ///  BaseClass based and open to any response types and maxinum resability of inheritance
    /// </summary>
    public abstract class ResultBase
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        protected ResultBase(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
    public sealed class Result : ResultBase
    {
        private Result(bool isSuccess, Error? error)
            : base(isSuccess, error)
        {
        }

        public static Result Success() => new(true, null);

        public static Result Failure(Error error)
            => new(false, error);
    }
    public sealed class Result<T> : ResultBase
    {
        public T? Value { get; }

        private Result(bool isSuccess, T? value, Error? error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
            => new(true, value, null);

        public static Result<T> Failure(Error error)
            => new(false, default, error);
    }

 **/

/**
    /// <summary>
    /// Option C
    ///  IResult interface based with maximum flexibility and open to any response types
    /// </summary>
        public interface IResult
        {
            bool IsSuccess { get; }
            Error? Error { get; }
        }
        public sealed class Result : IResult
        {
            public bool IsSuccess { get; }
            public Error? Error { get; }

            private Result(bool isSuccess, Error? error)
            {
                IsSuccess = isSuccess;
                Error = error;
            }

            public static Result Success() => new(true, null);

            public static Result Failure(Error error)
                => new(false, error);
        }
        public sealed class Result<T> : IResult
        {
            public bool IsSuccess { get; }
            public Error? Error { get; }
            public T? Value { get; }

            private Result(bool isSuccess, T? value, Error? error)
            {
                IsSuccess = isSuccess;
                Value = value;
                Error = error;
            }

            public static Result<T> Success(T value) => new(true, value, null);
            public static Result<T> Failure(Error error) => new(false, default, error);
        }

  **/