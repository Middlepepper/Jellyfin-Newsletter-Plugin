#pragma warning disable 1591, SYSLIB0014, CA1002, CS0162
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Newsletters.Configuration;
using Jellyfin.Plugin.Newsletters.LOGGER;
using Jellyfin.Plugin.Newsletters.Scripts.SCRAPER;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Newsletters.Scripts.NLDataGenerator;

public class NewsletterDataGenerator
{
    // Global Vars
    // Readonly
    private readonly PluginConfiguration config;
    private readonly string newslettersDir;
    private readonly string newsletterDataFile;

    private readonly string currRunList;
    private readonly string archiveFile;
    private readonly string myDataDir;
    private Logger logger;

    // Non-readonly
    private static string append = "Append";
    private static string write = "Overwrite";
    private List<JsonFileObj> archiveSeriesList;
    // private List<string> fileList;

    public NewsletterDataGenerator()
    {
        logger = new Logger();
        config = Plugin.Instance!.Configuration;
        myDataDir = config.TempDirectory + "/Newsletters";

        archiveFile = config.MyDataDir + config.ArchiveFileName; // curlist/archive
        currRunList = config.MyDataDir + config.CurrRunListFileName;
        newsletterDataFile = config.MyDataDir + config.NewsletterDataFileName;

        archiveSeriesList = new List<JsonFileObj>();
        newslettersDir = config.NewsletterDir; // newsletterdir
        Directory.CreateDirectory(newslettersDir);
    }

    public Task GenerateDataForNextNewsletter()
    {
        // progress.Report(25);
        archiveSeriesList = PopulateFromArchive(); // Files that shouldn't be processed again
        // progress.Report(50);
        GenerateData();
        // progress.Report(75);
        CopyCurrRunDataToNewsletterData();
        // progress.Report(99);

        return Task.CompletedTask;
    }

    public List<JsonFileObj> PopulateFromArchive()
    {
        List<JsonFileObj> myObj = new List<JsonFileObj>();
        if (File.Exists(archiveFile))
        {
            StreamReader sr = new StreamReader(archiveFile);
            string arFile = sr.ReadToEnd();
            foreach (string series in arFile.Split(";;;"))
            {
                JsonFileObj? currArcObj = JsonConvert.DeserializeObject<JsonFileObj?>(series);
                if (currArcObj is not null)
                {
                    myObj.Add(currArcObj);
                }
            }

            sr.Close();
        }

        return myObj;
    }

    private void GenerateData()
    {
        StreamReader sr = new StreamReader(currRunList); // curlist/archive
        string readScrapeFile = sr.ReadToEnd();

        foreach (string? ep in readScrapeFile.Split(";;;"))
        {
            JsonFileObj? obj = JsonConvert.DeserializeObject<JsonFileObj?>(ep);
            if (obj is not null)
            {
                JsonFileObj currObj = new JsonFileObj();
                currObj.Title = obj.Title;
                archiveSeriesList.Add(currObj);
            }

            break;
        }

        sr.Close();
    }

    public string FetchImagePoster(string title)
    {
        // string url = "https://www.googleapis.com/customsearch/v1?key=" + config.ApiKey + "&cx=" + config.CXKey + "&num=1&searchType=image&fileType=jpg&q=" + string.Join("%", (title + " series + cover + art").Split(" "));
        // string url = "http://" + GetIP() + ":8096/Items/1d288ad3613f82f523b6a9353f608bde/Images/Primary";

        // local posters are located in /config/metadata/library/XX/ItemID/poster.jpg
        // use config.ProgramDataPath (this points to /config)
        // can parse all directories to get correct poster, but need to have ItemId
        // then need to upload image to repo (imgur?) and get Imgurl from response (see link below for imgur api)
        // https://apidocs.imgur.com/#c85c9dfc-7487-4de2-9ecd-66f727cf3139

        // string posterFilePath = GetPosterFilePath();
        // string urlToImgurPoster = UploadToImgur(posterFilePath);
        // return urlToImgurPoster;
        // https://github.com/jellyfin/jellyfin/issues/2246

        string url = "http://" + GetIP() + ":8096/Items/Counts";
        logger.Debug("Image Search URL: " + url);
        // return "https://m.media-amazon.com/images/W/IMAGERENDERING_521856-T1/images/I/91eNqTeYvzL.jpg";

        // HttpClient hc = new HttpClient();
        // string res = await hc.GetStringAsync(url).ConfigureAwait(false);
        string res;
        WebClient wc = new WebClient();
        try
        {
            res = wc.DownloadString(url);
            logger.Info("Response: " + res);
            return string.Empty;
            string urlResFile = myDataDir + "/.lasturlresponse";

            // can pass response directly into forloop below?
            WriteFile(write, urlResFile, res);

            bool testForItems = false;

            foreach (string line in File.ReadAllLines(urlResFile))
            {
                if (testForItems)
                {
                    if (line.Contains("\"link\":", StringComparison.OrdinalIgnoreCase))
                    {
                        string fetchedURL = line.Split("\"")[3];

                        logger.Info("Fetched Image: " + fetchedURL);
                        if (fetchedURL.Length == 0)
                        {
                            logger.Warn("Image URL failed to be captured. Is this an error?");
                        }

                        return fetchedURL; // Actual URL
                    }
                }
                else
                {
                    if (line.Contains("\"items\":", StringComparison.OrdinalIgnoreCase))
                    {
                        testForItems = true;
                    }
                }
            }
        }
        catch (WebException e)
        {
            logger.Warn("Unable to get proper response from googleapi: " + e);
            return string.Empty;
        }

        return string.Empty;
    }

    private string GetIP()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        logger.Info("Server DNS: " + ipAddress.ToString());
        return ipAddress.ToString();
    }

    private void CopyCurrRunDataToNewsletterData()
    {
        if (File.Exists(currRunList)) // archiveFile
        {
            Stream input = File.OpenRead(currRunList);
            Stream output = new FileStream(newsletterDataFile, FileMode.Append, FileAccess.Write, FileShare.None);
            input.CopyTo(output);
            File.Delete(currRunList);
        }
    }

    private void WriteFile(string method, string path, string value)
    {
        if (method == append)
        {
            File.AppendAllText(path, value);
        }
        else if (method == write)
        {
            File.WriteAllText(path, value);
        }
    }
}