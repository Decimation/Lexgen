using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Novus.Utilities;
using Novus.Win32;
using RestSharp;
using SimpleCore.Utilities;

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


			var rc = new RestClient("https://tuna.thesaurus.com/pageData/{word}");

			var req = new RestRequest(Method.GET);
			req.AddUrlSegment("word", "root");

			var res = rc.Execute<Root>(req);

			var data = res.Data;


			foreach (var definition in data.Data.DefinitionData.Definitions) {
				Console.WriteLine($"{definition.Definition} {definition.Pos}");

				foreach (var synonym in definition.Synonyms) {
					Console.WriteLine($"{Strings.Indent}{synonym}");
				}
			}

			Console.OutputEncoding = Encoding.Unicode;
			Console.InputEncoding  = Encoding.Unicode;

			Console.WriteLine(get("word", "la"));
			Console.WriteLine(get2("word", "la"));
		}

		static string get2(string a, string b)
		{
			return py("-c \"from googletrans import * " + Environment.NewLine +
			          $"print(Translator().translate('{a}', dest='{b}'))\"");

		}


		static string py(string args)
		{
			var startInfo = new ProcessStartInfo("python")
			{
				Arguments              = args,
				UseShellExecute        = false,
				RedirectStandardOutput = true
			};


			using var process = Process.Start(startInfo);

			using var reader = process.StandardOutput;

			string result = reader.ReadToEnd();

			return result;
		}

		static string get(string a, string b)
		{

			return py("-c \"from translatepy import * " + Environment.NewLine +
			          $"print(Translator().translate('{a}', '{b}'))\"");

		}
	}

	
}