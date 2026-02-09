using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Myper.Trabajadores.Web.Data;
using Myper.Trabajadores.Web.Models;

namespace Myper.Trabajadores.Web.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly TrabajadoresContext _context;

        public TrabajadoresController(TrabajadoresContext context)
        {
            _context = context;
        }

        // GET: Trabajadores
        public async Task<IActionResult> Index(string? sexo)
        {
            // Obtenemos la lista desde el procedimiento almacenado
            var trabajadores = await _context.Trabajadores
                .FromSqlRaw("EXEC sp_ListarTrabajadores")
                .ToListAsync();

            // Filtro opcional por sexo (M/F)
            if (!string.IsNullOrEmpty(sexo))
            {
                sexo = sexo.ToUpper();
                if (sexo == "M" || sexo == "F")
                {
                    trabajadores = trabajadores
                        .Where(t => t.Sexo == sexo[0])
                        .ToList();
                }
            }

            ViewBag.SexoActual = sexo;
            return View(trabajadores);
        }

        // GET: Trabajadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trabajadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trabajador trabajador, IFormFile? fotoArchivo)
        {
            if (!ModelState.IsValid)
                return View(trabajador);

            if (fotoArchivo != null && fotoArchivo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(fotoArchivo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fotoArchivo.CopyToAsync(stream);
                }

                trabajador.Foto = "/images/" + fileName;
            }

            trabajador.FechaRegistro = DateTime.Now;
            trabajador.Activo = true;

            _context.Add(trabajador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Trabajadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador == null || !trabajador.Activo) return NotFound();

            return View(trabajador);
        }

        // POST: Trabajadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trabajador trabajador, IFormFile? fotoArchivo)
        {
            if (id != trabajador.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(trabajador);

            var existente = await _context.Trabajadores.FindAsync(id);
            if (existente == null || !existente.Activo) return NotFound();

            existente.Nombres = trabajador.Nombres;
            existente.Apellidos = trabajador.Apellidos;
            existente.TipoDocumento = trabajador.TipoDocumento;
            existente.NumeroDocumento = trabajador.NumeroDocumento;
            existente.Sexo = trabajador.Sexo;
            existente.FechaNacimiento = trabajador.FechaNacimiento;
            existente.Direccion = trabajador.Direccion;

            if (fotoArchivo != null && fotoArchivo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(fotoArchivo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fotoArchivo.CopyToAsync(stream);
                }

                existente.Foto = "/images/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Trabajadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo);
            if (trabajador == null) return NotFound();

            return View(trabajador);
        }

        // POST: Trabajadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador != null)
            {
                trabajador.Activo = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}