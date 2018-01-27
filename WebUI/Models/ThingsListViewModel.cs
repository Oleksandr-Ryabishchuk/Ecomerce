using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ThingsListViewModel
    {
        public IEnumerable<Thing> Things { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentKind { get; set; }
    }
}