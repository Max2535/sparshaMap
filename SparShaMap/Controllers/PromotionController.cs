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
    public class PromotionController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Promotion Api");
        }

        private readonly DataBaseService _db = new DataBaseService();
        [HttpGet("SearchData")]
        public async Task<IActionResult> SearchData(int startDateIndex)
        {
            try
            {
                string sql= "SELECT [PACKAGE_NAME],[DATA_AREA],[EFF_DATE],[END_DATE] "
                + "FROM [DB_01_UAT].[dbo].[M_PROMOTION] "
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