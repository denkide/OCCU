using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCApp.Interfaces;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class ProductStatusController : Controller
    {
        private readonly ILogger _logger;
        IGenericRepository _repo;
        private readonly int _pageSize = 5;
        public ProductStatusController(ILogger<ProductsController> logger, IGenericRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // GET: ProductStatus
        public ActionResult Index()
        {
            return View(LoadData());
        }

        public async Task<IActionResult> List(int? pageNum)
        {
            try
            {
                IQueryable<ProductStatus> productStatus;
                productStatus = _repo.ReturnAllAsQueryable<ProductStatus>();

                return View(await PaginatedList<ProductStatus>.CreateAsync(productStatus.AsNoTracking(), pageNum ?? 1, _pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in Product:Index", null);
                throw;
            }
        }

        private List<SimpleReportProductStatusModel> LoadData()
        {
            IQueryable<ProductStatus> productStatus;
            productStatus = _repo.ReturnAllAsQueryable<ProductStatus>();

            var pass = productStatus.Where(p => p.Status == "pass").ToList<ProductStatus>();
            var fail = productStatus.Where(p => p.Status == "fail").ToList<ProductStatus>();
            var warn = productStatus.Where(p => p.Status == "warn").ToList<ProductStatus>();

            var lstModel = new List<SimpleReportProductStatusModel>();
            lstModel.Add(new SimpleReportProductStatusModel
            {
                StatusVal = "Pass",
                Quantity = pass.Count
            });

            lstModel.Add(new SimpleReportProductStatusModel
            {
                StatusVal = "Fail",
                Quantity = fail.Count
            });

            lstModel.Add(new SimpleReportProductStatusModel
            {
                StatusVal = "Warn",
                Quantity = warn.Count
            });

            return lstModel;
        }
    }
}