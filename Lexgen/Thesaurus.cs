using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lexgen
{ // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


	public class DefinitionSubdata
	{
		[JsonProperty("isInformal")]
		public object IsInformal { get; set; }

		[JsonProperty("isVulgar")]
		public string IsVulgar { get; set; }

		[JsonProperty("definition")]
		public string Definition { get; set; }

		[JsonProperty("thesRid")]
		public string ThesRid { get; set; }

		[JsonProperty("pos")]
		public string Pos { get; set; }

		[JsonProperty("synonyms")]
		public List<Thesaurus> Synonyms { get; set; }

		[JsonProperty("antonyms")]
		public List<Thesaurus> Antonyms { get; set; }

		[JsonProperty("note")]
		public object Note { get; set; }
	}

	public class DefinitionData
	{
		[JsonProperty("_id")]
		public string Id { get; set; }

		[JsonProperty("entry")]
		public string Entry { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("definitions")]
		public List<DefinitionSubdata> Definitions { get; set; }

		[JsonProperty("slug")]
		public string Slug { get; set; }
	}

	public class Inflection
	{
		[JsonProperty("displayForm")]
		public string DisplayForm { get; set; }

		[JsonProperty("slug")]
		public string Slug { get; set; }
	}

	public class Audio
	{
		[JsonProperty("audio/ogg")]
		public string AudioOgg { get; set; }

		[JsonProperty("audio/mpeg")]
		public string AudioMpeg { get; set; }
	}

	public class Pronunciation
	{
		[JsonProperty("ipa")]
		public string Ipa { get; set; }

		[JsonProperty("spell")]
		public string Spell { get; set; }

		[JsonProperty("audio")]
		public Audio Audio { get; set; }
	}

	public class Source
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("publication_date")]
		public DateTime PublicationDate { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("source_name")]
		public string SourceName { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("abbreviation")]
		public string Abbreviation { get; set; }
	}

	public class ExampleSentence
	{
		[JsonProperty("source")]
		public Source Source { get; set; }

		[JsonProperty("profanity")]
		public int Profanity { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("sentence")]
		public string Sentence { get; set; }
	}

	public class Root
	{
		[JsonProperty("data")]
		public Data Data { get; set; }
	}

	public class Data
	{
		[JsonProperty("definitionData")]
		public DefinitionData DefinitionData { get; set; }

		[JsonProperty("inflections")]
		public List<Inflection> Inflections { get; set; }

		[JsonProperty("pronunciation")]
		public Pronunciation Pronunciation { get; set; }

		[JsonProperty("confusables")]
		public List<string> Confusables { get; set; }

		[JsonProperty("supplementaryNotes")]
		public List<object> SupplementaryNotes { get; set; }

		[JsonProperty("etymology")]
		public List<object> Etymology { get; set; }

		[JsonProperty("exampleSentences")]
		public List<ExampleSentence> ExampleSentences { get; set; }

		[JsonProperty("slugLuna")]
		public string SlugLuna { get; set; }
	}

	public class Thesaurus
	{
		[JsonProperty("similarity")]
		public string Similarity { get; set; }

		[JsonProperty("isInformal")]
		public string IsInformal { get; set; }

		[JsonProperty("isVulgar")]
		public object IsVulgar { get; set; }

		[JsonProperty("term")]
		public string Term { get; set; }

		[JsonProperty("targetTerm")]
		public string TargetTerm { get; set; }

		[JsonProperty("targetSlug")]
		public string TargetSlug { get; set; }

		public override string ToString()
		{
			return $"{Term} {Similarity}";
		}
	}
}