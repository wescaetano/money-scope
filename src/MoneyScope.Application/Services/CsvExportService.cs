using MoneyScope.Application.Interfaces;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class CsvExportService : BaseService, ICsvExportService
    {
        public CsvExportService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public byte[] Export<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Cabeçalho
            sb.AppendLine(string.Join(";", properties.Select(p => p.Name)));

            // Linhas
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item, null);
                    return Escape(value);
                });

                sb.AppendLine(string.Join(";", values));
            }

            // UTF-8 com BOM (excel-friendly)
            return Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
        }

        private static string Escape(object? value)
        {
            if (value == null)
                return string.Empty;

            var text = value.ToString()!;

            // Escapa aspas
            if (text.Contains('"'))
                text = text.Replace("\"", "\"\"");

            // Se tiver ; ou quebra de linha, envolve com aspas
            if (text.Contains(';') || text.Contains('\n') || text.Contains('\r'))
                text = $"\"{text}\"";

            return text;
        }
    }
}

