namespace FrontEndMovile.Util
{
    public interface ISetting
    {
        string BackendApiUrl { get; }
    }
    public class Setting: ISetting
    {
        public string BackendApiUrl { get; set; }
    }

    public static class GlovalVar {
        public static bool IsDevelopMode => true;

    }

}
