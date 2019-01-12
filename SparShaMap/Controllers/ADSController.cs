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
    public class ADSController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("ADS Api");
        }
        private readonly DataBaseService _db = new DataBaseService();
        [HttpGet("SearchData")]
        public async Task<IActionResult> SearchData(int startDateIndex)
        {
            try
            {
                string sql = "SELECT * "
               + "FROM [M_ADS] "
               + "WHERE GETDATE()>EFF_DATE and GETDATE()<END_DATE and REC_STATUS='Y'";
                var result = _db.SelectQueryNoAsync(sql);
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