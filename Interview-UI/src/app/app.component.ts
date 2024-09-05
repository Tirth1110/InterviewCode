import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUploadValidationCheckService } from './Services/FileUploadValidationCheck.service';
import { SwalToastService } from './Services/SwalToast.service';
import { StudentService } from './Services/student.service';
import { ResponseModel } from './Models/ResponseModel';
import { ResponseResultIncludes, AlertTitle, ToastType } from './Common/ToastType';
import { StudentListModel } from './Models/StudentListModel';
import Validation from './Common/MarkRangeValidator';
import { StudentSubjectForCreateDTO } from './Models/StudentSubjectForCreateDTO';
import { DatePipe } from '@angular/common';
import * as saveAs from 'file-saver';
import * as XLSX from 'xlsx';
import { StudentDetailsImport } from './Models/StudentDetailsImport';
export const DEFAULT_GUID = '00000000-0000-0000-0000-000000000000';
export const IMG_URL = 'https://localhost:7141/';
export const DEFAULT_IMG_URL = 'https://localhost:7141/Images/userprofile.png';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angular16';
  mode: string = 'add';
  profileImageUrl: string = '';
  studentForm!: FormGroup;
  studentSubjectForm!: FormGroup;
  studentId!: string;
  defaultGuid: string = DEFAULT_GUID;
  submitted: boolean = false;
  imgUrl: string = IMG_URL;
  defaultImgUrl: string = DEFAULT_IMG_URL;
  existingImageUrl: string = '';
  studentList: StudentListModel[] = [];
  studentSubjectForCreateDTO: StudentSubjectForCreateDTO = new StudentSubjectForCreateDTO();
  studentSubjectForCreateList: StudentSubjectForCreateDTO[] = [];

  studentDetailsImportObj = new StudentDetailsImport();
  studentDetailsImportList: StudentDetailsImport[] = [];
  studentDetailsExcelData: any;
  @ViewChild('uploadStudentExcel') uploadStudentExcel: ElementRef<HTMLInputElement> | undefined;
  constructor(private modalService: NgbModal, private formBuilder: FormBuilder,
    private _fileUploadValidationCheckService: FileUploadValidationCheckService, private _swalToastService: SwalToastService,
    private _studentService: StudentService, private datePipe: DatePipe, private httpClient: HttpClient,  private renderer: Renderer2
  ) { }

  ngOnInit() {
    this.initStudentForm();
    this.initStudentSubjectForm();
    this.getStudentList();
  }

  initStudentForm() {
    this.studentForm = this.formBuilder.group({
      studentName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      studentContactNo: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10), Validators.pattern('^[0-9]*$')]],
      studentEmail: ['', [Validators.required, Validators.email]],
      collegeName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      enrolmentNumber: ['', [Validators.required]],
      imageFile: [null, Validators.required],
    });
  }


  initStudentSubjectForm() {
    this.studentSubjectForm = this.formBuilder.group({
      subject1: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      subject1Mark: ['', [Validators.required, Validation.markRangeValidator]],

      subject2: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      subject2Mark: ['', [Validators.required, Validation.markRangeValidator]],

      subject3: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      subject3Mark: ['', [Validators.required, Validation.markRangeValidator]],

      subject4: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      subject4Mark: ['', [Validators.required, Validation.markRangeValidator]],

      subject5: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      subject5Mark: ['', [Validators.required, Validation.markRangeValidator]],
    });
  }

  get studentFormControl() {
    return this.studentForm.controls;
  }

  get studentSubjectFormControl() {
    return this.studentSubjectForm.controls;
  }

  profileImageChange(event: any) {
    if (this._fileUploadValidationCheckService.fileExtensionValidateForImage(this._fileUploadValidationCheckService.getFileExtension(event))) {
      this.studentForm.setErrors(null);
      this.studentForm.get('imageFile')?.updateValueAndValidity()
      // File Preview
      const reader = new FileReader();
      reader.onload = () => {
        this.profileImageUrl = reader.result as string;
      }
      reader.readAsDataURL(event.target.files[0]);
      this.studentForm.patchValue({
        imageFile: event.target.files[0]
      });
    } else {
      this.studentForm.setErrors({ invalid: true });
      this.studentForm.patchValue({
        imageFile: null
      });
      this._swalToastService.toast('Oops', 'Please select .png .jpg .jpeg or .gif file only', 'warning', false);
    }
  }

  onSubmit(studentForm: FormGroup) {
    this.submitted = true;
    if (this.studentForm.invalid) {
      return;
    }

    const formData = new FormData();
    formData.append('StudentId', this.studentId);
    formData.append('Name', studentForm.value.studentName);
    formData.append('Email', studentForm.value.studentEmail);
    formData.append('ContactNo', studentForm.value.studentContactNo);
    formData.append('CollegeName', studentForm.value.collegeName);
    formData.append('EnrolmentNumber', studentForm.value.enrolmentNumber);
    this.studentForm.get('imageFile')?.value ?
      formData.append('imageFile', this.studentForm.get('imageFile')?.value) : formData.append('profileImage', this.existingImageUrl);

    this._studentService.insertUpdate(formData).subscribe({
      next: (res: ResponseModel<any>) => {
        this._swalToastService.toast(res.message.includes(ResponseResultIncludes.CREATED) ? AlertTitle.CREATED : res.message.includes(ResponseResultIncludes.CHANGED) ? AlertTitle.CHANGED : res.message.includes(ResponseResultIncludes.ALREADY_EXISTS) ? AlertTitle.ALREADY_EXISTS : AlertTitle.SOMETHING_WRONG, res.message.toString(), res.message.includes(ResponseResultIncludes.SUCCESS) ? ToastType.SUCCESS : res.message.includes(ResponseResultIncludes.ALREADY_EXISTS) ? ToastType.WARNING : ToastType.ERROR, false);
        if (!res.message.includes(ResponseResultIncludes.ALREADY_EXISTS)) {
          this.studentForm.controls['imageFile'].setValue(null);
          this.modalService.dismissAll();
        }
        this.getStudentList();
      },
      error: (err: string) =>
        this._swalToastService.toast('Error', err.toString(), 'error', false)
    });
  }

  openAddEditModal(modelId: any, studentId: any) {
    this.submitted = false;
    this.studentId = studentId;
    if (this.studentId == DEFAULT_GUID) {
      this.profileImageUrl = '';
      this.mode = 'add'
      this.studentForm.get('imageFile')?.addValidators(Validators.required);
      this.studentForm.reset();
    } else {
      this.mode = 'edit'
      this.getStudentById(studentId);
    }
    this.modalService.open(modelId, { size: 'lg' });
  }


  getStudentById(studentId: string) {
    this._studentService.getById(studentId).subscribe({
      next: (res: ResponseModel<any>) => {
        this.studentForm.patchValue({
          studentName: res.data.name,
          studentContactNo: res.data.contactNo,
          studentEmail: res.data.email,
          collegeName: res.data.collegeName,
          enrolmentNumber: res.data.enrolmentNumber,
        })
        this.existingImageUrl = res.data.profileImage;
        this.profileImageUrl = res.data.profileImage != null || '' ? this.imgUrl + res.data.profileImage : this.defaultImgUrl;
        if (this.profileImageUrl) {
          this.studentForm.get('imageFile')?.clearValidators();
          this.studentForm.get('imageFile')?.updateValueAndValidity();
        }
      },
      error: (err: string) =>
        this._swalToastService.toast(AlertTitle.ERROR, err.toString(), ToastType.ERROR, false)
    });
  }

  getStudentList() {
    this._studentService.getAll().subscribe({
      next: (res: ResponseModel<any>) => {
        this.studentList = res.data as StudentListModel[];
        this.studentList.forEach((student) => {
          if (student.profileImage == "" || student.profileImage == null || student.profileImage == '') {
            student.imgPath = this.defaultImgUrl
          } else {
            student.imgPath = this.imgUrl + student.profileImage + '?v=' + this.getTimeStamp();
          }
        });
      },
      error: (err: string) =>
        this._swalToastService.toast(AlertTitle.ERROR, err.toString(), ToastType.ERROR, false)
    });
  }

  getTimeStamp(): number {
    return new Date().getTime();
  }


  openSubjectAddEditModal(modelId: any, studentId: any) {
    this.submitted = false;
    this.studentSubjectForm.reset();
    this.mode = 'add';
    this.studentId = studentId;
    this.modalService.open(modelId, { size: 'lg' });
  }

  onStudentSubjectSubmit(studentSubjectForm: FormGroup) {
    this.submitted = true;
    if (this.studentSubjectForm.invalid) {
      return;
    }
    this.studentSubjectForCreateDTO.studentId = this.studentId;
    this.studentSubjectForCreateDTO.subjectName = studentSubjectForm.value.subject1;
    this.studentSubjectForCreateDTO.marks = studentSubjectForm.value.subject1Mark;

    this.studentSubjectForCreateList.push(this.studentSubjectForCreateDTO);
    this.studentSubjectForCreateDTO = new StudentSubjectForCreateDTO();


    this.studentSubjectForCreateDTO.studentId = this.studentId;
    this.studentSubjectForCreateDTO.subjectName = studentSubjectForm.value.subject2;
    this.studentSubjectForCreateDTO.marks = studentSubjectForm.value.subject2Mark;
    this.studentSubjectForCreateList.push(this.studentSubjectForCreateDTO);
    this.studentSubjectForCreateDTO = new StudentSubjectForCreateDTO();

    this.studentSubjectForCreateDTO.studentId = this.studentId;
    this.studentSubjectForCreateDTO.subjectName = studentSubjectForm.value.subject3;
    this.studentSubjectForCreateDTO.marks = studentSubjectForm.value.subject3Mark;
    this.studentSubjectForCreateList.push(this.studentSubjectForCreateDTO);
    this.studentSubjectForCreateDTO = new StudentSubjectForCreateDTO();


    this.studentSubjectForCreateDTO.studentId = this.studentId;
    this.studentSubjectForCreateDTO.subjectName = studentSubjectForm.value.subject4;
    this.studentSubjectForCreateDTO.marks = studentSubjectForm.value.subject4Mark;
    this.studentSubjectForCreateList.push(this.studentSubjectForCreateDTO);
    this.studentSubjectForCreateDTO = new StudentSubjectForCreateDTO();

    this.studentSubjectForCreateDTO.studentId = this.studentId;
    this.studentSubjectForCreateDTO.subjectName = studentSubjectForm.value.subject5;
    this.studentSubjectForCreateDTO.marks = studentSubjectForm.value.subject5Mark;
    this.studentSubjectForCreateList.push(this.studentSubjectForCreateDTO);
    this.studentSubjectForCreateDTO = new StudentSubjectForCreateDTO();

    this._studentService.insertSubjectMarks(this.studentSubjectForCreateList).subscribe({
      next: (res: ResponseModel<any>) => {
        this._swalToastService.toast(res.message.includes(ResponseResultIncludes.CREATED) ? AlertTitle.CREATED : res.message.includes(ResponseResultIncludes.SOMETHING_WRONG) ? AlertTitle.SOMETHING_WRONG : '', res.message.toString(), res.message.includes(ResponseResultIncludes.CREATED) ? ToastType.SUCCESS : ToastType.ERROR, false);
        this.modalService.dismissAll();
        this.getStudentList();
      },
      error: (err: string) =>
        this._swalToastService.toast(AlertTitle.ERROR, err.toString(), ToastType.ERROR, false)
    });
  }

  async printPDF(studentId: any) {
    const res: ResponseModel<any> = (await this._studentService.getById(studentId).toPromise())!;
    debugger
    const fileName: string = res.data.name + '_' + this.datePipe.transform(new Date().toString(), 'yyyy-MM-dd HH:mm:ss')?.toString();

    this._studentService.exportPDF(studentId).subscribe({
      next: (response: Blob) => {
        const blobUrl = window.URL.createObjectURL(response);
        saveAs(blobUrl, fileName);
        window.URL.revokeObjectURL(blobUrl);
      },
      error: (error) => {
        this._swalToastService.toast(AlertTitle.ERROR, error.toString(), ToastType.ERROR, false);
      }
    });
  }

  downloadExcel() {
    const excelFileUrl = this.imgUrl + 'ExcelExample/StudentImport.xlsx';
    this.httpClient.get(excelFileUrl, { responseType: 'blob' }).subscribe({
      next: (blob: Blob) => {
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = 'StudentImport.xlsx';
        link.click();
      },
      error: (error) => {
        this._swalToastService.toast(AlertTitle.ERROR, error.toString(), ToastType.ERROR, false);
      }
    });
  }

  importStudent(event: any) {
    const selectedFile = event.target.files[0];
    if (this._fileUploadValidationCheckService.fileExtensionValidateForExcel(this._fileUploadValidationCheckService.getFileExtension(event))) {
      this.studentDetailsExcelData = null;
      const fileReader = new FileReader();
      fileReader.readAsBinaryString(selectedFile);

      fileReader.onload = (event: any) => {
        const binaryString = event.target.result;
        const workBook = XLSX.read(binaryString, { type: 'binary' });
        const worksheetName = workBook.SheetNames[0];
        this.studentDetailsExcelData = XLSX.utils.sheet_to_json(workBook.Sheets[worksheetName], { header: 1 });
        if (this.studentDetailsExcelData.length == 0) {
          this._swalToastService.toast(AlertTitle.ERROR, 'Please fill the data in excel sheet', ToastType.ERROR, false);
          return;
        }
        if (this.studentDetailsExcelData[0][0] != 'Name' || this.studentDetailsExcelData[0][1] != 'Email' || this.studentDetailsExcelData[0][2] != 'ContactNo' || this.studentDetailsExcelData[0][3] != 'CollegeName' || this.studentDetailsExcelData[0][4] != 'EnrolmentNumber') {
          this._swalToastService.toast(AlertTitle.ERROR, 'You have uploaded invalid excel format, please upload valid excel file', ToastType.ERROR, false);
          return;
        } else {
          for (let excelDataIndex = 1; excelDataIndex < this.studentDetailsExcelData?.length; excelDataIndex++) {
            this.studentDetailsImportObj.name = this.studentDetailsExcelData[excelDataIndex][0];
            this.studentDetailsImportObj.email = this.studentDetailsExcelData[excelDataIndex][1] == undefined || "" ? "" : this.studentDetailsExcelData[excelDataIndex][1].toString();
            this.studentDetailsImportObj.contactNo = this.studentDetailsExcelData[excelDataIndex][2] == undefined || "" ? "" : this.studentDetailsExcelData[excelDataIndex][2].toString();
            this.studentDetailsImportObj.collegeName = this.studentDetailsExcelData[excelDataIndex][3] == undefined || "" ? "" : this.studentDetailsExcelData[excelDataIndex][3].toString();
            this.studentDetailsImportObj.enrolmentNumber = this.studentDetailsExcelData[excelDataIndex][4] == undefined || "" ? "" : this.studentDetailsExcelData[excelDataIndex][4].toString();
            this.studentDetailsImportList.push(this.studentDetailsImportObj);
            this.studentDetailsImportObj = new StudentDetailsImport();
          }
          if (this.studentDetailsImportList?.length > 0) {
            this._studentService.updateStudentDataViaExcel(this.studentDetailsImportList).subscribe({
              next: (res: ResponseModel<any>) => {
                this._swalToastService.toast(res.message.includes(ResponseResultIncludes.SUCCESSFULLY) ? AlertTitle.SUCCESS : res.message.includes(ResponseResultIncludes.SUCCESSFULLY) ? AlertTitle.SUCCESS : res.message.includes(ResponseResultIncludes.ALREADY_EXISTS) ? AlertTitle.ALREADY_EXISTS : AlertTitle.SOMETHING_WRONG, res.message.toString(), res.message.includes(ResponseResultIncludes.SUCCESS) ? ToastType.SUCCESS : res.message.includes(ResponseResultIncludes.ALREADY_EXISTS) ? ToastType.WARNING : ToastType.ERROR, false);
                this.getStudentList();
                this.studentDetailsExcelData = null;
              },
              error: (err: string) =>
                this._swalToastService.toast('Error', err.toString(), 'error', false)
            });
          }
        }
      }
    } else {
      this._swalToastService.toast(AlertTitle.ERROR, 'Please upload .csv, .xlsx or .xls file', ToastType.ERROR, false);
    }
    this.renderer.setProperty(this.uploadStudentExcel?.nativeElement, 'value', '');
  }
}
