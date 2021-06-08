using RestSharp;
using SimpleCore.Utilities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Novus.Win32;

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
			//Console.OutputEncoding = Encoding.Unicode;
			//Console.InputEncoding  = Encoding.Unicode;


			Native.SetConsoleOutputCP(Native.Win32UnicodeCP);
			
			if (args.Any()) {
				Show(args.First());

			}
		}


		private static string CallPy(string args)
		{
			var startInfo = new ProcessStartInfo("python")
			{
				Arguments              = args,
				UseShellExecute        = false,
				RedirectStandardOutput = true,
				StandardOutputEncoding = Native.Win32Unicode
			};

			using var process = Process.Start(startInfo);

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
					Console.WriteLine($"{Strings.Indent}{synonym}");
				}
			}
		}

		private static void Translate(string src, string lang)
		{
			//print(Language('la').name)


			string t2x = CallPy("-c \"from translatepy import * " + Environment.NewLine +
			                    $"print(Language('{lang}').name)\"").Trim();


			Console.WriteLine($"[{t2x}]");

			string t1 = CallPy("-c \"from googletrans import * " + Environment.NewLine +
			                   $"print(Translator().translate('{src}', dest='{lang}').text)\"").Trim();

			string t1x = CallPy("-c \"from googletrans import * " + Environment.NewLine +
			                    $"print(Translator().translate('{src}', dest='{lang}').pronunciation)\"").Trim();

			
			Console.WriteLine($"{Strings.Indent}[#1] | {t1} ({t1x})");

			string t2 = CallPy("-c \"from translatepy import * " + Environment.NewLine +
			                   $"print(Translator().translate('{src}', '{lang}'))\"").Trim();

			Console.WriteLine($"{Strings.Indent}[#2] | {t2}");
		}
	}
}