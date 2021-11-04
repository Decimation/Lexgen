using RestSharp;
using SimpleCore.Utilities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json.Linq;
using Novus.Win32;

// ReSharper disable PossibleNullReferenceException

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace Lexgen
{
	public static class Program
	{
		private static void Main(string[] args)
		{
#if DEBUG
			if (!args.Any()) {
				args = new[] { "hello" };
			}
#endif

			//Console.OutputEncoding = Encoding.Unicode;
			//Console.InputEncoding  = Encoding.Unicode;

			//if (args.Length==0) {
			//	args = new[] {"food"};
			//}

			// Native.SetConsoleOutputCP(Native.CP_UTF8);

			if (args.Any()) {
				Show(args.First());
			}
		}


		private static string CallPy(params string[] args)
		{
			var process = Command.Py(args);
			process.Start();
			using var reader = process!.StandardOutput;

			string result = reader.ReadToEnd();

			return result;
		}

		private static string CallShell(string s)
		{
			var process = Command.Shell(s);
			process.Start();
			using var reader = process!.StandardOutput;

			string result = reader.ReadToEnd();

			return result;
		}

		private static void Show(string s)
		{
			string value = new('-', 20);

			Console.WriteLine("[Thesaurus]");
			Thesaurus(s);
			Console.WriteLine(value);

			Translate(s, "la");
			Console.WriteLine(value);

			Translate(s, "el");
			Console.WriteLine(value);

			Translate(s, "fr");
			Console.WriteLine(value);

			Translate(s, "es");
			Console.WriteLine(value);

			Translate(s, "ja");
		}

		private static void Thesaurus(string s)
		{
			var rc = new RestClient("https://tuna.thesaurus.com/pageData/{word}");

			var req = new RestRequest(Method.GET);
			req.AddUrlSegment("word", s);

			var res = rc.Execute<Root>(req);
			//var data=JsonConvert.DeserializeObject<Root>(res.Content);

			var data = res.Data;

			var dataDefinitions = data.Data.DefinitionData.Definitions;

			foreach (var definition in dataDefinitions) {
				Console.WriteLine($"{definition.Definition} ({definition.Pos})");

				var synonyms = definition.Synonyms.Take(10);

				foreach (var synonym in synonyms) {
					Console.WriteLine($"{Strings.Indentation}{synonym}");
				}
			}
		}

		private static void Translate(string src, string lang)
		{
			//print(Language('la').name)


			/*string t2x = CallPy("from translatepy import * ",
			                    $"print(Language('{lang}').name)").Trim();*/

			var srcl = "en";

			/*
			var l1  = CallShell($"translatepy language -t {lang}");
			var l1j = JObject.Parse(l1);

			var s1 = l1j["result"].ToString();
			var s2 = l1j["service"].ToString();

			Console.WriteLine($"[{s1}] ({s2})");*/

			Console.WriteLine($"[{lang}]");


			string t1 = CallPy("from googletrans import * ",
			                   $"print(Translator().translate('{src}', dest='{lang}').text)").Trim();

			string t1x = CallPy("from googletrans import * ",
			                    $"print(Translator().translate('{src}', dest='{lang}').pronunciation)").Trim();


			Console.WriteLine($"{Strings.Indentation}[#1] | {t1} ({t1x})");


			var j = JObject.Parse(CallShell($"translatepy translate -t {src} -d {lang} -s {srcl}"));

			string t2 = null;
			string s  = null;

			if (bool.Parse(j["success"].ToString())) {
				t2 = j["result"].ToString();
				s  = j["service"].ToString();
			}


			Console.WriteLine($"{Strings.Indentation}[#2] | {t2} ({s})");
		}
	}
}