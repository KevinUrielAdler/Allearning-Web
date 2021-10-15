using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto_Aula_W.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Proyecto_Aula_W.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;


namespace Proyecto_Aula_W.Controllers
{
    public class HomeController : Controller
    {
        string ruta = "", RutaIn, param = "";
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _UserManager;
        Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> _SignInManager;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            this.context = context;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_SignInManager.IsSignedIn(User) == true)
            {
                string id = _UserManager.GetUserId(User);
                int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
                string foto= context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Foto;
                var usuario = await context.Usuarios.FindAsync(id2);
                if (usuario.Bloq == null)
                {
                    usuario.Bloq = "1";
                    context.Update(usuario);
                    await context.SaveChangesAsync();
                }
                int Bloq = int.Parse(usuario.Bloq);
                if (Bloq == 1)
                {
                    ViewBag.TBA = "Introducción a la Programación";
                    ViewBag.TPA = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Introduccion * 100 / 5;
                }
                else if (Bloq == 2)
                {
                    ViewBag.TBA = "Creación y Manejo de Datos";
                    ViewBag.TPA = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Datos * 100 / 28;
                }
                else if (Bloq == 3)
                {
                    ViewBag.TBA = "Estructuras Lógicas";
                    ViewBag.TPA = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).EstructurasL * 100 / 11;
                }
                else if (Bloq == 4)
                {
                    ViewBag.TBA = "Colecciones de Datos";
                    ViewBag.TPA = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Colecciones * 100 / 24;
                }
                //---
                ViewBag.P1 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Introduccion * 100 / 5;
                ViewBag.P2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Datos * 100 / 28;
                ViewBag.P3 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).EstructurasL * 100 / 11;
                ViewBag.P4 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Colecciones * 100 / 24;
                ViewBag.P5 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Funciones * 100 / 1;
                ViewBag.P6 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Ordenacion * 100 / 1;
                ViewBag.P7 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Optimizacion * 100 / 1;
                ViewBag.P8 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).ColeccionesA * 100 / 1;
                ViewBag.P9 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).OptimizacionII * 100 / 1;
                ViewBag.P10 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).ColeccionesAII * 100 / 1;
                return View();
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Poliforme(int Bloq,int Lecc)
        {
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            Bloq = int.Parse(usuario.Bloq);
            //Empiezan Validaciones
            ViewBag.VS = true;
            if (Bloq == 1)
            {
                ViewBag.MB = 5;
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Introduccion;
                if (Lecc < 1) Lecc = 1;
            }
            else if (Bloq == 2)
            {
                ViewBag.MB = 28;
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Datos;
                if (Lecc < 1) Lecc = 1;
            }
            else if (Bloq == 3)
            {
                ViewBag.MB = 11;
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).EstructurasL;
                if (Lecc < 1) Lecc = 1;
            }
            else if (Bloq == 4)
            {
                ViewBag.MB = 24;
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Colecciones;
                if (Lecc < 1) Lecc = 1;
            }
            ViewBag.NA = Lecc;
            //Terminan Validaciones
            RutaIn = "wwwroot/Data/Bloqu"+Bloq.ToString()+"/";
            ruta = RutaIn + "config"+Lecc.ToString()+".txt";
            ViewBag.NA = Lecc;
            Construir(Bloq);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Siguiente()
        {
            StreamReader ficheror;
            string linea;

            ficheror = System.IO.File.OpenText("wwwroot/Contr/contr.txt");
            linea = ficheror.ReadLine();
            ficheror.Close();
            StreamReader ficheror2;

            string linea2;
            ficheror2 = System.IO.File.OpenText("wwwroot/Contr/contr2.txt");
            linea2 = ficheror2.ReadLine();
            ficheror2.Close();

            if(linea==null && linea2 == null)
            {
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            int bloq = int.Parse(usuario.Bloq);
            int Lecc;
            //Empiezan Validaciones
            if (bloq == 1)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Introduccion;
                if (Lecc == 5)
                {
                    usuario.Bloq = "2";
                    usuario.Datos = 1;
                }
                else if (Lecc == 0) { usuario.Introduccion = 2; }
                else
                {
                    usuario.Introduccion = Lecc+1;
                }
            }
            else if (bloq == 2)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Datos;
                if (Lecc == 28)
                {
                    usuario.Bloq = "3";
                    usuario.EstructurasL = 1;
                }
                else if (Lecc == 0) { usuario.Datos = 2; }
                else
                {
                    usuario.Datos = Lecc + 1;
                }
            }
            else if (bloq == 3)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).EstructurasL;
                if (Lecc == 11)
                {
                    usuario.Bloq = "4";
                    usuario.Colecciones = 1;
                }
                else if (Lecc == 0) { usuario.EstructurasL = 2; }
                else
                {
                    usuario.EstructurasL = Lecc + 1;
                }
            }
            else if (bloq == 4)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Colecciones;
                if (Lecc == 24)
                {
                    usuario.Bloq = "4";
                    usuario.Colecciones = 24;
                }
                else if (Lecc == 0) { usuario.Colecciones = 2; }
                else
                {
                    usuario.Colecciones = Lecc + 1;
                }
            }
            //Terminan Validaciones
            context.Update(usuario);
            await context.SaveChangesAsync();

            }
            return RedirectToAction("Poliforme");
        }

        public async Task<IActionResult> Atras()
        {
            StreamWriter ficheror3;
            ficheror3 = System.IO.File.CreateText("wwwroot/Contr/contr2.txt");
            ficheror3.Write("");
            ficheror3.Close();

            StreamWriter ficheror4;
            ficheror4 = System.IO.File.CreateText("wwwroot/Contr/contr.txt");
            ficheror4.Write("");
            ficheror4.Close();
            //--
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            int bloq = int.Parse(usuario.Bloq);
            int Lecc;
            //Empiezan Validaciones
            if (bloq == 1)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Introduccion;
                if (Lecc == 1)
                {
                    usuario.Bloq = "1";
                    usuario.Introduccion = 1;
                }
                else
                {
                    usuario.Introduccion = Lecc - 1;
                }
            }
            else if (bloq == 2)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Datos;
                if (Lecc == 1)
                {
                    usuario.Bloq = "1";
                    usuario.Introduccion = 5;
                }
                else
                {
                    usuario.Datos = Lecc - 1;
                }
            }
            else if (bloq == 3)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).EstructurasL;
                if (Lecc == 1)
                {
                    usuario.Bloq = "2";
                    usuario.Datos = 28;
                }
                else
                {
                    usuario.EstructurasL = Lecc - 1;
                }
            }
            else if (bloq == 4)
            {
                Lecc = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Colecciones;
                if (Lecc == 1)
                {
                    usuario.Bloq = "3";
                    usuario.EstructurasL = 11;
                }
                else
                {
                    usuario.Colecciones = Lecc - 1;
                }
            }
            //Terminan Validaciones
            context.Update(usuario);
            await context.SaveChangesAsync();

            return RedirectToAction("Poliforme");
        }

        public async Task<IActionResult> Introduccion()
        {
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            usuario.Bloq = "1";
            context.Update(usuario);
            await context.SaveChangesAsync();
            return RedirectToAction("Poliforme");
        }

        public async Task<IActionResult> Datos()
        {
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            usuario.Bloq = "2";
            context.Update(usuario);
            await context.SaveChangesAsync();
            return RedirectToAction("Poliforme");
        }

        public async Task<IActionResult> EstructurasL()
        {
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            usuario.Bloq = "3";
            context.Update(usuario);
            await context.SaveChangesAsync();
            return RedirectToAction("Poliforme");
        }

        public async Task<IActionResult> Colecciones()
        {
            string id = _UserManager.GetUserId(User);
            int id2 = context.Usuarios.FirstOrDefault(p => p.IDSesion == id).Id;
            var usuario = await context.Usuarios.FindAsync(id2);
            usuario.Bloq = "4";
            context.Update(usuario);
            await context.SaveChangesAsync();
            return RedirectToAction("Poliforme");
        }

        public void Construir(int Bloque)//wwwroot/Data/Prueba.txt
        {
            try
            {
                StreamReader ficheror;
                string linea;

                ficheror = System.IO.File.OpenText("wwwroot/Contr/contr.txt");
                linea = ficheror.ReadLine();
                ficheror.Close();
                StreamReader ficheror2;

                string linea2;
                ficheror2 = System.IO.File.OpenText("wwwroot/Contr/contr2.txt");
                linea2 = ficheror2.ReadLine();
                ficheror2.Close();
                if (linea==null && linea2==null)
                {
                    ViewBag.v1 = true;
                    StreamReader fichero;
                    fichero = System.IO.File.OpenText(ruta);
                    param = fichero.ReadLine();
                    ViewBag.Titulo = param;
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        ViewBag.VCT = true;
                        param = fichero.ReadLine();
                        StreamReader fichero2;
                        string ruta2 = RutaIn + param;
                        fichero2 = System.IO.File.OpenText(ruta2);
                        ViewBag.CT = fichero2.ReadToEnd();
                        fichero2.Close();
                    }
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        param = fichero.ReadLine();
                        int param2 = int.Parse(param);
                        if (param2 > 0)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc1 = param;
                            ViewBag.VOpc1 = true;
                        }
                        if (param2 > 1)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc2 = param;
                            ViewBag.VOpc2 = true;
                        }
                        if (param2 > 2)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc3 = param;
                            ViewBag.VOpc3 = true;
                        }
                        if (param2 > 3)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc4 = param;
                            ViewBag.VOpc4 = true;
                        }
                        if (param2 > 4)
                        {
                            ViewBag.VOpc1 = false;
                            ViewBag.VOpc2 = false;
                            ViewBag.VOpc3 = false;
                            ViewBag.VOpc4 = false;
                        }
                        param = fichero.ReadLine();
                        StreamWriter ficheror3;
                        ficheror3 = System.IO.File.CreateText("wwwroot/Contr/contr2.txt");
                        ficheror3.Write(param);
                        ficheror3.Close();
                    }
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        ViewBag.VET = true;
                        param = fichero.ReadLine();
                        StreamWriter ficheror4;
                        ficheror4 = System.IO.File.CreateText("wwwroot/Contr/contr.txt");
                        ficheror4.Write(param);
                        ficheror4.Close();
                    }
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        param = fichero.ReadLine();
                        string ruta2 = "/Data/Bloqu" + Bloque.ToString() + "/" + param;
                        ViewBag.VI = true;
                        ViewBag.I = ruta2;
                    }
                    fichero.Close();
                }
                else
                {
                    ViewBag.v1 = false;
                    StreamReader fichero;
                    fichero = System.IO.File.OpenText(ruta);
                    param = fichero.ReadLine();
                    ViewBag.Titulo = param;
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        ViewBag.VCT = true;
                        param = fichero.ReadLine();
                        StreamReader fichero2;
                        string ruta2 = RutaIn + param;
                        fichero2 = System.IO.File.OpenText(ruta2);
                        ViewBag.CT = fichero2.ReadToEnd();
                        fichero2.Close();
                    }
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        param = fichero.ReadLine();
                        int param2 = int.Parse(param);
                        if (param2 > 0)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc1 = param;
                            ViewBag.VOpc1 = true;
                        }
                        if (param2 > 1)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc2 = param;
                            ViewBag.VOpc2 = true;
                        }
                        if (param2 > 2)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc3 = param;
                            ViewBag.VOpc3 = true;
                        }
                        if (param2 > 3)
                        {
                            param = fichero.ReadLine();
                            ViewBag.Opc4 = param;
                            ViewBag.VOpc4 = true;
                        }
                        if (param2 > 4)
                        {
                            ViewBag.VOpc1 = false;
                            ViewBag.VOpc2 = false;
                            ViewBag.VOpc3 = false;
                            ViewBag.VOpc4 = false;
                        }
                        param = fichero.ReadLine();
                        ViewBag.contr2 = param;
                        StreamWriter ficheror3;
                        ficheror3 = System.IO.File.CreateText("wwwroot/Contr/contr2.txt");
                        ficheror3.Write("");
                        ficheror3.Close();
                    }
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        ViewBag.VET = true;
                        param = fichero.ReadLine();
                        ViewBag.contr = param;
                        StreamWriter ficheror4;
                        ficheror4 = System.IO.File.CreateText("wwwroot/Contr/contr.txt");
                        ficheror4.Write("");
                        ficheror4.Close();
                    }
                    param = fichero.ReadLine();
                    if (param[0] == 't')
                    {
                        param = fichero.ReadLine();
                        string ruta2 = "/Data/Bloqu" + Bloque.ToString() + "/" + param;
                        ViewBag.VI = true;
                        ViewBag.I = ruta2;
                    }
                    fichero.Close();
                }
            }
            catch
            {
                ViewBag.Titulo= "Error al intentar abrir el archivo " + ruta;
            }
        }
    }
}
