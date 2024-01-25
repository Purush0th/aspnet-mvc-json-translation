using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace QuickTranslation.Utils
{
    public static class TranslateUtil
    {

        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly string _translationsFolder =  ConfigurationManager.AppSettings["Translation:JsonFolderPath"];
        private static readonly List<string> _supportedLanguagesList = new List<string>();

        private static readonly object _lockObject = new object();

        static TranslateUtil()
        {
            LoadTranslationsToCache();
            StartFileWatcher();
        }

        public static string Translate(string key, string language = null)
        {
            language = language ?? Thread.CurrentThread.CurrentCulture.Name;

            if (_supportedLanguagesList.Any() && !_supportedLanguagesList.Contains(language.ToLower()))
                language = ConfigurationManager.AppSettings["Translation:DefaultLanguage"];

            var cacheKey = $"{language.ToLower()}_TranslationsCacheKey";
            var translationsCache = _cache.Get(cacheKey) as Dictionary<string, string>;

            if (translationsCache != null && translationsCache.TryGetValue(key, out var translation))
            {
                return translation;
            }

            // Handle missing translation
            return $"{key}";
        }

        private static void LoadTranslationsToCache()
        {
            lock (_lockObject)
            {
                try
                {
                    foreach (var filePath in Directory.EnumerateFiles(_translationsFolder, "*.json"))
                    {
                        var fileName = Path.GetFileNameWithoutExtension(filePath);
                        var language = fileName.ToLower();
                        var json = File.ReadAllText(filePath);
                        var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                        _supportedLanguagesList.Add(language);
                        var cacheKey = $"{language}_TranslationsCacheKey";
                        _cache.Set(cacheKey, translations, new CacheItemPolicy());
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log, return default translations, etc.)
                }
            }
        }

        private static void StartFileWatcher()
        {
            var fileWatcher = new FileSystemWatcher(_translationsFolder);
            fileWatcher.Filter = "*.json";
            fileWatcher.IncludeSubdirectories = false;
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            fileWatcher.Changed += (sender, e) => OnFileChanged();
            fileWatcher.Created += (sender, e) => OnFileChanged();
            fileWatcher.Deleted += (sender, e) => OnFileChanged();
            fileWatcher.EnableRaisingEvents = true;
        }

        private static void OnFileChanged()
        {
            LoadTranslationsToCache();
        }
    }

}