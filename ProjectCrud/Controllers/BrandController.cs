using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectCrud.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _dbContext;
        private readonly ILogger<BrandController> _logger;

        // Constructeur qui reçoit un objet BrandContext injecté par l'injection de dépendances
        public BrandController(BrandContext dbContext, ILogger<BrandController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // Action HTTP GET pour récupérer tous les enregistrements de la table Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            // Vérifier si la table Brands est vide
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }

            // Retourner tous les enregistrements de la table Brands
            return await _dbContext.Brands.ToListAsync();
        }

        // Action HTTP GET pour récupérer un enregistrement de la table Brands par ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrands(int id)
        {
            // Vérifier si la table Brands est vide
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }

            // Rechercher l'enregistrement par ID
            var brand = await _dbContext.Brands.FindAsync(id);

            // Vérifier si l'enregistrement existe
            if (brand == null)
            {
                return NotFound();
            }

            // Retourner l'enregistrement trouvé
            return brand;
        }

        // Action HTTP POST pour ajouter un nouvel enregistrement dans la table Brands
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            try
            {
                // Ajouter le nouvel enregistrement
                _dbContext.Brands.Add(brand);

                // Enregistrer les modifications dans la base de données
                await _dbContext.SaveChangesAsync();

                // Retourner la réponse avec le statut 201 (Created) et l'URI de la nouvelle ressource
                return CreatedAtAction(nameof(GetBrands), new { id = brand.id }, brand);
            }
            catch (Exception ex)
            {
                // Log des détails de l'exception pour débogage
                _logger.LogError(ex, "Erreur lors de l'ajout d'un objet Brand");
                // Retourner une réponse d'erreur appropriée (par exemple, statut 500)
                return StatusCode(500, "Une erreur interne s'est produite.");
            }
        }


        // Action HTTP PUT pour mettre à jour un enregistrement dans la table Brands
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            // Vérifier si l'ID dans le chemin d'accès correspond à l'ID de l'objet Brand
            if (id != brand.id)
            {
                return BadRequest();
            }

            // Marquer l'objet comme modifié dans le contexte de base de données
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                // Tenter de sauvegarder les modifications
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gérer les erreurs de concurrence (si l'enregistrement a été modifié par quelqu'un d'autre)
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retourner le statut 200 (OK) si la mise à jour a réussi
            return Ok();
        }

        // Méthode privée pour vérifier si un enregistrement avec l'ID donné existe dans la table Brands
        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.id == id)).GetValueOrDefault();
        }

        // Action HTTP DELETE pour supprimer un enregistrement dans la table Brands par ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            // Vérifier si la table Brands est vide
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }

            // Rechercher l'enregistrement par ID
            var brand = await _dbContext.Brands.FindAsync(id);

            // Vérifier si l'enregistrement existe
            if (brand == null)
            {
                return NotFound();
            }

            // Supprimer l'enregistrement
            _dbContext.Brands.Remove(brand);

            // Enregistrer les modifications dans la base de données
            await _dbContext.SaveChangesAsync();

            // Retourner le statut 200 (OK) si la suppression a réussi
            return Ok();
        }
    }
}
