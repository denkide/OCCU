using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCApp.Data;
using MVCApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MVCApp.Interfaces;
using System.Linq.Expressions;

namespace MVCApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger _logger;
        IGenericRepository _repo;

        private readonly int _pageSize = 5;

        public ProductsController(ILogger<ProductsController> logger, IGenericRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }


        // GET: Products
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNum) 
        {
            ViewData["Filter"] = searchString;
            try
            {
                if (searchString != null)
                    pageNum = 1;
                else
                    searchString = currentFilter;

                var products = _repo.ReturnAllAsQueryable<Product>();

                if (!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
                }

                return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNum ?? 1,  _pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Index", null);
                throw;
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                Expression<Func<Product, bool>> filter = o => o.ProductId == id;
                var product = await _repo.ReturnFirstOrDefault(filter);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Details", null);
                throw;
            }
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Create", null);
                throw;
            }
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,QuantityOnHand,BackorderFlag")] Product product)
        {
            try
            {
                if (await this.DoesNameExists(product))
                {
                    ModelState.AddModelError("error_msg", "This name already exists. Please use a different name.");
                    return View(product);
                }
                else
                {
                    await _repo.AddObject<Product>(product);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Create", null);
                throw;
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                Expression<Func<Product, bool>> filter = o => o.ProductId == id;
                var product = await _repo.ReturnFirstOrDefault(filter);

                if (product == null) return NotFound();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Edit", null);
                throw;
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,QuantityOnHand,BackorderFlag")] Product product)
        {
            if (id != product.ProductId) return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Func<Product, bool> filter = o => o.ProductId == product.ProductId;
                        _repo.UpdateObjectByID<Product>(product, filter);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await ProductExists(product.ProductId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Edit", null);
                throw;
            }

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)return NotFound();

            try
            {
                Expression<Func<Product, bool>> filter = o => o.ProductId == id;
                var product = await _repo.ReturnFirstOrDefault<Product>(filter);
                _repo.DeleteObj<Product>(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Delete", null);
                throw;
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Expression<Func<Product, bool>> filter = o => o.ProductId == id;
                var product = _repo.ReturnFirstOrDefault<Product>(filter).Result;
                _repo.DeleteObj<Product>(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Delete", null);
                throw;
            }
        }

        public async Task<IActionResult> Copy(int? id)
        {
            if (id == null)return NotFound();

            try
            {
                Expression<Func<Product, bool>> filter = o => o.ProductId == id;
                var product = await _repo.ReturnFirstOrDefault<Product>(filter);
                
                if (product != null)
                {
                    var copyProduct = new Product();
                    copyProduct.Name = product.Name + "(copy)";
                    copyProduct.BackorderFlag = product.BackorderFlag;
                    copyProduct.Description = product.Description;
                    copyProduct.QuantityOnHand = product.QuantityOnHand;

                    if (await this.DoesNameExists(copyProduct))
                    {
                        ModelState.AddModelError("error_msg", "This name already exists. Please use a different name.");
                        //return View(product);
                        //return RedirectToAction("Index");
                    }
                    else
                    {
                        var newProduct = await _repo.AddObject<Product>(copyProduct);
                        return RedirectToAction("Edit", new { id = newProduct.ProductId, product = newProduct });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Edit", null);
                throw;
            }
        }
        private async Task<bool> ProductExists(int id)
        {
            Expression<Func<Product, bool>> filter = o => o.ProductId == id;
            var product = await _repo.ReturnFirstOrDefault<Product>(filter);
            if (product == null) return false;
            else return true;
        }

        private async Task<bool> DoesNameExists(Product product)
        {
            Expression<Func<Product, bool>> filter = o => o.Name == product.Name;
            var foundProduct = await _repo.ReturnFirstOrDefault(filter);

            if (foundProduct == null) return false;
            else return true;
        }
    }
}

