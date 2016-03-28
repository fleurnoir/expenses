using System;
using Expenses.BL.Entities;
using Expenses.Common.Service;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class StatsItemViewData : StatsItem
    {
        public StatsItemViewData(){
        }

        public StatsItemViewData(StatsItem stats){
            Cloner.Clone (stats, this);
        }

        public string CurrencyName { get; set; }
    }
}

