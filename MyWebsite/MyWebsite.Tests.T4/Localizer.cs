﻿namespace Resources
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public interface ILocalizer
    {
        string GetString(Type category, string resourceKey);

        string GetString(string category, string resourceKey);

        string GetString(Type category, string resourceKey, string culture);

        string GetString(string category, string resourceKey, string culture);
    }

    public class Localizer : ILocalizer
    {
        private const string DefaultCulture = "en-gb";
        //private const string _resourceFolder = "Resources";
        private static readonly string _resourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        private static readonly Lazy<Dictionary<string, Dictionary<string, string>>> _resources = new Lazy<Dictionary<string, Dictionary<string, string>>>(LoadResources);
        private CultureInfo _culture;
        private Message _Message;
        private Text _Text;

        public Localizer(string culture = DefaultCulture)
        {
            _culture = new CultureInfo(culture);
        }

        public Message Message { get { if (_Message == null) { _Message = new Message(this); } return _Message; } }

        public Text Text { get { if (_Text == null) { _Text = new Text(this); } return _Text; } }

        #region ILocalizer

        public string GetString(Type category, string resourceKey)
        {
            return GetString(category.Name.ToString(), resourceKey);
        }

        public string GetString(string category, string resourceKey)
        {
            return GetString(category, resourceKey, _culture.Name);
        }

        public string GetString(Type category, string resourceKey, string culture)
        {
            return GetString(category.Name.ToString(), resourceKey, culture);
        }

        public string GetString(string category, string resourceKey, string culture)
        {
            var resource = GetResource($"{category}.{culture}") ?? GetResource($"{category}.{DefaultCulture}");
            if (resource == null)
            {
                return resourceKey;
            }
            else
            {
                return resource.SingleOrDefault(r => r.Key.Contains(resourceKey)).Value ?? resourceKey;
            }
        }

        #endregion ILocalizer

        #region Private Methods

        private static Dictionary<string, Dictionary<string, string>> LoadResources()
        {
            var files = Directory.GetFiles(_resourceFolder, "*.resx");
            var resources = files.ToDictionary(file => Path.GetFileNameWithoutExtension(file), file =>
            {
                var xdoc = XDocument.Load(file);
                var dictionary = xdoc.Root.Elements("data").ToDictionary(e => e.Attribute("name").Value, e => e.Element("value").Value);
                return dictionary;
            }, StringComparer.CurrentCultureIgnoreCase);
            return resources;
        }

        private Dictionary<string, string> GetResource(string key)
        {
            return _resources.Value.SingleOrDefault(r => r.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).Value;
        }

        #endregion Private Methods
    }

    public abstract class ResourceBase
    {
        protected ResourceBase(ILocalizer localizer)
        {
            Localizer = localizer;
        }

        protected ILocalizer Localizer { get; private set; }

        protected string GetString(string resourceKey)
        {
            return Localizer.GetString(GetType(), resourceKey);
        }
    }

    public class Message : ResourceBase
    {
        public Message(ILocalizer localizer) : base(localizer)
        {
        }

        public string Hello { get { return GetString("Hello"); } }
    }

    public class Text : ResourceBase
    {
        public Text(ILocalizer localizer) : base(localizer)
        {
        }

        public string Hello { get { return GetString("Hello"); } }
    }
}