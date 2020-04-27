using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using UploadFilesServer.Context;
using UploadFilesServer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly GenesParserContext _context;

        public UploadController(GenesParserContext context)
        {
            _context = context;
        }
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            List<UserResult> DataList = new List<UserResult>();
            try
            {
                var allGeneResults = _context.GeneResults.ToList();
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    var fileLocation = new FileInfo(fullPath);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(fileLocation))
                    {

                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        //var workSheet = package.Workbook.Worksheets.First();
                        int totalRows = workSheet.Dimension.Rows;

                        //var DataList = new List<Customers>();

                        for (int i = 2; i <= totalRows; i++)
                        {
                            //DataList.Add(new Customers
                            {
                                GeneResult geneResult = new GeneResult {
                                    GeneName = workSheet.Cells[i, 1].Value.ToString(),
                                    Result = Int32.Parse(workSheet.Cells[i, 2].Value.ToString()),
                                    Text = workSheet.Cells[i, 3].Value.ToString(),
                                };
                                var entryToMod = _context.GeneResults.SingleOrDefault(i => i.GeneName == geneResult.GeneName && i.Result == geneResult.Result);
                                if (entryToMod==null)
                                {
                                    _context.GeneResults.Add(geneResult);
                                }
                                else
                                {
                                    if (entryToMod.Text != geneResult.Text)
                                    {
                                        entryToMod.Text = geneResult.Text;
                                        _context.Entry(entryToMod).State = EntityState.Modified;
                                    }                                                                  
                                }                                                           
                            }
                           
                        }
                        _context.SaveChanges();
                        ExcelWorksheet workSheet1 = package.Workbook.Worksheets[0];
                        //var workSheet = package.Workbook.Worksheets.First();
                        int totalRows1 = workSheet1.Dimension.Rows;


                        var allGeneResults1 = _context.GeneResults.ToList();
                        for (int i = 2; i <= totalRows1; i++)
                        {
                            string GeneName = workSheet.Cells[i, 1].Value.ToString();
                            int Result = Int32.Parse(workSheet.Cells[i, 2].Value.ToString());
                            string text = allGeneResults.Single(x => x.GeneName == GeneName && x.Result == Result).Text;

                            DataList.Add(new UserResult
                            {
                                GeneName = GeneName,
                                Text = text
                                //var GeneName = workSheet.Cells[i, 1].Value.ToString();
                                //var Results = workSheet.Cells[i, 2].Value.ToString();
                                //var CustomerCountry = workSheet.Cells[i, 3].Value.ToString();
                            });

                        }



                    }
                    return Ok(DataList);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        //public IActionResult Upload()
        //{
        //    try
        //    {
        //        var files = Request.Form.Files;
        //        var folderName = Path.Combine("StaticFiles", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        if (files.Any(f => f.Length == 0))
        //        {
        //            return BadRequest();
        //        }

        //        foreach (var file in files)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);

        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //        }

        //        return Ok("All the files are successfully uploaded.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}