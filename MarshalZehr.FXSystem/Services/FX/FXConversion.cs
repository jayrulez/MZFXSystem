﻿using System;

namespace MarshalZehr.FXSystem.Services.FX
{
    public class FXConversion
    {
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Value { get; set; }
        public string Date { get; set; }

        public bool Direct { get; set; }

        public override string ToString()
        {
            return $"{Amount} {SourceCurrencyCode} = {Math.Round(Value, 4)} {TargetCurrencyCode} using Exchange Rate = {ExchangeRate} on date '{Date}'.";
        }
    }
}
