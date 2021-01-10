namespace People.Models.Settings
{

    public class WSettings
    {
        public WSettings()
        {
            ConnectionStrings = new ConnectionStrings();
            AppSettings = new AppSettings();
        }

        public ConnectionStrings ConnectionStrings { get; set; }
        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        public AppSettings()
        {
            Common = new Common();
        }


        public Common Common { get; set; }
    }



    public class Common
    {
        public string Test { get; set; }
    }

}
