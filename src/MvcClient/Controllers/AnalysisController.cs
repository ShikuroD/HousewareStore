using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MvcClient.Authorization;
using MvcClient.Models;
using MvcClient.Services;
using MvcClient.ViewModels;
namespace MvcClient.Controllers{
    public class AnalysisController : Controller{
        private readonly ILogger<AdminController> _logger;
        private readonly IItemService _itemService;
        public AnalysisController(ILogger<AdminController> logger, IItemService itemService){
            _logger = logger;
            _itemService = itemService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}