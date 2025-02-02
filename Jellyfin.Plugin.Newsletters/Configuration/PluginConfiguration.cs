using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Newsletters.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    public PluginConfiguration()
    {
        // set default options here
        DebugMode = false;

        // default Server Details
        SMTPServer = "smtp.gmail.com";
        SMTPPort = 587;
        SMTPUser = string.Empty;
        SMTPPass = string.Empty;

        // default Email Details
        ToAddr = string.Empty;
        FromAddr = "JellyfinNewsletter@donotreply.com";
        Subject = "Jellyfin Newsletter";
        Body = string.Empty;

        // default Scraper config
        ApiKey = string.Empty;

        // System Paths
        DataPath = string.Empty;
        TempDirectory = string.Empty;
        PluginsPath = string.Empty;
        ProgramDataPath = string.Empty;
        SystemConfigurationFilePath = string.Empty;
        ProgramSystemPath = string.Empty;
        LogDirectoryPath = string.Empty;

        // default newsletter paths
        NewsletterFileName = string.Empty;
        MyDataDir = string.Empty;
        CurrRunListFileName = "Currlist.txt";
        ArchiveFileName = "Archive.txt";
        NewsletterDataFileName = "NewsletterList.txt";
        NewsletterDir = string.Empty;

        // default libraries
        MoviesEnabled = true;
        SeriesEnabled = true;

        // poster hosting
        PHType = "Imgur";
        Hostname = string.Empty;
    }

    /// <summary>
    /// Gets or sets a value indicating whether debug mode is enabled..
    /// </summary>
    public bool DebugMode { get; set; }

    // Server Details

    /// <summary>
    /// Gets or sets a value indicating whether some true or false setting is enabled..
    /// </summary>
    public string SMTPServer { get; set; }

    /// <summary>
    /// Gets or sets an integer setting.
    /// </summary>
    public int SMTPPort { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string SMTPUser { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string SMTPPass { get; set; }

    // -----------------------------------

    // Email Details

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string ToAddr { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string FromAddr { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string Body { get; set; }

    // -----------------------------------

    // Scraper Config

    /// <summary>
    /// Gets or sets a value indicating hosting type.
    /// </summary>
    public string PHType { get; set; }

    /// <summary>
    /// Gets or sets a value for JF hostname accessible outside of network.
    /// </summary>
    public string Hostname { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string ApiKey { get; set; }

    // -----------------------------------

    // System Paths

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string PluginsPath { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string TempDirectory { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string DataPath { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string ProgramDataPath { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string SystemConfigurationFilePath { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string ProgramSystemPath { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string LogDirectoryPath { get; set; }

    // -----------------------------------

    // Newsletter Paths

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string NewsletterFileName { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string MyDataDir { get; set; }

    /// <summary>
    /// Gets a string setting.
    /// </summary>
    public string CurrRunListFileName { get; }

    /// <summary>
    /// Gets a string setting.
    /// </summary>
    public string ArchiveFileName { get; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string NewsletterDir { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string NewsletterDataFileName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Series should be scanned.
    /// </summary>
    public bool SeriesEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Movies should be scanned.
    /// </summary>
    public bool MoviesEnabled { get; set; }
}
