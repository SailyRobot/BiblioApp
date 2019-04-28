using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BiblioApp.Model
{
    /*    public class Livre
        {

            public string Titre { get; set; }
            public string Auteur { get; set; }
            public string Description { get; set; }


            public string TitreMaj { get { return Titre.PremiereLettreMaj(); } }

            public Livre()
            {
            }
        }
    */
    public partial class Livre
    {
        [JsonProperty("info_url")]
        public Uri InfoUrl { get; set; }

        [JsonProperty("bib_key")]
        public string Isbn { get; set; }

        [JsonProperty("preview_url")]
        public Uri PreviewUrl { get; set; }

        [JsonProperty("thumbnail_url")]
        public Uri ThumbnailUrl { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }

        [JsonProperty("preview")]
        public string Preview { get; set; }
    }

    public partial class Details 
    {
        [JsonProperty("contributors")]
        public List<Contributor> Contributors { get; set; }

        [JsonProperty("covers")]
        public List<long> Covers { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("languages")]
        public List<TypeElement> Languages { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("authors")]
        public List<Author> Authors { get; set; }

        [JsonProperty("number_of_pages")]
        public long NumberOfPages { get; set; }
    }

    public partial class TypeElement
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public partial class Contributor
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Created
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public DateTimeOffset Value { get; set; }
    }

}
