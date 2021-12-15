using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace DronCities.Assets
{
	class Parse
	{
		static public List<string> ParseCVSFile(TextFieldParser parser)
		{
			List<string> result = new List<string>();
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			int i = 0;
			while (!parser.EndOfData)
			{

				string[] fields = parser.ReadFields();
				foreach (string field in fields)
				{
					result.Add(field);
					i++;
				}
			}
			
			return result;
		}
	}
}
