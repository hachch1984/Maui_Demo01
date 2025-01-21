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




    #region interfaces


    public interface ICmdBase_Errors
    {
        public Dictionary<string, string[]> Errors { get; set; }
    }
    public interface ICmdBase_DataNickName : ICmdBase_Errors
    {
        public string Data_NickName { get; }
    }

    #endregion


    #region abstract classes


    /// <summary>
    /// comando que recibe datos y devuelve resultados
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class CmdBase_Data_Result<TData, TResult> : ICmdBase_DataNickName
    {
        public string Data_NickName { get; protected set; } = string.Empty;
        public CmdBase_Data_Result<TData, TResult> Set_DataNickName(string dataNickName)
        {
            this.Data_NickName = dataNickName;
            return this;
        }

        
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public bool HasErrors => Errors.Count > 0;

        /// <summary>
        /// datos de entrada para el comando
        /// </summary>
        protected TData Data { get; }
        /// <summary>
        /// resultado lueego de la ejecucion del comando    
        /// </summary>
        public TResult? Result { get; protected set; } = default;


        protected CmdBase_Data_Result(TData data)
        {
            this.Data = data;
        }
    }

    /// <summary>
    /// comando que solo devuelve resultados
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class CmdBase_Result<TResult> : ICmdBase_Errors
    {
        /// <summary>
        /// detalle de errores
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        /// <summary>
        /// indica si hay errores
        /// </summary>
        public bool HasErrors => Errors.Count > 0;

        /// <summary>
        /// resultado luego de la ejecucion del comando
        /// </summary>
        public TResult? Result { get; protected set; } = default;
    }

    /// <summary>
    /// comando que solo recibe datos
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class CmdBase_Data<TData> : ICmdBase_DataNickName
    {
        /// <summary>
        /// devuelve el ninckname de la propiedad de data
        /// </summary>
        public string Data_NickName { get; protected set; } = string.Empty;
        /// <summary>
        /// define el ninckname de la propiedad de data para mostrar en los errores
        /// </summary>
        /// <param name="dataNickName"></param>
        /// <returns></returns>
        public CmdBase_Data<TData> Set_DataNickName(string dataNickName)
        {
            this.Data_NickName = dataNickName;
            return this;
        }

        /// <summary>
        /// detalle de errores
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        /// <summary>
        /// indica si hay errores
        /// </summary>
        public bool HasErrors => Errors.Count > 0;

        /// <summary>
        /// datos de entrada para el comando
        /// </summary>
        public TData Data { get; protected set; } = default;
        protected CmdBase_Data(TData data)
        {
            this.Data = data;
        }

    }


    #endregion





    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (this.validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .Select(f => new { f.PropertyName, f.ErrorMessage })
                    .GroupBy(x => x.PropertyName)
                    .OrderBy(x => x.Key)
                    .ToDictionary(f => f.Key, f => f.Select(x => x.ErrorMessage).ToArray());

                if (failures.Any())
                {
                    var type = request.GetType();
                    var baseType = type.BaseType;

                    if (baseType.Name == typeof(CmdBase_Data_Result<,>).Name ||
                        baseType.Name == typeof(CmdBase_Data<>).Name)
                    {
                        var obj = request as ICmdBase_DataNickName;

                        if (string.IsNullOrEmpty(obj.Data_NickName) == false && failures.Count == 1 && failures.ContainsKey("Data"))
                        {
                            obj.Errors.Add(obj.Data_NickName, failures["Data"]);
                        }
                        else
                        {
                            obj.Errors = failures;
                        }
                        return obj as TResponse;
                    }
                    else if (baseType.Name == typeof(CmdBase_Result<>).Name)
                    {
                        var obj = request as ICmdBase_Errors;
                        obj.Errors = failures;
                        return obj as TResponse;
                    }
                    else
                    {
                        throw new Exception("Not Valid Command");
                    }

                }
            }

            return await next();
        }
    }
}
