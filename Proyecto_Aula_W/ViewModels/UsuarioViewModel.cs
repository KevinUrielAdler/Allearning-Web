using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Aula_W.ViewModels
{
    public class UsuarioViewModel
    {
        public string IDSesion { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "El nombre de usuario debe tener mínimo 6 letras")]
        [MaxLength(30, ErrorMessage = "El nombre de usuario debe tener máximo 15 letras")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Introduccion")]
        public int Introduccion { get; set; }

        [Display(Name = "Datos")]
        public int Datos { get; set; }

        [Display(Name = "EstructurasL")]
        public int EstructurasL { get; set; }

        [Display(Name = "Colecciones")]
        public int Colecciones { get; set; }

        [Display(Name = "Funciones")]
        public int Funciones { get; set; }

        [Display(Name = "Ordenacion")]
        public int Ordenacion { get; set; }

        [Display(Name = "Optimizacion")]
        public int Optimizacion { get; set; }

        [Display(Name = "ColeccionesA")]
        public int ColeccionesA { get; set; }

        [Display(Name = "OptimizacionII")]
        public int OptimizacionII { get; set; }

        [Display(Name = "ColeccionesAII")]
        public int ColeccionesAII { get; set; }

        [Display(Name = "Lecc")]
        public int Lecc { get; set; }

        [Display(Name = "Resultados")]
        public string Resultados { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Display(Name = "Foto")]
        public IFormFile Foto { get; set; }
    }
}
