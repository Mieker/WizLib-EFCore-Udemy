using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WizLib_Model.Models
{
    public class Fluent_BookAuthor
    {
        //[Key]
        public int Book_Id { get; set; }
        public Fluent_Book Fluent_Book { get; set; }
        //[Key]
        public int Author_Id { get; set; }
        public Fluent_Author Fluent_Author { get; set; }
    }
}
