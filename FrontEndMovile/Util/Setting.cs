namespace FrontEndMovile.Util
{
    /// <summary>
    /// Interface para acceder a los valores definidos en el archivo appsettings.json
    /// </summary>
    public interface ISetting
    {
        string BackendApiUrl { get; }
    }

    /// <summary>
    /// clase que implementa la interfaz ISetting
    /// </summary>
    public class Setting: ISetting
    {
        public string BackendApiUrl { get; set; }
    }

   

}
