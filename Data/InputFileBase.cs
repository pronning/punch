using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Punch.Models;
using Punch.Utils;

namespace Punch.Data
{
    public class InputFileBase
    {
        public Stream FileStream { get; set; }
        public int DatePos { get; set; }
        public int ReferencePos { get; set; }
        public int DescPos { get; set; }
        public int AmountPos{ get; set; }
        public bool HasHeaderRow{ get; set; }
        public string SourceName{ get; set; }

        public List<ExpenseModel> Parse(string identity)
        {
            if (FileStream==null)
                throw new NullReferenceException("ingen fil funnet");

            var expenseList = new List<ExpenseModel>();
            
            var streamReader = new StreamReader(FileStream, Encoding.Default);
            if (streamReader.BaseStream.Length == 0)
                return expenseList;
           
            if( HasHeaderRow)
                streamReader.ReadLine();

            while( streamReader.Peek() >= 0 )
            {
                var sb = streamReader.ReadLine();

                var fields = Split(sb);
                if( fields.Count == 0)
                    continue;
                
                DateTime date;
                if (!DateTime.TryParse(fields[DatePos],out date))
                    continue;

                string description = fields[DescPos];

                double amount;
                if (!double.TryParse(fields[AmountPos], out amount))
                    amount = 0;

                // a.k.a. no expense
                if (amount == 0)
                    continue;


                var em = new ExpenseModel
                {
                    Date = date,
                    Description = description,
                    Owner = identity,
                    Amount = amount,
                    Means = SourceName,
                    IsCommon = true
                };

                expenseList.Add(em);
            }

            return expenseList;
        }

        private static List<string> Split(string p)
        {
            var separators = new[] {'\t', ';', ','};
            foreach (var separator in separators)
            {
                var list = p.Split(separator).ToList().Clean();
                string firstOrDefault = list.FirstOrDefault();
                bool isDate = firstOrDefault.IsDate();
                if (isDate)
                    return list.ToList();
            }
            return new List<string>();
        }
    }
}