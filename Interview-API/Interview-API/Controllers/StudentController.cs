using DinkToPdf;
using DinkToPdf.Contracts;
using Interview.Entity.DTOs;
using Interview.Entity.Entities;
using Interview.Entity.ResponseHandler;
using Interview.Service.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Interview_API.Controllers
{
    public class StudentController(IStudentService studentService, IConverter pdfConverter) : BaseAPIController
    {
        private readonly IStudentService _studentService = studentService;
        private readonly IConverter _pdfConverter = pdfConverter;


        [HttpPost("InsertUpdate")]
        public async Task<IActionResult> InsertUpdate([FromForm] StudentForCreateDTO studentForCreateDTO)
        {
            if (await _studentService.ContactNoAlreadyExists(studentForCreateDTO))
            {
                return Ok(new ResponseModel<object>()
                {
                    Message = "Contact no is already exists",
                    Data = null,
                    TotalCount = 0
                });
            }
            else if (await _studentService.EmailIdAlreadyExists(studentForCreateDTO))
            {
                return Ok(new ResponseModel<object>()
                {
                    Message = "Email is already exists",
                    Data = null,
                    TotalCount = 0
                });
            }
            else
            {
                if (studentForCreateDTO.imageFile is not null)
                {
                    if (studentForCreateDTO.StudentId != Guid.Empty)
                    {
                        StudentForListDTO? studentDetails = await _studentService.GetByIdAsync(studentForCreateDTO.StudentId);
                        if (studentDetails?.ProfileImage is not null)
                        {
                            string existingImageUrl = Path.Combine("wwwroot", studentDetails?.ProfileImage ?? "");
                            if (!existingImageUrl.EndsWith("userprofile.png"))
                            {
                                if (System.IO.File.Exists(existingImageUrl))
                                    System.IO.File.Delete(existingImageUrl);
                            }
                        }
                    }
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(studentForCreateDTO.imageFile.FileName);
                    string folderName = Path.Combine("wwwroot", "Images", "StudentImages");

                    if (Directory.Exists(folderName))
                        studentForCreateDTO.profileImage = folderName;
                    else
                    {
                        Directory.CreateDirectory(folderName);
                        studentForCreateDTO.profileImage = folderName;
                    }
                    studentForCreateDTO.profileImage = Path.Combine(studentForCreateDTO.profileImage, newFileName);
                    using (FileStream fileStream = new FileStream(studentForCreateDTO.profileImage, FileMode.Create))
                    {
                        studentForCreateDTO.imageFile.CopyTo(fileStream);
                        studentForCreateDTO.profileImage = studentForCreateDTO.profileImage.Replace("wwwroot\\", "");
                    }
                }
                else if (studentForCreateDTO.profileImage is null)
                    studentForCreateDTO.profileImage = "Images\\userprofile.png";

                var student = await _studentService.AddUpdateAsync(studentForCreateDTO);
                if (student is not null)
                {
                    return Ok(new ResponseModel<object>()
                    {
                        Message = studentForCreateDTO.StudentId == Guid.Empty ? "Student created successfullly" : "Student details changed successfullly",
                        Data = null,
                        TotalCount = 0
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Message = "Student not found",
                        Data = null,
                        TotalCount = 0
                    });
                }
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            IEnumerable<StudentForListDTO>? students = await _studentService.GetAllAsync();
            return Ok(new ResponseModel<IEnumerable<StudentForListDTO>>()
            {
                Message = string.Empty,
                Data = students,
                LatestId = null,
                TotalCount = students.Count()
            });
        }

        [HttpGet("ExportPDF/{studentId}")]
        public async Task<ActionResult> ExportPDF(Guid studentId)
        {
            StudentForListDTO? student = await _studentService.GetByIdAsync(studentId);

            if (student is not null)
            {
                // Generate HTML content for the student
                StringBuilder htmlContent = new StringBuilder();
                htmlContent.Append("<html><body style='font-family: Arial, sans-serif; font-size: 12px;'>");
                htmlContent.Append("<h3> Marks Details : </h3>");
                htmlContent.Append("<table border='1'>");

                // Add table headers
                htmlContent.Append("<tr>");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Id");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Name");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Email");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Phone");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "College Name");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Total");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Percentage");
                htmlContent.AppendFormat("<th style='padding:12px; font-size: 14px;'>{0}</th>", "Grade");
                // Add more headers as needed
                htmlContent.Append("</tr>");

                // Add table rows with student data
                htmlContent.Append("<tr>");
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", 1);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.Name);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.Email);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.ContactNo);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.CollegeName);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.Total);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.Percentage);
                htmlContent.AppendFormat("<td style='padding:10px'>{0}</td>", student.Grade);
                // Add more properties as needed
                htmlContent.Append("</tr>");

                htmlContent.Append("</table></body></html>");
                GlobalSettings globalSettings = new()
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "PDF Report",

                };
                ObjectSettings objectSettings = new()
                {
                    PagesCount = true,
                    HtmlContent = htmlContent.ToString(),
                };
                return File(_pdfConverter.Convert(new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                }), "application/pdf", $"MarkDetails_of_{student.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            else
            {
                return Ok(new ResponseModel<string>()
                {
                    Message = "Student not found",
                    Data = null,
                    TotalCount = 0
                });
            }
        }

        [HttpGet("GetById/{studentId}")]
        public async Task<ActionResult> GetById(Guid studentId)
        {
            StudentForListDTO? student = await _studentService.GetByIdAsync(studentId);
            if (student is null)
            {
                return Ok(new ResponseModel<string>()
                {
                    Message = "Student not found",
                    Data = null,
                    TotalCount = 0
                });
            }
            else
            {
                return Ok(new ResponseModel<StudentForListDTO>()
                {
                    Message = string.Empty,
                    Data = student,
                    LatestId = null,
                    TotalCount = student is not null ? 1 : 0
                });
            }
        }

        [HttpPost("InsertSubjectMarks")]
        public async Task<IActionResult> InsertSubjectMarks(List<StudentSubjectForCreateDTO> studentSubjectForCreateDTOs)
        {
            bool rev = await _studentService.AddSubjectMarksAsync(studentSubjectForCreateDTOs);
            return Ok(new ResponseModel<Student>()
            {
                Message = rev ? "Subject created successfully" : "Something went wrong",
                Data = null,
                LatestId = null,
                TotalCount = 0
            });
        }

        [HttpPut("UploadStudentDataViaExcel")]
        public async Task<ActionResult> UploadStudentDataViaExcel([FromBody] List<StudentDetailsImport> studentDetailsImports)
        {
            bool rev = await _studentService.UploadStudentDataViaExcel(studentDetailsImports);
            return Ok(new ResponseModel<string>()
            {
                Message = rev ? "Student import operation completed successfully" : "Something went wrong",
                Data = null,
                LatestId = null,
                TotalCount = 0
            });
        }
    }
}