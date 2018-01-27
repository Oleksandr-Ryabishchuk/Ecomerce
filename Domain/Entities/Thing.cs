using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Thing
    {
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "ID")]
        public int ThingId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название товара")]
        public string Name { get; set; }
        

        [Display(Name = "Вид")]
        [Required(ErrorMessage = "Пожалуйста, введите название вида товара")]
        public string Kind { get; set; }

        [Display(Name = "Материал")]
        [Required(ErrorMessage = "Пожалуйста, введите название материала из которого изготовлена вещь")]
        public string Material { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, опишите товар")]
        public string Description { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Пожалуйста, назовите страну-производитель")]
        public string Producer { get; set; }

        [Display(Name = "Артикул")]
        [Required(ErrorMessage = "Пожалуйста, введите артикул товара")]
        public string Articul { get; set; }

        [Display(Name = "Цена (грн)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите цену товара")]
        public decimal Price { get; set; }

    }
}
