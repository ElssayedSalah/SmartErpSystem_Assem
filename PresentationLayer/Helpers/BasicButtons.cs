using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Helpers
{
    public class BasicButtons
    {
        public string CreateNewBtnControler { get; set; }
        public string CreateNewBtnAction { get; set; }
        public string BackToListControler { get; set; }
        public string BackToListAction { get; set; }
        public string EditBtnControler { get; set; }
        public string EditBtnAction { get; set; }
        public string DeleteBtnControler { get; set; }
        public string DeleteBtnAction { get; set; }
        public string PrintBtnAction { get; set; }
        public string PrintBtnControler { get; set; }
        public string SendEInvoiceBtnAction { get; set; }
        public string SendEInvoiceBtnControler { get; set; }

        public bool CreateNewBtnVisibilty { get; set; }
        public bool BackToListBtnVisibilty { get; set; }
        public bool PrintBtnVisibilty { get; set; }
        public bool EditBtnVisibilty { get; set; }
        public bool SaveBtnVisibilty { get; set; }
        public bool DeleteBtnVisibilty { get; set; }
        public bool SendEInvoiceBtnVisibilty { get; set; }
        public int RouteId { get; set; }
        public string StringRouteId { get; set; }
        public string DocumentType { get; set; }

    }
}
