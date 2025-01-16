using FluentValidation;
using MediatR;

namespace Ges
{
    public enum CommandAction
    {
        Add,
        Update,
        Delete,
        Select
    }
    /// <summary>
    /// create, update, delete
    /// </summary>
    public abstract class CmdBase
    {
        public Dictionary<string, string[]> Errors { get; set; } = [];
        public bool HasErrors => Errors.Count > 0;
    }

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : CmdBase
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
           this. validators = validators;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            if (this.validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    this.validators.Select(v =>v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .Select(f => new { f.PropertyName, f.ErrorMessage })
                    .GroupBy(x => x.PropertyName)
                    .OrderBy(x=>x.Key)
                    .ToDictionary(f => f.Key, f => f.Select(x => x.ErrorMessage).ToArray());

                if (failures.Any())
                {
                    var cmdBase = request as CmdBase;
                    cmdBase.Errors = failures;
                    return cmdBase as TResponse;
                }
            }

            return await next();
        }
    }
}
