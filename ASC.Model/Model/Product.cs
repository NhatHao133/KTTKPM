using ASC.Model.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Model.Model
{
	public class Product
	{
		[Key] 
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProducId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }


	}
}
