using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SparShaMap.DataService;

namespace SparShaMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("User Api");
        }

        private readonly DataBaseService _db = new DataBaseService();
        [HttpGet("SearchData")]
        public async Task<IActionResult> SearchData(int startDateIndex)
        {
            try
            {
                var result = _db.SelectQueryNoAsync("SELECT [FIRST_NAME_TH],[LAST_NAME_TH],[CUS_ID] FROM [DB_01_UAT].[dbo].[M_CUSTOMER] WHERE REC_STATUS='Y'");
                var list = result.Skip(startDateIndex).Take(5);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}