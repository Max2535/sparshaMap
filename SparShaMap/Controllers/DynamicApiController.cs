using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SparShaMap.DataService;

namespace SparShaMap.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicApiController : ControllerBase
    {
        class _Response
        {
            public int code { get; set; }
            public string msg { get; set; }
        }
        class _File
        {
            public int code { get; set; }
            public string name { get; set; }
            public string path { get; set; }
        }
        private IHostingEnvironment _hostingEnvironment;

        public DynamicApiController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        private readonly DataBaseService _db = new DataBaseService();
        [AllowAnonymous] //กรณีที่ไม่ต้องผ่าน Auth
        //api/dynamicapi/{table}
        /*
        Service ตัวนี้จะแสดงทุก Table ใน Database
         */
        [HttpGet]
        public async Task<IActionResult> AllTable()
        {
            try
            {
                var result = _db.GetAllTable();
                if (result.Result.Count <= 0)
                {
                    return Ok(new _Response
                    {
                        code = 500,
                        msg = "ไม่พบข้อมูล"
                    });
                }
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous] //กรณีที่ไม่ต้องผ่าน Auth
        /*
        Service ตัวนี้ะแสดง ข้อมูลของ Table ที่ระบุพร้อม offset
         */
        [HttpPost("{table}/{start}/{length}")]
        public async Task<IActionResult> GetAll(string table, string start, string length)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถค้นหาข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            string column_pk = await _db.GetPrimaryKey(table);
            if (column_pk == null || column_pk == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถค้นหาข้อมูลได้ เนื่องจากไม่พบ COLUMN PRIMARY KEY กรุณาระบุ COLUMN PRIMARY KEY"
                });
            }
            try
            {
                //กรณีที่มี perfix
                var result = _db.SelectQuery("SELECT * FROM " + db_name + "." + table + " ORDER BY " + column_pk + " ASC OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY");
                if (result == null || result.Result.Count <= 0)
                {
                    return Ok(new _Response
                    {
                        code = 500,
                        msg = "ไม่พบข้อมูล"
                    });
                }
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                try
                {
                    //กรณีที่ไม่มี perfix
                    var result = _db.SelectQuery("SELECT * FROM " + table + " ORDER BY " + column_pk + " ASC OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY");
                    if (result == null || result.Result.Count <= 0)
                    {
                        return Ok(new _Response
                        {
                            code = 500,
                            msg = "ไม่พบข้อมูล"
                        });
                    }
                    return Ok(result.Result);
                }
                catch (Exception er)
                {
                    return Ok(new _Response
                    {
                        code = 500,
                        msg = ex.Message + "&&" + er.Message
                    });
                }
            }
        }
        [AllowAnonymous] //กรณีที่ไม่ต้องผ่าน Auth
        /*
        
        Service  ตัวจะแสดงข้อมูล row ตาม id ที่ส่งมา
         */
        [HttpGet("{table}/{id}")]
        public async Task<IActionResult> Find(string table, string id)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถค้นหาข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            string column_pk = await _db.GetPrimaryKey(table);
            if (column_pk == null || column_pk == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถค้นหาข้อมูลได้ เนื่องจากไม่พบ COLUMN PRIMARY KEY กรุณาระบุ COLUMN PRIMARY KEY"
                });
            }
            try
            {
                var result = SearchData(table, id, db_name, column_pk);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new _Response
                    {
                        code = 500,
                        msg = "ไม่พบข้อมูล"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous] //กรณีที่ไม่ต้องผ่าน Auth
        /*
        กรณีที่ไม่ได้สร้าง COLUMN PRIMARY KEY
        Service  ตัวจะแสดงข้อมูล row ตาม id ที่ส่งมา
         */
        [HttpGet("{table}/{pk}/{id}")]
        public async Task<IActionResult> FindForPK(string table, string pk, string id)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถค้นหาข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            try
            {
                var result = SearchData(table, id, db_name, pk);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new _Response
                    {
                        code = 500,
                        msg = "ไม่พบข้อมูล"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }

        private List<Dictionary<string, object>> SearchData(string table, string id, string db_name, string column_pk)
        {
            try
            {
                var result = _db.SelectQueryNoAsync("SELECT * FROM " + db_name + "." + table + " WHERE " + column_pk + "='" + id + "'");
                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    //กรณีที่ไม่มี prefix
                    var result = _db.SelectQueryNoAsync("SELECT * FROM " + table + " WHERE " + column_pk + "='" + id + "'");
                    return result;
                }
                catch (Exception er)
                {
                    throw new ArgumentException(er.Message);
                }
            }
        }
        [AllowAnonymous] //กรณีที่ไม่ต้องผ่าน Auth
        //ส่งเป็น Object json
        [HttpPost("savejson/{table}")]
        public async Task<IActionResult> SaveJson(string table, object json)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถเพิ่มข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            string column_pk = await _db.GetPrimaryKey(table);
            if (column_pk == null || column_pk == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถเพิ่มข้อมูลได้ เนื่องจากไม่ได้กำหนด COLUMN PRIMARY KEY กรุณาระบุ COLUMN PRIMARY KEY"
                });
            }
            return Ok(InsertSql(table, db_name, column_pk, json));
        }

        [AllowAnonymous] //กรณีที่ไม่ต้องผ่าน Auth
        /*กรณีที่ไม่ได้สร้าง COLUMN PRIMARY KEY
        ส่งเป็น Object json*/
        [HttpPost("savejson/{table}/{pk}")]
        public async Task<IActionResult> SaveJsonForPk(string table, string pk, object json)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถเพิ่มข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            return Ok(InsertSql(table, db_name, pk, json));
        }
        private object InsertSql(string table, string db_name, string column_pk, object json)
        {
            string sql = "";
            List<string> result = null;//สร้างเพื่อรับ ID ที่ INSERT ไป
            try
            {
                JArray dataArrayOject = (JArray)json;
                List<string> key = new List<string>();
                List<object> value = new List<object>();
                foreach (var ob in dataArrayOject.Children<JObject>())
                {
                    foreach (JProperty property in ob.Properties())
                    {
                        key.Add(property.Name);
                        value.Add(property.Value);
                    }
                    var col = String.Join(", ", key.ToArray());
                    var val = String.Join("','", value.ToArray());
                    sql += "INSERT INTO " + db_name + "." + table + " (" + col + ") output INSERTED." + column_pk + " VALUES ('" + val + "');";
                    key.Clear();
                    value.Clear();
                }
                //กรณีที่มี prefix
                string Datesql = sql.Replace("'GETDATE()'", "GETDATE()");//กรณี Insert Date
                result = _db.InsertData(Datesql);
            }
            catch (Exception ex)
            {
                try
                {
                    //กรณีที่ไม่มี prefix
                    string outputSql = sql.Replace(db_name + ".", "");
                    outputSql = outputSql.Replace("'GETDATE()'", "GETDATE()");//กรณี Insert Date
                    result = _db.InsertData(outputSql);
                }
                catch (Exception er)
                {
                    return new _Response
                    {
                        code = 500,
                        msg = "ไม่สามารถบันทึกข้อมูลได้ กรุณาลองใหม่อีกครั้ง (" + er.Message + ")"
                    };
                }
            }
            //นำข้อมูลมาแสดง
            try
            {
                if (result.Count > 0)
                {
                    var obj = new List<object>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        obj.AddRange(SearchData(table, result[i], db_name, column_pk));
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                return new _Response
                {
                    code = 500,
                    msg = ex.Message
                };
            }
            //กรณีอื่นๆที่คาดไม่ถึง
            return new _Response
            {
                code = 500,
                msg = "ไม่สามารถบันทึกข้อมูลได้ กรุณาลองใหม่อีกครั้ง"
            };
        }
        //TODO:: update List object
        [AllowAnonymous]//กรณีที่ไม่ต้อง Auth
        [HttpPut("updatejson/{table}/{id}")]
        public async Task<IActionResult> UpdateJson(string table, string id, object json)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถแก้ไขข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            string column_pk = await _db.GetPrimaryKey(table);
            if (column_pk == null || column_pk == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถแก้ไขข้อมูลได้ เนื่องจากไม่พบ COLUMN PRIMARY KEY กรุณาระบุ COLUMN PRIMARY KEY"
                });
            }
            return Ok(await UpdateSql(db_name, table, column_pk, id, json));
        }
        [AllowAnonymous]//กรณีที่ไม่ต้อง Auth
        //กรณีได้ได้สร้าง COLUMN PRIMARY KEY
        [HttpPut("updatejson/{table}/{pk}/{id}")]
        public async Task<IActionResult> UpdateJsonForPk(string table, string pk, string id, object json)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถแก้ไขข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            return Ok(await UpdateSql(db_name, table, pk, id, json));
        }
        private async Task<object> UpdateSql(string db_name, string table, string column_pk, string id, object json)
        {
            string sql = "";
            try
            {
                JArray dataArrayOject = (JArray)json;
                List<string> set = new List<string>();
                foreach (var ob in dataArrayOject.Children<JObject>())
                {
                    foreach (JProperty property in ob.Properties())
                    {
                        set.Add(property.Name + "='" + property.Value + "'");
                    }
                    var setValue = String.Join(",", set.ToArray());
                    sql += "UPDATE " + db_name + "." + table + " SET " + setValue + " WHERE " + column_pk + "='" + id + "';";
                    set.Clear();
                }
                string Datesql = sql.Replace("'GETDATE()'", "GETDATE()");//กรณี Update Date
                var result = await _db.QueryData(Datesql);
                return SearchData(table, id, db_name, column_pk);
            }
            catch (Exception ex)
            {
                try
                {
                    //กรณีที่ไม่มี prefix
                    string outputSql = sql.Replace(db_name + ".", "");
                    outputSql = outputSql.Replace("'GETDATE()'", "GETDATE()");//กรณี Update Date
                    var result = await _db.QueryData(outputSql);
                    return SearchData(table, id, db_name, column_pk);
                }
                catch (Exception er)
                {
                    return new _Response
                    {
                        code = 500,
                        msg = "ไม่สามารถแก้ไขข้อมูลได้ กรุณาลองใหม่อีกครั้ง (" + er.Message + ")"
                    };
                }
            }
            //กรณีอื่นๆที่คาดไม่ถึง
            return new _Response
            {
                code = 500,
                msg = "ไม่สามารถแก้ไขข้อมูลได้ กรุณาลองใหม่อีกครั้ง"
            };
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpDelete("delete/{table}/{id}")]
        public async Task<IActionResult> Delete(string table, string id)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถลบข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            string column_pk = await _db.GetPrimaryKey(table);
            if (column_pk == null || column_pk == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถลบข้อมูลได้ เนื่องจากไม่พบ COLUMN PRIMARY KEY กรุณาระบุ COLUMN PRIMARY KEY"
                });
            }
            return Ok(await DeleteSql(db_name, table, column_pk, id));
        }

        [AllowAnonymous]//ไม่ต้อง login
        [HttpDelete("delete/{table}/{pk}/{id}")]
        public async Task<IActionResult> Delete(string table, string pk, string id)
        {
            var db_name = await _db.GetDataBaseName();
            if (db_name == null || db_name == "")
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = "ไม่สามารถลบข้อมูลได้ เนื่องจากไม่พบ ฐานข้อมูล"
                });
            }
            return Ok(await DeleteSql(db_name, table, pk, id));
        }
        private async Task<object> DeleteSql(string db_name, string table, string column_pk, string id)
        {
            string sql = "";
            try
            {
                //ค้นหาข้อมูลก่อนลบ
                try
                {
                    var search = SearchData(table, id, db_name, column_pk);
                }
                catch (Exception ex)
                {
                    return new _Response
                    {
                        code = 500,
                        msg = "ไม่พบข้อมูลที่ต้องการลบ"
                    };
                }
                //กรณีที่มี prefix
                sql = "DELETE FROM " + db_name + ". \"" + table + "\" WHERE  \"" + column_pk + "\"='" + id + "';";
                var result = await _db.QueryData(sql);
            }
            catch (Exception ex)
            {
                try
                {
                    //กรณีที่ไม่มี prefix
                    sql = "DELETE FROM \"" + table + "\" WHERE  \"" + column_pk + "\"='" + id + "';";
                    var result = await _db.QueryData(sql);
                }
                catch (Exception er)
                {
                    return new _Response
                    {
                        code = 500,
                        msg = "ไม่สามารถลบข้อมูลได้ กรุณาลองใหม่อีกครั้ง (" + er.Message + ")"
                    };
                }
            }
            return Ok(new _Response
            {
                code = 200,
                msg = "ลบข้อมูลสำเร็จ"
            });
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var result = new List<_File>();
                foreach (var file in files)
                {
                    string newPath = Path.Combine(webRootPath, file.FileName);//TO srever
                    //string newPath = Path.Combine(_db.getPath(), folderName);//TO config path
                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    result.Add(new _File
                    {
                        code = 200,
                        name = file.FileName,
                        path = newPath
                    });//เพิ่มไฟล์
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpPost("uploadFile"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                //var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}")
                var location = new Uri($"{Request.Scheme}://{Request.Host}");
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                //string newPath = Path.Combine(webRootPath, folderName);//TO srever
                string newPath = Path.Combine(_db.getPath(), folderName);//TO config path
                string fullPath = "";
                string fileName = "";
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return Ok(new _File
                {
                    code = 200,
                    name = fileName,
                    path = location.ToString() + "api/dynamicapi/getFile/" + fileName
                });
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpGet("getFile/{filename}")]
        public async Task<IActionResult> getFile(string filename)
        {
            try
            {
                if (filename == null)
                {
                    return Ok(new _Response
                    {
                        code = 500,
                        msg = "filename not present"
                    });
                }
                string folderName = "Upload";
                var path = Path.Combine(_db.getPath(), folderName, filename);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        /*
        CONTRACT_SUB_TYPE CONTRACT_TYPE
             MONEY            TM       (จำนวนเงิน)
             QTY              PD       (สินค้า)
             QTY              TM       (Treatment และ Pack)
         */
        [AllowAnonymous]//ไม่ต้อง login
        [HttpGet("getcontractproduct/{id_cus}")]
        public async Task<IActionResult> getContractProduct(string id_cus)
        {
            string sql = "SELECT "
            + "UID_CUSTOMER,CONTRACT_NO,SELLING_DATE,EXPIRE_DATE,DATA_AREA, "
            + "t2.PRODUCT_NAME,PRODUCT_FULL_NAME,"
            + "t3.UID_PROMOTION "
            + "FROM [T_CONTRACT_HEADER] t1 left outer join [T_CONTRACT_SELLING] t2 "
            + "on t1.UID=t2.UID_CONTRACT_HEADER "
            + "left outer join [T_CONTRACT_SELLING_PROMOTION] t3 "
            + "on t1.UID=t3.UID_CONTRACT_H "
            + "where CONTRACT_SUB_TYPE='QTY' "
            + "and CONTRACT_TYPE='PD' "
            + "and (FLG_DEL='N' or FLG_DEL is null) "
            + "and PRINT_STATUS='INPROGRESS' "
            + "and FLG_CUT_DOWN<>'Y' "
            + "and PRODUCT_NAME is not null "
            + "and PRODUCT_FULL_NAME is not null "
            + "and t3.UID_PROMOTION is null "
            + "and UID_CUSTOMER='" + id_cus + "'  order by CONTRACT_NO asc";
            try
            {
                var result = _db.SelectQueryNoAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpGet("getcontracttreatment/{id_cus}")]
        public async Task<IActionResult> getContractTreatment(string id_cus)
        {
            string sql = "  SELECT distinct "
            + "UID_CUSTOMER,CONTRACT_NO,SELLING_DATE,EXPIRE_DATE,t1.DATA_AREA, "
            + "t2.PRODUCT_NAME,PRODUCT_FULL_NAME "
            + "FROM [T_CONTRACT_HEADER] t1 "
            + "left outer join [T_CONTRACT_SELLING] t2 "
            + "on t1.UID=t2.UID_CONTRACT_HEADER "
            + "left outer join [T_CONTRACT_SELLING_PROMOTION] t3 "
            + "on t1.UID=t3.UID_CONTRACT_H "
            + "left outer join [M_PROMOTION] t4 "
            + "on t3.UID_PROMOTION=t4.UID "
            + "where CONTRACT_SUB_TYPE='QTY' "
            + "and CONTRACT_TYPE='TM' "
            + "and (FLG_DEL='N' or FLG_DEL is null) "
            + "and PRINT_STATUS='INPROGRESS' "
            + "and FLG_CUT_DOWN<>'Y' "
            + "and PRODUCT_NAME is not null "
            + "and PRODUCT_FULL_NAME is not null "
            + "and t3.UID_PROMOTION is null "
            + "and UID_CUSTOMER='" + id_cus + "'  order by CONTRACT_NO asc";
            try
            {
                var result = _db.SelectQueryNoAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpGet("getcontractpackage/{id_cus}")]
        public async Task<IActionResult> getContractPackage(string id_cus)
        {
            string sql = "  SELECT distinct "
            + "UID_CUSTOMER,CONTRACT_NO,SELLING_DATE,EXPIRE_DATE,t1.DATA_AREA, "
            + "t2.PRODUCT_NAME,PRODUCT_FULL_NAME "
            + "FROM [T_CONTRACT_HEADER] t1 "
            + "left outer join [T_CONTRACT_SELLING] t2 "
            + "on t1.UID=t2.UID_CONTRACT_HEADER "
            + "left outer join [T_CONTRACT_SELLING_PROMOTION] t3 "
            + "on t1.UID=t3.UID_CONTRACT_H "
            + "left outer join [M_PROMOTION] t4 "
            + "on t3.UID_PROMOTION=t4.UID "
            + "where CONTRACT_SUB_TYPE='QTY' "
            + "and CONTRACT_TYPE='TM' "
            + "and (FLG_DEL='N' or FLG_DEL is null) "
            + "and PRINT_STATUS='INPROGRESS' "
            + "and FLG_CUT_DOWN<>'Y' "
            + "and PRODUCT_NAME is not null "
            + "and PRODUCT_FULL_NAME is not null "
            + "and t3.UID_PROMOTION is not null "
            + "and UID_CUSTOMER='" + id_cus + "'  order by CONTRACT_NO asc";
            try
            {
                var result = _db.SelectQueryNoAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpGet("getcontractpackagemoney/{id_cus}")]
        public async Task<IActionResult> getContractPackageMoney(string id_cus)
        {
            string sql = "  SELECT distinct "
            + "UID_CUSTOMER,CONTRACT_NO,SELLING_DATE,EXPIRE_DATE,t1.DATA_AREA "
            + ",t3.UID_PROMOTION,t4.PACKAGE_NAME,t4.PROMOTION_TYPE "
            + "FROM [T_CONTRACT_HEADER] t1 "
            + "left outer join [T_CONTRACT_SELLING] t2 "
            + "on t1.UID=t2.UID_CONTRACT_HEADER "
            + "left outer join [T_CONTRACT_SELLING_PROMOTION] t3 "
            + "on t1.UID=t3.UID_CONTRACT_H "
            + "left outer join [M_PROMOTION] t4 "
            + "on t3.UID_PROMOTION=t4.UID "
            + "where CONTRACT_SUB_TYPE='MONEY' "
            + "and CONTRACT_TYPE='TM' "
            + "and (FLG_DEL='N' or FLG_DEL is null) "
            + "and PRINT_STATUS='INPROGRESS' "
            + "and FLG_CUT_DOWN<>'Y' "
            + "and t3.UID_PROMOTION is not null "
            + "and UID_CUSTOMER='" + id_cus + "' "
            + "order by CONTRACT_NO asc";
            try
            {
                var result = _db.SelectQueryNoAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
        [AllowAnonymous]//ไม่ต้อง login
        [HttpGet("getbranch")]
        public async Task<IActionResult> getbranch()
        {
            string sql = "SELECT [UID] as value,[BRANCH_NAME] as label FROM [M_BRANCH] WHERE REC_STATUS='Y' "
           + "order by BRANCH_NAME asc";
            try
            {
                var result = _db.SelectQueryNoAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new _Response
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
    }
}